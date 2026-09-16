# Android deployment documentation

The original deployment summary described an earlier setup and is no longer a current configuration or release-readiness checklist.

Use these maintained sources:

- [Android deployment guide](sample/zoft.MauiExtensions.Sample/ANDROID_DEPLOYMENT.md): setup, signing, workflow behavior, and deployment checks.
- [Android workflow](.github/workflows/android-deploy.yml): actual trigger, build, secrets, and Play upload configuration.
- [Sample project](sample/zoft.MauiExtensions.Sample/zoft.MauiExtensions.Sample.csproj): application identity, platform targets, version defaults, and `Resources/AppIcon/appicon.png`.
- [Development and validation](docs/development.md): local build commands and validation limits.
- [Versioning](docs/versioning.md): package versus sample app versions.

Store listing text and privacy documents remain under the sample's `PlayStore/` directory. Their presence does not establish that a build has passed device testing or current store review requirements.
