# Android App Store Deployment Guide

This guide covers the deployment of the MAUI Extensions Demo app to the Google Play Store.

## App Configuration

### App Identity
- **App Name**: MAUI Extensions Demo
- **Package ID**: `com.zoft.mauiextensions.demo`
- **Version**: 1.0 (Version Code: 1)

### Key Features
- Demonstrates zoft.MauiExtensions library capabilities
- Dependency injection patterns
- Localization examples
- Main thread operations
- Messaging patterns
- Input validation

## Prerequisites

### Development Environment
- .NET 9.0 SDK with MAUI workloads
- Android SDK (API 21-34)
- Java Development Kit (JDK 11+)

### Google Play Console
- Google Play Console developer account
- App registration in Play Console
- Compliance with Play Store policies

## Build Process

### Local Build
```bash
# Navigate to sample project
cd sample/zoft.MauiExtensions.Sample

# Restore dependencies
dotnet restore

# Build for Android
dotnet build -c Release -f net9.0-android
```

### GitHub Actions
The repository includes a GitHub workflow (`.github/workflows/android-deploy.yml`) that:
1. Builds the Android APK automatically
2. Uploads build artifacts
3. Provides deployment pipeline structure

## App Signing

### Development Signing
For development and testing, generate a debug keystore:
```bash
cd sample/zoft.MauiExtensions.Sample/PlayStore
./generate-keystore.sh
```

### Production Signing
For Play Store deployment:
1. **Option A**: Use Play App Signing (Recommended)
   - Upload an app bundle (.aab)
   - Google manages signing keys
   
2. **Option B**: Manual signing
   - Generate production keystore
   - Configure signing in project file
   - Store keystore securely

## Store Submission

### Required Assets
- **App Icon**: 512x512 PNG high-resolution icon
- **Feature Graphic**: 1024x500 PNG promotional image  
- **Screenshots**: Minimum 2 phone screenshots
- **Store Listing**: Title, description, keywords
- **Privacy Policy**: Required for all apps

### Metadata Files
All store metadata is prepared in the `PlayStore/` directory:
- `app-description.md` - Full app description
- `privacy-policy.md` - Privacy policy text
- `store-metadata.md` - Complete store listing information

### Submission Steps
1. **Prepare Release**
   - Build signed app bundle (.aab)
   - Test on physical devices
   - Verify all features work correctly

2. **Upload to Play Console**
   - Create new release in Play Console
   - Upload app bundle
   - Fill out store listing
   - Upload required graphics

3. **Review Process**
   - Submit for review
   - Address any policy violations
   - Monitor review status

## Compliance

### Privacy
- App collects no personal data
- Minimal permissions (Internet, Network State)
- Clear privacy policy provided

### Content Rating
- Target audience: Everyone
- No inappropriate content
- Educational/utility app category

### Technical Requirements
- Targets Android API 34
- Supports Android 5.0+ (API 21)
- 64-bit compatible
- Follows Material Design guidelines

## Monitoring

### Post-Launch
- Monitor crash reports in Play Console
- Review user feedback and ratings
- Track download and engagement metrics
- Plan updates based on user feedback

### Updates
- Use semantic versioning
- Test thoroughly before release
- Provide clear release notes
- Monitor rollout for issues

## Security

### Key Management
- Store signing keys securely
- Use environment variables for sensitive data
- Enable Play App Signing for automatic key management
- Backup signing keys safely

### GitHub Secrets
For automated deployment, configure these secrets:
- `ANDROID_KEYSTORE_FILE` - Base64 encoded keystore
- `ANDROID_KEYSTORE_PASSWORD` - Keystore password
- `ANDROID_KEY_ALIAS` - Key alias name
- `ANDROID_KEY_PASSWORD` - Key password

## Support

For issues with the deployment process:
1. Check GitHub Actions logs for build errors
2. Review Play Console for policy violations
3. Consult MAUI documentation for platform-specific issues
4. Open issues in the repository for deployment questions

---

**Note**: This is a demonstration app. For production apps, ensure proper security practices, comprehensive testing, and adherence to all applicable store policies.