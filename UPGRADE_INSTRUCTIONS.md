# Unity Project Upgrade Instructions

## Version Upgrade Path
- **Original**: Unity 2017.3.0f3
- **Previous Upgrade**: Unity 2022.3.20f1 LTS  
- **Current Upgrade**: Unity 6.3 LTS (6000.3.0f1)

> [!IMPORTANT]
> This project has been upgraded to Unity 6.3 LTS. For detailed Unity 6 migration instructions, see [UNITY_6_MIGRATION_GUIDE.md](UNITY_6_MIGRATION_GUIDE.md).

### Changes Made for Unity 6.3 LTS
1. ✅ Updated `ProjectVersion.txt` to Unity 6000.3.0f1
2. ✅ Updated package manifest with Unity 6 compatible packages
3. ✅ Added XR Interaction Toolkit 3.0.10 (replaces VRTK)
4. ✅ Added XR Management 5.0.1 and OpenXR 1.15.0
5. ✅ Updated UGUI to 2.0.0 and TextMeshPro to 3.0.9

## Previous Unity 2022 Upgrade (Historical Reference)

### Important Steps When Opening in Unity 2022

#### 1. **Open the Project**
   - Install Unity 2022.3.20f1 LTS from Unity Hub if you haven't already
   - Open the project in Unity 2022.3.20f1
   - Unity will automatically start upgrading the project

#### 2. **Let Unity Complete the Upgrade**
   - Unity will reimport all assets automatically
   - This process may take 10-30 minutes depending on your system
   - **DO NOT** interrupt this process

#### 3. **Check for Breaking Changes**

   **VRTK (VR Toolkit):**
   - VRTK v3.2.1 (used in this project) may have compatibility issues with Unity 2022
   - Consider upgrading to VRTK v4 or migrating to XR Interaction Toolkit (Unity's official VR solution)
   - Check: `Assets/VRTK/` folder

   **SteamVR:**
   - SteamVR plugin may need updating for Unity 2022
   - Ensure you have the latest SteamVR Unity Plugin from the Asset Store or GitHub
   - Check: `Assets/SteamVR/` folder

   **ZFBrowser:**
   - May need updates for Unity 2022 compatibility
   - Check: `Assets/ZFBrowser/` folder

#### 4. **Update Packages**
   - Open **Window → Package Manager**
   - Update all packages to their latest compatible versions
   - Install **XR Plugin Management** and **OpenXR Plugin** for VR support

#### 5. **VR Settings Configuration**
   - Go to **Edit → Project Settings → XR Plug-in Management**
   - Enable **OpenXR** or **OpenVR** (for HTC Vive)
   - Configure VR settings for your HTC Vive headset

#### 6. **Script Compilation Errors**
   - After upgrade, check the **Console** for any script errors
   - Common issues:
     - Deprecated API calls (Unity 2017 → 2022 has many API changes)
     - Namespace changes
     - Component reference changes
   - Fix errors one by one, testing after each fix

#### 7. **Test VR Functionality**
   - Test all VR interactions
   - Verify HTC Vive controllers work correctly
   - Check that all VRTK scripts function properly

#### 8. **Backup Before Testing**
   - **IMPORTANT:** Make a backup of your project before making changes
   - Consider using Git to track changes

### Known Compatibility Issues

1. **VRTK v3 → Unity 2022:**
   - VRTK v3 was designed for Unity 2017-2019
   - May require significant code updates
   - Consider migrating to VRTK v4 or XR Interaction Toolkit

2. **SteamVR Plugin:**
   - Older versions may not work with Unity 2022
   - Update to latest SteamVR Unity Plugin

3. **Shader Compatibility:**
   - Some shaders may need recompilation
   - Check for shader errors in the Console

4. **Input System:**
   - Unity 2022 uses the new Input System by default
   - Old Input Manager may still work but consider migrating

### Recommended Next Steps

1. **Immediate:**
   - Open project in Unity 2022.3.20f1
   - Let Unity complete the automatic upgrade
   - Fix any compilation errors

2. **Short-term:**
   - Update VRTK to v4 or migrate to XR Interaction Toolkit
   - Update SteamVR plugin
   - Test all VR interactions

3. **Long-term:**
   - Consider migrating from VRTK to Unity's XR Interaction Toolkit (official Unity solution)
   - Update all third-party plugins
   - Optimize for Unity 2022 features

### Resources

- [Unity 2022.3 LTS Release Notes](https://unity.com/releases/editor/whats-new/2022.3.20)
- [Unity Upgrade Guide](https://docs.unity3d.com/Manual/UpgradeGuide.html)
- [VRTK v4 Migration Guide](https://www.vrtk.io/)
- [XR Interaction Toolkit Documentation](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest)

### Support

If you encounter issues:
1. Check Unity Console for error messages
2. Review Unity upgrade logs
3. Check plugin documentation for Unity 2022 compatibility
4. Consider creating a new Unity 2022 project and migrating assets gradually

---

**Note:** This is a major version jump (2017 → 2022), so expect some breaking changes. The automatic upgrade will handle most asset conversions, but scripts and plugins may need manual updates.
