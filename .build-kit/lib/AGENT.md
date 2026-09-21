# Agent Learnings

Patterns and gotchas discovered during task processing. Update this file whenever you encounter something reusable.

## Cratis non-negotiables (seed — full detail in `.claude/skills/_shared/cratis-conventions.md`)

- ALL backend artifacts for a slice go in ONE `.cs` file under the project's slice folder (the shipped starter uses `<Module>/<Feature>/<Slice>/<Slice>.cs` — discover the real top-level folder from an existing slice). Never split into `Commands/`, `Handlers/`, `Events/`.
- `[Command]` records define `Handle()` directly on the record — never a separate handler class.
- `[EventType]` takes NO attribute arguments (the type name is the id); events are past-tense and never nullable.
- Use `ConceptAs<T>` for every identity/value — no raw `Guid`/`string` in the domain.
- Read models: `[ReadModel]` record with `public static` query methods on it; observable queries return `ISubject<T>` directly (never `Task<ISubject<T>>`). Projections join events, never read models.
- Reactors implement the marker `IReactor`; dispatch is by the first parameter type. Write new events only via `ICommandPipeline.Execute(...)`, never `IEventLog`. Reactors must be idempotent and stateless.
- Namespace mirrors the folders and drops any `.Features.` segment: `<Root>.<Module>.<Feature>.<Slice>` (the starter's `<Root>` is `CratisApp`). Find `<Root>` from the `.csproj` `<RootNamespace>` / existing slices; never hard-code.
- `dotnet build` generates the TypeScript proxies — backend must compile before a slice's frontend can reference them. Order: Backend → build → Specs → Frontend → Composition.
- Quality gate: `dotnet build` with zero warnings/errors (warnings = errors); `dotnet test --filter "FullyQualifiedName~<SliceName>"`. File header only if the project's existing `.cs` files already carry one (the shipped example uses none).

## tasks.json

- Tasks are objects with `id`, `createdAt`, and `payload` (a `SliceChangedPayload`).
- After completing a task, remove it from the array entirely — do not add a status field.
- Write `[]` to `tasks.json` if the last task is completed.

## SliceChangedPayload fields

```
event           always "slice:changed"
organizationId  org UUID or null
boardId         board UUID
sliceId         SLICE_BORDER node UUID — use this with /load-slice
sliceTitle      human-readable slice name (may be null)
sliceStatus     e.g. "Created", "InProgress", "Done", "Blocked" (may be null)
timestamp       unix ms when the change was emitted
```

## Slice files

The realtime agent writes one file per slice on startup and after each `slice:changed` event:

```
.slices/<context>/<sliceName>/slice.json
```

- `<context>` is the slice's context value, or `default` if none.
- `<sliceName>` is the slice title lowercased with spaces removed (e.g. `"Enable User"` → `enableuser`).

These files are always up to date — read them directly before invoking any skill.

## Skill Usage

- Always run `/connect` first to load credentials from `.eventmodelers/config.json` before calling any other skill.
- `/load-slice sliceId=<uuid>` re-fetches all slices from the API, refreshes the slice files, and returns the requested slice. Use it when you need a guaranteed-fresh view of a specific slice.
- Read `.slices/<context>/<sliceName>/slice.json` directly when you already know the context and name and the file is recent enough.

## Board API

- The `boardId` and `organizationId` from each payload provide full context — pass them to skills.
- Node events use `node:created`, `node:changed`, `node:deleted` — always POST to `/api/org/:orgId/boards/:boardId/nodes/events`.
- Slice metadata (title, status) lives on the SLICE_BORDER node under `meta.sliceStatus` and `meta.title`.
- `/update-slice-status` rejects moving a slice into a status it's already in — this is a concurrency guard, not a bug. It means another agent already claimed the slice. Treat it as `ALREADY_IN_STATUS`, skip that slice, and move on to the next `Planned` one instead of erroring out.
- The local `.slices/<context>/<sliceName>/slice.json` file can be a stale, minimal placeholder (only `id`/`title`/`status`/`sliceType`). When it lacks fields/specs, fetch the real data with `mcp__eventmodelers__get_slice_data`, passing `contextName` as the CHAPTER/timeline title (e.g. `"Time Tracking"`), not the slice-index `contextName` value (`"default"`) — `list_slices` alone never returns field/spec detail.
- Before building an AUTOMATION/TRANSLATION slice, check the status of the slice that owns its trigger event and grep the codebase for that event type. If the owning slice isn't Planned/Done and the event type doesn't exist in code, the reactor can't compile (Cratis dispatches by first-parameter type) — this is a genuine blocker for `/request-feedback`, not something to guess past.
- `mcp__eventmodelers__get_slice_data`'s `contextName` must be the CHAPTER hosting the *slice's own* elements — it does not necessarily match the bounded-context implied by an externally-referenced trigger event's origin (e.g. a Time Tracking automation reacting to a Shift Management event still resolves under `contextName="Time Tracking"`, not `"Shift Management"`).
- `mcp__eventmodelers__add_comment` supports `type: "QUESTION"` in addition to `"COMMENT"`/`"TASK"` — prefer `QUESTION` for `/request-feedback` escalations.
- The "missing upstream event" blocker isn't limited to AUTOMATION/TRANSLATION/STATE_VIEW slices — a plain STATE_CHANGE command's own `Handle()` can also need data from an aggregate that doesn't exist yet (e.g. `ClockIn`'s "reject if >15 min before shift start" rule needs the Shift's scheduled start time, but no Shift/`ShiftCreated` exists anywhere in the codebase). Check every specification's business-rule wording, not just field mappings, for hidden cross-aggregate reads before assuming a command slice is self-contained.
- Quick sanity check for "does X exist in the codebase yet": `find . -iname "*.cs" -not -path "*/.build-kit/*" -not -path "*/bin/*" -not -path "*/obj/*"` lists every real source file outside the kit/build noise in one shot.
- The local `.slices/default/index.json` can lag the live board by more than per-slice staleness — whole slices can flip Created→Planned on the board before the local file catches up. Cross-check with `mcp__eventmodelers__list_slices` before deciding which slice is highest-priority-Planned.
- `mcp__eventmodelers__search_board_events(name=...)` searches the entire board, not just the current chapter/context — a `[]` result is a real signal that an entity/event was never modeled anywhere, not just that it's missing from the chapter you're looking at. Useful for confirming a whole missing-aggregate blocker (e.g. no "Employee" entity/event exists anywhere) rather than just a missing-precursor-slice blocker.
- This project has NO test project — a prior attempt to add `CratisApp.Specs` was reverted (commit "Removed specs project that was added incorrectly") because `CratisApp.csproj` (`Microsoft.NET.Sdk.Web`) globs `**/*.cs` by default, so nesting a test project's folder anywhere under the main project directory makes the main project try to compile the test project's files too (`CS0579` duplicate-attribute errors, missing `Xunit`/`Cratis.Specifications` refs). Fix: add to `CratisApp.csproj` an `<ItemGroup><Compile Remove="CratisApp.Specs/**/*.cs" /><Content Remove="CratisApp.Specs/**/*" /><None Remove="CratisApp.Specs/**/*" /></ItemGroup>` BEFORE adding any files under that folder. Recreate the project with `dotnet new xunit -n CratisApp.Specs -o CratisApp.Specs`, then edit its `.csproj` to `net9.0` (match the main TFM), add `PackageReference` for `Cratis.Specifications`, `Cratis.Specifications.XUnit`, `NSubstitute` (all `Version="*"`, matching the main csproj's wildcard convention), add a `<ProjectReference Include="..\CratisApp.csproj" />`, delete the scaffolded `UnitTest1.cs`, and add a `GlobalUsings.cs` with `global using Cratis.Specifications;` / `global using Xunit;`. Register it with `dotnet sln CratisApp.sln add CratisApp.Specs/CratisApp.Specs.csproj` (don't hand-edit the `.sln` — the CLI generates correct GUIDs/config sections). The specs project does NOT set `TreatWarningsAsErrors` (only the main csproj does) — nullable-field warnings (`CS8618`) on spec fixture fields (`RegisterAuthor _command;` with no initializer) are expected and don't fail the build; this matches the original (reverted) spec files' style, so don't "fix" them.
- A command field marked `"generated": true` in the slice JSON (e.g. an auto-generated id, or `Place Reservation`'s `Code`) is excluded from the `[Command]` record's parameters entirely — it's computed inside `Handle()` and only appears on the resulting `[EventType]` record. If that generated field is also `"idAttribute": true` (the aggregate's own id, not e.g. a business code), use the "generate-and-return" pattern: `Handle()` returns `(TId, TEvent)` with `TId.New()` generated inside, and the id is also passed into the event's constructor since the modeled event still lists it as one of its own fields.
- Cross-field command validation that doesn't need event-sourced state (e.g. "fromDay must not be later in the week than toDay", "endDateTime must be after startDateTime") belongs in the `CommandValidator<T>` via `RuleFor(c => c.PropertyA).Must((command, value) => ...comparison against command.PropertyB...)` — same idiom as the existing `PlaceReservationValidator`'s `RuleFor(c => c.End).GreaterThan(c => c.Start)`. Test these by instantiating the validator directly in a spec and calling `.Validate(command)`, asserting on the returned `FluentValidation.Results.ValidationResult.IsValid` — no HTTP/Chronicle fixture needed since the rule never touches the event store.
- A slice's namespace segment for the slice folder is legally allowed to equal its own command's type name (e.g. namespace `...Scheduling.CreateShift` containing `record CreateShift`) — same pattern as the existing `PlaceReservation` slice. When referencing that type from a different project/namespace (e.g. from specs), alias it to avoid any reader confusion: `using CreateShiftCommand = CratisApp.ShiftManagement.Scheduling.CreateShift.CreateShift;`.
- The Cratis proxy generator's `index.ts` barrel files are NOT fully regenerated/overwritten on every build once hand-edited — adding a second `export * from './Xyz'` line (e.g. for a hand-written `*Dialog.tsx`) survives subsequent `dotnet build` runs (`0 index files written, N unchanged`). Same pattern already used by `Reservations/Booking/PlaceReservation/index.ts` (exports both the generated command and the hand-written dialog).
- `ConceptAs<T>`, `EventSourceId`, `[Command]`, `[EventType]`, `[Key]`, `Result<,>`/`CommandValidator<T>` base types resolve with NO explicit `using` in slice `.cs` files even though `GlobalUsings.cs` only globally-usings `System.Reactive.Subjects` / `Cratis.Arc.MongoDB` / `MongoDB.Driver` / `Guid`-related — the Cratis SDK/analyzer package injects its own implicit global usings at build time. Don't add `using Cratis.Chronicle.Keys;` etc. to a concept file just because the type is unqualified; match the existing concept files (no using directives at all).
