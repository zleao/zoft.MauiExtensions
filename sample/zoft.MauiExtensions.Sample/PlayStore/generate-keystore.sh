#!/bin/bash

# Script to generate Android signing keystore for demo app
# This is for demonstration purposes - in production, use secure key management

KEYSTORE_NAME="demo.keystore"
KEY_ALIAS="demo"
VALIDITY_DAYS=10000

echo "Generating Android signing keystore for MAUI Extensions Demo..."

# Generate keystore
keytool -genkey -v \
  -keystore $KEYSTORE_NAME \
  -alias $KEY_ALIAS \
  -keyalg RSA \
  -keysize 2048 \
  -validity $VALIDITY_DAYS \
  -dname "CN=MAUI Extensions Demo, OU=Development, O=zoft, L=Demo, ST=Demo, C=US" \
  -storepass demo123 \
  -keypass demo123

echo "Keystore generated: $KEYSTORE_NAME"
echo "Key alias: $KEY_ALIAS"
echo ""
echo "IMPORTANT: In production:"
echo "1. Use strong passwords"
echo "2. Store keystore securely"
echo "3. Use environment variables for passwords"
echo "4. Enable Play App Signing"
echo ""
echo "For GitHub Actions, add these secrets:"
echo "- ANDROID_KEYSTORE_FILE (base64 encoded keystore)"
echo "- ANDROID_KEYSTORE_PASSWORD"
echo "- ANDROID_KEY_ALIAS"
echo "- ANDROID_KEY_PASSWORD"