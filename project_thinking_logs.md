# LearnForge: Decisions Log

Options we considered, what we chose, and when to revisit. Update this file every time we make or change a decision.

Status values: **Decided**, **Proposed** (needs confirmation), **Deferred**.

Last updated: 2026-09-21

---

## D1. One platform for all teachers
**Status:** Decided
- **Options:** (a) one shared platform where teachers create courses; (b) a separate site or deployment per teacher.
- **Chose:** (a). It is simpler to build, and learners keep one account and one leaderboard.
- **Revisit if:** teachers ask for their own domain or branding.

## D2. Content structure: Course > Unit > Lesson > Exercise
**Status:** Decided
- Every level has a `Position` for ordering (named `Position` because `Order` is a reserved word in SQL).

## D3. The "template": per-course settings stored in the database
**Status:** Decided
- The teacher's choices live in `CourseSettings`, not in code: streaks on/off, hearts on/off, max hearts, XP per exercise, weekly leaderboard on/off.
- **Not included yet:** which exercise types a course allows (planned in the roadmap).
- **Revisit if:** the settings list grows a lot (consider a JSON column).

## D4. Stack
**Status:** Decided
- .NET 10 (LTS), ASP.NET Core Web API with Controllers, EF Core + PostgreSQL, Identity + JWT, xUnit.
- Controllers were chosen over Minimal APIs for structure in a large API.

## D5. Architecture: 4 projects
**Status:** Decided
- `Domain <- Application <- Infrastructure`, and `Api` wires everything together.
- No extra abstractions until we actually need them.
- **Revisit if:** the structure starts to feel heavy for the size of the project.

## D6. Database: PostgreSQL installed locally on Windows
**Status:** Decided
- **Options:** local install (chosen), hosted free tier (Neon or Supabase), Docker (online Docker options were not a good fit).
- The connection string lives in configuration, so switching later is a config change.
- **Docker is deferred** to the deployment phase (roadmap phase 8).

## D7. Entity style
**Status:** Decided
- Private setters, constructors that validate, read-only collections exposed to the outside, and a private parameterless constructor for EF Core.
- `Course.TeacherId` is a plain `Guid`. Domain does not reference Identity.
- **Revisit in phase 2:** link `TeacherId` to Identity users (the user type will live in Infrastructure).

## D8. Exercise type-specific data stored as JSON
**Status:** Decided (for now)
- `ExerciseType`: MultipleChoice, FillInBlank, CodeOrdering, OutputPrediction.
- Type-specific data goes in `ContentJson` (mapped to PostgreSQL `jsonb`).
- **Revisit in phase 4:** typed content classes, and the Strategy pattern for checking answers.

## D9. Lesson teaching content as Markdown
**Status:** Proposed (needs confirmation)
- **Options:** (a) a `ContentMarkdown` field on `Lesson`; (b) ordered content blocks (Text, Code, Image, Video) in their own entity.
- **Proposed:** (a). It is much less work, and teachers write naturally with code blocks.
- `Exercise.Prompt` is Markdown too, so a question can include a code snippet.
- Frontend must render Markdown without raw HTML.
- **Revisit if:** teachers need interactive or richer elements inside lessons. Moving to blocks would need a data migration.

## D10. Exercises are attached to lessons
**Status:** Decided (for now)
- **Options:** (a) exercises belong to a lesson; (b) a standalone exercise bank; (c) separate challenges.
- **Chose:** (a) for now.
- **Future:** we may add challenges or standalone exercises.
- **Note for later:** `Exercise.LessonId` is currently required. Supporting standalone exercises will likely mean making it optional or linking exercises to lessons and challenges through a join table. Expect a migration plus changes to the teacher and learner APIs.

---

## Revisit list

- [ ] Phase 2: link `Course.TeacherId` to Identity users (D7)
- [ ] Phase 4: typed exercise content and answer-checking strategies (D8)
- [ ] Phase 5: streak logic and time zones
- [ ] Phase 8: Docker and deployment (D6)
- [ ] Add "allowed exercise types" to `CourseSettings` (D3)
- [ ] Confirm the lesson content approach (D9)
- [ ] Future: challenges or standalone exercises (D10)
- [ ] Future: content blocks for lessons, if Markdown is not enough (D9)