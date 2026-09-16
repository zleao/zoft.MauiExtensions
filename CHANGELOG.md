# Changelog

All notable changes to this project should be documented in this file.

The repository now uses tag-based package versioning. When publishing a release, keep the section header aligned with the Git tag so the publish workflow can inject the matching notes into the NuGet package metadata.

## [6.1.0]

### Changed

- Updated CommunityToolkit.Mvvm to 8.4.2 and Microsoft.Maui.Controls to 10.0.101 in the library and sample.
- Updated MinVer to 8.0.0 and Microsoft.SourceLink.GitHub to 10.0.401; kept SourceLink private to the build.
- Updated the sample's CommunityToolkit.Maui dependency to 15.0.1.
- Renamed the shared build properties file to `Directory.Build.props` for discovery on case-sensitive filesystems and removed the unsupported `MinVerMinimumMajorMinorPatch` property. Package versions remain Git-tag driven.
- Added tool-neutral repository guidance and development, architecture, and versioning documentation; removed Copilot-specific instructions.
- Corrected documentation for asynchronous work, model inheritance, and Android deployment.

## [6.0.0]

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
