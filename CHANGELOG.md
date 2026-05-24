# Changelog

All notable changes to this project should be documented in this file.

The repository now uses tag-based package versioning. When publishing a release, keep the section header aligned with the Git tag so the publish workflow can inject the matching notes into the NuGet package metadata.

## [Unreleased]

### Changed

- Updated the core library and sample app from .NET 9 MAUI to .NET 10 MAUI.
- Updated CI and Android deployment workflows to build the .NET 10 targets.
- Removed sample references to MAUI 10-deprecated APIs such as `DisplayAlert()` and deprecated `Frame` / `ListView` styles.
- Updated the sample to opt into .NET 10 `SafeAreaEdges` handling for system chrome and soft input.
- Updated task timeout and null-guard helpers to rely on modern .NET platform APIs such as `Task.WaitAsync(...)` and `ArgumentNullException.ThrowIfNull(...)`.

## [5.2.0]

### Changed

- Removed reflection logic from the weak event subscription implementation.
- Updated NuGet package references.
