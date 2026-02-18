import anthropic
import json
import os
import subprocess
import requests
import sys

# ── Config ────────────────────────────────────────────────────────────────────

ANDREW_GITHUB_PRBOT = os.environ["ANTHROPIC_API_KEY"]
GITHUB_TOKEN      = os.environ["GITHUB_TOKEN"]
REPO              = os.environ["REPO"]           # e.g. "yourname/yourrepo"
PR_NUMBER         = os.environ["PR_NUMBER"]
BASE_SHA          = os.environ["BASE_SHA"]
HEAD_SHA          = os.environ["HEAD_SHA"]

GH_HEADERS = {
    "Authorization": f"Bearer {GITHUB_TOKEN}",
    "Accept":        "application/vnd.github+json",
    "X-GitHub-Api-Version": "2022-11-28",
}

# ── Gather context ────────────────────────────────────────────────────────────

def get_diff() -> str:
    result = subprocess.run(
        ["git", "diff", f"{BASE_SHA}...{HEAD_SHA}"],
        capture_output=True, text=True, check=True
    )
    return result.stdout

def get_readme() -> str:
    for name in ["README.md", "readme.md", "README.txt"]:
        if os.path.exists(name):
            return open(name).read()[:3000]  # cap at 3k chars
    return "No README found."

def get_pr_description() -> str:
    url = f"https://api.github.com/repos/{REPO}/pulls/{PR_NUMBER}"
    resp = requests.get(url, headers=GH_HEADERS)
    data = resp.json()
    return data.get("body") or "No PR description provided."

# ── Call Claude ───────────────────────────────────────────────────────────────

SYSTEM_PROMPT = """You are a senior software engineer performing a thorough PR review.

Your priorities in order:
1. Bugs and logic errors — these are blockers
2. Opportunities to reduce code — shorter is better when clarity is maintained
3. Performance improvements (algorithmic only, not micro-optimizations)
4. Idiomatic patterns for the language/framework in use

Rules:
- Be direct and specific. No praise for doing basics correctly.
- Only comment on things that genuinely matter.
- If a section of the diff is fine, say nothing about it.
- Prefer suggesting concrete replacement code over abstract advice.

Respond ONLY with a valid JSON object in exactly this structure (no markdown fences):
{
  "summary": "One paragraph overall assessment.",
  "verdict": "APPROVE" | "REQUEST_CHANGES" | "COMMENT",
  "comments": [
    {
      "path": "relative/path/to/file.py",
      "line": <integer, the line number in the new file>,
      "body": "Your specific comment here, with suggested fix if applicable."
    }
  ]
}

If there are no issues, return an empty comments array and verdict APPROVE."""

def run_review(diff: str, readme: str, pr_body: str) -> dict:
    client = anthropic.Anthropic(api_key=ANTHROPIC_API_KEY)

    user_message = f"""Project context (README):
{readme}

PR description from author:
{pr_body}

Diff to review:
{diff}"""

    response = client.messages.create(
        model="claude-sonnet-4-6",
        max_tokens=4096,
        system=SYSTEM_PROMPT,
        messages=[{"role": "user", "content": user_message}]
    )

    raw = response.content[0].text.strip()

    try:
        return json.loads(raw)
    except json.JSONDecodeError:
        # Claude occasionally wraps output in ```json fences despite instructions
        import re
        match = re.search(r"\{.*\}", raw, re.DOTALL)
        if match:
            return json.loads(match.group())
        raise

# ── Post to GitHub ────────────────────────────────────────────────────────────

def get_pr_commit_id() -> str:
    url = f"https://api.github.com/repos/{REPO}/pulls/{PR_NUMBER}"
    return requests.get(url, headers=GH_HEADERS).json()["head"]["sha"]

def post_review(review: dict, commit_id: str):
    url = f"https://api.github.com/repos/{REPO}/pulls/{PR_NUMBER}/reviews"

    # GitHub requires comments to reference a valid position in the diff.
    # We post inline comments only when we have a line number.
    gh_comments = []
    for c in review.get("comments", []):
        if c.get("path") and c.get("line"):
            gh_comments.append({
                "path":     c["path"],
                "line":     c["line"],
                "side":     "RIGHT",
                "body":     c["body"],
            })

    payload = {
        "commit_id": commit_id,
        "body":      review.get("summary", ""),
        "event":     review.get("verdict", "COMMENT"),
        "comments":  gh_comments,
    }

    resp = requests.post(url, headers=GH_HEADERS, json=payload)

    if resp.status_code not in (200, 201):
        print(f"GitHub API error {resp.status_code}: {resp.text}")
        sys.exit(1)
    else:
        print(f"Review posted successfully. Verdict: {review.get('verdict')}")

# ── Main ──────────────────────────────────────────────────────────────────────

if __name__ == "__main__":
    diff    = get_diff()
    readme  = get_readme()
    pr_body = get_pr_description()

    if not diff.strip():
        print("Empty diff — nothing to review.")
        sys.exit(0)

    # Truncate massive diffs to avoid token limits
    if len(diff) > 60000:
        diff = diff[:60000] + "\n\n[Diff truncated — too large for single review]"

    review    = run_review(diff, readme, pr_body)
    commit_id = get_pr_commit_id()

    post_review(review, commit_id)
