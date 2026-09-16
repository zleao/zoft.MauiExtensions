# Repository guidance

## Start here

This repository publishes `zoft.MauiExtensions.Core`, a .NET MAUI extension library built on CommunityToolkit.Mvvm. The sample references the local library and demonstrates intended API usage.

- `src/zoft.MauiExtensions.Core/`: published library; models, localization, markup, weak subscriptions, and extensions.
- `sample/zoft.MauiExtensions.Sample/`: Shell app with one page per feature; inspect the matching viewmodel and view before changing behavior.
- `src/Directory.Build.props`: library build and package metadata; it does not apply to the sample.
- `.github/workflows/`: executable source of truth for CI, NuGet publishing, and Android deployment.

Read only the task-relevant guides:

- [Development and validation](docs/development.md): SDK setup, commands, platform limits, and manual checks.
- [Architecture and behavioral constraints](docs/architecture.md): ownership, threading, localization, and compatibility.
- [Versioning and releases](docs/versioning.md): MinVer, tags, release notes, and sample application versions.
- [Android deployment](sample/zoft.MauiExtensions.Sample/ANDROID_DEPLOYMENT.md): signing and deployment workflow.

## Editing conventions

- Preserve existing user changes. Keep edits focused; avoid unrelated formatting or dependency upgrades.
- Read SDK and dependency versions from `global.json` and project files rather than copying versions into instructions. Keep the root and sample SDK pins aligned when intentionally changing them.
- Follow `.editorconfig` for new code. Existing files contain both namespace styles; avoid restyling entire files. Preserve public namespace spelling and casing.
- Use CommunityToolkit.Mvvm generators (`ObservableProperty`, `RelayCommand`, and notification hooks) and the appropriate `ZoftObservable*` base class for sample viewmodels.
- Preserve compiled XAML bindings (`x:DataType`), constructor injection, and registrations in `MauiProgram.cs`.
- Document new public library APIs with XML comments. Consider source and binary compatibility when changing public signatures, namespaces, constraints, or behavior.
- Keep generated files out of edits: `bin/`, `obj/`, `src/ReleaseNotes.g.props`, and resource designer output. Change resource inputs and regenerate designer output with the appropriate tooling when necessary.
- Package versions come from Git tags through MinVer; do not add fixed `Version` or `PackageVersion` properties.

## Validation and reporting

Use the commands and feature checks in [development.md](docs/development.md). There are no dedicated automated test projects: a successful `dotnet test` invocation does not prove behavior was tested. Report exactly what was built or exercised and any missing SDK, workload, device, or platform validation. For documentation-only edits, check links, referenced paths, and consistency with project/workflow files; a full MAUI build is not required.

Update the relevant guide when changing build commands, release behavior, architecture, or validation requirements. Keep this file concise and tool-neutral, with detailed guidance in the linked documents. Assistants that do not discover `AGENTS.md` automatically should be directed to read it explicitly.
