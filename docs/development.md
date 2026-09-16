# Development and validation

Run commands below from the repository root in PowerShell. The [CI workflow](../.github/workflows/ci.yml) is the source of truth for hosted builds.

## Environment

1. Read the SDK version in [global.json](../global.json); the [sample pin](../sample/zoft.MauiExtensions.Sample/global.json) should match.
2. Run `dotnet --list-sdks` and `dotnet --version` from the repository root. Install a compatible SDK for the pinned feature band if resolution fails. Having a newer SDK installed does not necessarily satisfy the pin; do not silently rewrite it or bypass it to claim a normal build passed.
3. Restore the project's MAUI workloads using the commands below. Initial setup needs network access and may need permission to install workloads.

CI builds the library and Windows sample on Windows. Windows validation requires the Windows SDK and MAUI tools. Android requires the Android SDK and JDK compatible with the selected MAUI workload. Apple app builds require a compatible macOS/Xcode environment (or the supported paired-Mac setup for iOS). Building the Windows sample does not verify Android, iOS, or Mac Catalyst behavior.

For platform setup details, use [Microsoft's MAUI installation guide](https://learn.microsoft.com/dotnet/maui/get-started/installation). A missing SDK or workload is an environment failure, not evidence of a source regression.

## Library and packages

```powershell
dotnet workload restore src/zoft.MauiExtensions.Core/zoft.MauiExtensions.Core.csproj --ignore-failed-sources --skip-manifest-update
dotnet restore src/zoft.MauiExtensions.Core/zoft.MauiExtensions.Core.csproj --ignore-failed-sources
dotnet build src/zoft.MauiExtensions.Core/zoft.MauiExtensions.Core.csproj -c Release --no-restore --nologo
```

Release builds generate XML documentation, `.nupkg`, and `.snupkg` files under `src/zoft.MauiExtensions.Core/bin/Release/`. A separate pack step is normally unnecessary. Analyzer feedback comes from the build; there is no separate repository lint command. `--ignore-failed-sources` does not make missing packages optional.

## Windows sample

```powershell
dotnet workload restore sample/zoft.MauiExtensions.Sample/zoft.MauiExtensions.Sample.csproj
dotnet build sample/zoft.MauiExtensions.Sample/zoft.MauiExtensions.Sample.csproj -f net10.0-windows10.0.19041.0 -c Release --nologo
```

The sample references the local core project. For interactive checks, launch it with the IDE's Windows target after a successful build. Use an Android emulator/device or Apple target separately when changing platform behavior.

## Test limitations

CI currently invokes:

```powershell
dotnet test src/zoft.MauiExtensions.sln --no-build --configuration Release --logger "trx;LogFileName=test_results.trx"
```

There are no dedicated test projects, so this is an entry-point smoke check, not an automated behavioral suite. Do not report it as test coverage. When adding automated tests, prioritize regressions in weak-subscription lifetime, busy-state cleanup, localization notifications, and task cancellation/timeout behavior; document the new commands here.

## Manual feature checks

Exercise the page relevant to a behavior change and record the platform and outcome:

| Area | Sample check |
| --- | --- |
| DependsOn | Change inputs and verify dependent properties and bound text update. |
| Localization | Switch between supported languages and confirm visible translations update without recreating the page. |
| MainThread | Execute the command; check busy feedback clears and the UI notification appears without a thread-access failure. |
| Messenger | Exercise the messaging controls and verify the expected receiver updates; navigate away and back to check for duplicate delivery. |
| Validation | Enter invalid then valid values and confirm error notifications update and clear. |

These UI checks do not establish garbage-collection behavior, disposal correctness, concurrency safety, or all error paths. Changes in those areas need focused automated coverage or an explicit reproducible harness. For busy-state changes, include success, exception, silent work, and overlapping operations when constructing that validation.
