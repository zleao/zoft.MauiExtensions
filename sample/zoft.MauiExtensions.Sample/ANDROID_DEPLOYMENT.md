# Android deployment guide

The [Android workflow](../../.github/workflows/android-deploy.yml) is the source of truth for automation. The [sample project](zoft.MauiExtensions.Sample.csproj) defines application identity, target frameworks, minimum platform versions, and the PNG app icon. See [versioning](../../docs/versioning.md) for the distinction between NuGet and application versions.

## Local setup and validation

Follow [development setup](../../docs/development.md) to install an SDK compatible with the repository's `global.json` and the MAUI workloads. Use the Android SDK and JDK required by that workload; do not rely on fixed Android/JDK versions from historical deployment summaries.

From the repository root:

```powershell
dotnet workload restore sample/zoft.MauiExtensions.Sample/zoft.MauiExtensions.Sample.csproj
dotnet build sample/zoft.MauiExtensions.Sample/zoft.MauiExtensions.Sample.csproj -c Release -f net10.0-android
```

Launch a debug build on an emulator or physical device with your IDE to exercise the feature checks in the development guide. A successful build alone does not establish device behavior or store readiness. The project configures Release Android packaging as AAB.

## Workflow behavior

The workflow is manually triggered with `workflow_dispatch`; pushes and pull requests do not trigger it.

1. A Windows runner installs the SDK and Android workload, restores dependencies, and reconstructs the keystore from a secret.
2. It computes `ApplicationVersion` from the current year concatenated with the workflow run number, then publishes the signed Android output. `ApplicationDisplayVersion` stays at the project value.
3. It uploads `*-Signed.aab` files as the `maui-android-aab` artifact with 30-day retention. Some step names still say APK; the configured artifact is AAB.
4. A dependent job uses the `production` environment and uploads the AAB to Google Play's `internal` track with status `draft`. This is an external upload, not just a build. Any environment protection rules are configured in GitHub, outside this repository.

Before a deployment, check that the computed application version code exceeds previously uploaded codes. Concatenating a year and an unpadded run number is not a general guarantee of monotonic version codes across year changes.

## Signing and credentials

Configure the secrets consumed by the workflow:

| Secret | Purpose |
| --- | --- |
| `ANDROID_KEYSTORE_BASE64` | Base64-encoded signing keystore. |
| `ANDROID_KEYSTORE_PASSWORD` | Keystore password. |
| `ANDROID_KEY_ALIAS` | Signing key alias. |
| `ANDROID_KEY_PASSWORD` | Signing key password. |
| `SERVICE_ACCOUNT_JSON` | Google Play service-account credentials for the upload job. |

Keep keys and credentials out of source control and logs. Confirm that the signing key and Play account match the application being updated.

The repository contains a Windows demo helper, [generate_keystore.bat](PlayStore/generate_keystore.bat). Inspect and configure it before running: it contains a machine-specific `KEYTOOL` path and demo credentials. It is not a production key-management procedure. There is no `generate-keystore.sh` in this repository.

## Store material and release checks

The `PlayStore/` directory contains [app description](PlayStore/app-description.md), [privacy policy](PlayStore/privacy-policy.md), and [store metadata](PlayStore/store-metadata.md) drafts. Review these against actual app behavior and current Play Console requirements before submission; historical checklists do not prove current compliance.

Before uploading, verify the application ID, signing identity, display version and version code, test the intended release on Android devices, and review the artifact. Confirm that store graphics, listing, privacy disclosures, and account configuration are ready in Play Console. The workflow's internal draft upload does not itself publish a production release.
