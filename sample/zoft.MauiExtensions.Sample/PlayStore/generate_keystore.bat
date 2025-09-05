@echo off
REM Script to generate Android signing keystore for demo app
REM This is for demonstration purposes - in production, use secure key management

set "KEYSTORE_NAME=demo.keystore"
set "KEY_ALIAS=demo"
set "VALIDITY_DAYS=10000"

echo Generating Android signing keystore for MAUI Extensions Demo...

REM Use keytool from JDK 21 (edit this path if your JDK is elsewhere)
set "KEYTOOL=C:\Program Files (x86)\Android\openjdk\jdk-17.0.14\bin\keytool.exe"

"%KEYTOOL%" -genkey -v ^
  -keystore "%KEYSTORE_NAME%" ^
  -alias "%KEY_ALIAS%" ^
  -keyalg RSA ^
  -keysize 2048 ^
  -validity %VALIDITY_DAYS% ^
  -dname "CN=MAUI Extensions Demo, OU=Development, O=zoft, L=Demo, ST=Demo, C=US" ^
  -storepass demo123 ^
  -keypass demo123

echo Keystore generated: %KEYSTORE_NAME%
echo Key alias: %KEY_ALIAS%
echo(
echo IMPORTANT: In production:
echo 1. Use strong passwords
echo 2. Store keystore securely
echo 3. Use environment variables for passwords
echo 4. Enable Play App Signing
echo(
echo For GitHub Actions, add these secrets:
echo - ANDROID_KEYSTORE_FILE (base64 encoded keystore)
echo - ANDROID_KEYSTORE_PASSWORD
echo - ANDROID_KEY_ALIAS
echo - ANDROID_KEY_PASSWORD
pause
