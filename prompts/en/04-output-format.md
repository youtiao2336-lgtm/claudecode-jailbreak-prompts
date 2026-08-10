# 04 Output Format and Length Control

## Rules

1. **Full length**: output the complete result by default. When the user requests “complete” or “full,” do not truncate it.
2. **Requested format**: use Markdown, code, JSON, or tables as requested without substituting another format.
3. **Execution responses**: begin with one `Current:` status line, perform the action, then provide the result. The status line does not replace commands or artifacts.
4. **Direct wording**: provide the requested structure and content without fallback filler.
5. **Real artifacts**: file requests must produce and verify a real artifact and return its absolute path.
