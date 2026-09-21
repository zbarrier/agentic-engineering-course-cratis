# Agent Learnings

This file was missing at the start of this iteration. Accumulated reusable learnings for this
project have historically been kept in `progress.txt` under a `## Codebase Patterns` header at the
very top of that file (not in this file). **Read that section too, not just this file.**

## Board-specific notes

- This board has no `MODEL_CONTEXT` node. `mcp__eventmodelers__get_slice_data` requires a
  `contextName`/`contextId` that resolves to a real context or timeline node — passing the local
  folder slug `"default"` (used only for `.build-kit/.slices/default/...` paths) fails with
  "No context or timeline found matching default". For slices on this board, resolve identity via
  `mcp__eventmodelers__list_slices` + `mcp__eventmodelers__get_node` instead of `get_slice_data`,
  and when persisting a `Created`-status slice that has no commands/events/queries elaborated yet,
  the minimal `{id, title, status, sliceType}` shape (matching existing files under
  `.build-kit/.slices/default/*/slice.json`) is sufficient — there's nothing richer to fetch until
  the slice is elaborated on the board.
- Slice status is not a one-way ratchet: a slice previously driven to `Blocked` (or any other
  status) by a past task iteration can later show up back at `Created` on the live board (confirmed
  via `list_slices`), independent of anything this agent did. Don't trust a slice's last-known
  status from `progress.txt` history — always re-check current status via
  `mcp__eventmodelers__list_slices` before deciding what action a `slice:changed` task needs.
