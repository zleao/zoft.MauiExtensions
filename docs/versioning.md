# Versioning and releases

## Package version

The core project's MinVer package computes the NuGet and assembly versions from Git history. Read the installed MinVer version from the [project file](../src/zoft.MauiExtensions.Core/zoft.MauiExtensions.Core.csproj). There is no fixed `PackageVersion` in the repository.

- A valid version tag on the current commit supplies the version directly, e.g. `6.0.0` or `6.1.0-beta.1`.
- After an unprefixed stable tag `6.0.0`, the default for later commits is `6.0.1-alpha.0.<height>`, where height is the number of commits since that tag.
- Tags use no `v` prefix; neither MinVer configuration nor the publish trigger is configured for prefixed tags.
- Use full Git history and tags. The library CI and package publish workflows use `fetch-depth: 0`.
- Uncommitted edits do not create a new Git height; a modified checkout at a release tag can still calculate that release version.

`MinVerMinimumMajorMinorPatch` is not a supported setting and has been removed. If a future development line needs a major/minor floor, deliberately set `MinVerMinimumMajorMinor`, for example `6.1`, in [Directory.Build.props](../src/Directory.Build.props). This selects a development range, not an exact release version; an exact tag on the current commit still takes precedence. No minimum is currently configured. See [MinVer's documentation](https://github.com/adamralph/minver#can-i-bump-the-major-or-minor-version).

The props filename must retain the exact casing `Directory.Build.props` for automatic discovery on case-sensitive filesystems. Its directory scope covers the library, not the sample.

After restoring dependencies with the pinned SDK, inspect the calculated version without compiling:

```powershell
dotnet msbuild src/zoft.MauiExtensions.Core/zoft.MauiExtensions.Core.csproj -nologo -t:MinVer -getProperty:MinVerVersion,PackageVersion,Version
```

Run the target: evaluating properties without `-t:MinVer` may show SDK defaults before MinVer has calculated the version.

## NuGet release flow

The [publish workflow](../.github/workflows/publish-package.yml) defines the process:

1. Prepare and validate the release commit. Add a matching `## [MAJOR.MINOR.PATCH]` (or prerelease) section to [CHANGELOG.md](../CHANGELOG.md).
2. Create and push the intended unprefixed semantic-version tag when releasing. A tag push triggers publishing to NuGet.org and creation/update of the GitHub release.
3. The workflow restores workloads and packages, builds Release packages, and checks that the `.nupkg` filename version matches the pushed tag before publishing.

The changelog supplies release notes, not the package version. CI writes escaped notes into `src/ReleaseNotes.g.props`, which is imported by the shared props file and git-ignored. Do not edit or commit that generated file. Missing changelog notes produce a warning rather than blocking the release.

Manual `workflow_dispatch` accepts a ref and defaults `publish_to_nuget` to false, allowing package artifact builds. Setting it to true publishes packages. The tag/package equality check runs only on tag pushes; manual publishing can publish a calculated prerelease from an untagged ref. Exact-tag release notes are extracted only when applicable. GitHub release creation runs only on tag pushes.

## Sample app versions

The sample has separate `ApplicationDisplayVersion` and integer `ApplicationVersion` properties in its project file. These do not come from the NuGet version or the library props file.

The [Android workflow](../.github/workflows/android-deploy.yml) overrides `ApplicationVersion` by concatenating the current year and workflow run number. It does not override `ApplicationDisplayVersion`. See the [Android guide](../sample/zoft.MauiExtensions.Sample/ANDROID_DEPLOYMENT.md) for deployment; changing the library release tag does not update these application version fields.
