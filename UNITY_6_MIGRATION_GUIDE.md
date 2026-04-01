# Unity 6.3 LTS Migration Guide - Skills-Lab VR Nursing Simulator

## Overview
This guide provides instructions for migrating the Skills-Lab VR Nursing Simulator from Unity 2022.3.62f3 to Unity 6.3 LTS (6000.3.0f1).

## Critical Changes from Unity 2022 to Unity 6

### VR/XR System Changes

> [!WARNING]
> **VRTK v3.2.1 is NOT Compatible with Unity 6**
> 
> The project currently uses VRTK v3.2.1, which does not work with Unity 6's XR Plugin Management system. You have two options:
> 
> 1. **Recommended**: Migrate to Unity's XR Interaction Toolkit 3.0.10 (already added to manifest)
> 2. **Alternative**: Upgrade to VRTK v4 (more complex, requires significant code changes)

### Package Updates
The following packages have been updated in `Packages/manifest.json`:

- **XR Interaction Toolkit**: 2.x → 3.0.10
- **XR Management**: 4.x → 5.0.1
- **OpenXR Plugin**: 1.x → 1.15.0
- **UI (UGUI)**: 1.0.0 → 2.0.0
- **TextMeshPro**: 3.0.6 → 3.0.9

### Breaking API Changes

Unity 6 has deprecated several APIs that were already updated in the Unity 2022 migration:

✅ **Already Fixed** (from previous Unity 2022 upgrade):
- `FindObjectsOfType` → `FindObjectsByType` (in DrawerManager.cs)
- `FindGameObjectWithTag` → `FindWithTag` (in PullSyringe.cs, Inventory.cs)

❌ **New Unity 6 Deprecations**:
- Enlighten Baked Global Illumination removed
- Graphics formats `DepthAuto`, `ShadowAuto`, `VideoAuto` obsolete
- Navigation system moved to AI Navigation package

## Migration Steps

### 1. Backup Your Project
```cmd
xcopy "C:\Users\skavi\Downloads\VR PROJECT\skills-lab" "C:\Users\skavi\Downloads\VR PROJECT\skills-lab-backup" /E /I /H
```

### 2. Install Unity 6.3 LTS
- Open Unity Hub
- Go to Installs → Add
- Select Unity 6.3 LTS (6000.3.0f1)
- Include modules: Windows Build Support, Android Build Support (if needed)

### 3. Open Project in Unity 6.3 LTS
- In Unity Hub, click "Open" and select the skills-lab folder
- Unity will detect the version mismatch and prompt to upgrade
- Click "Confirm" to proceed with the upgrade
- **Wait 10-30 minutes** for Unity to reimport all assets

### 4. Configure XR Settings

#### Option A: Using OpenXR (Recommended)
1. Go to **Edit → Project Settings → XR Plug-in Management**
2. Enable **OpenXR**
3. Click **OpenXR** in the left panel
4. Add **HTC Vive Controller Profile** under Interaction Profiles
5. Configure OpenXR Runtime to use SteamVR

#### Option B: Using OpenVR Legacy
1. Install SteamVR Unity Plugin from Asset Store
2. Configure in XR Plug-in Management

### 5. Handle VRTK Migration

#### If Migrating to XR Interaction Toolkit:

The XR Interaction Toolkit 3.0.10 has been added to your package manifest. You'll need to:

1. **Review VRTK Scripts**:
   - Identify all scripts using VRTK namespaces
   - Map VRTK interactions to XR Interaction Toolkit equivalents

2. **Common Mappings**:
   - `VRTK_InteractableObject` → `XRGrabInteractable`
   - `VRTK_ControllerEvents` → `XRController` + Input Actions
   - `VRTK_Pointer` → `XRRayInteractor`

3. **Update Prefabs**:
   - Replace VRTK controller prefabs with XR Interaction Toolkit rigs
   - Reconfigure interaction layers

### 6. Fix Compilation Errors

After Unity completes the upgrade:

1. Open **Console** window (Window → General → Console)
2. Check for errors (red messages)
3. Common issues:
   - VRTK namespace errors (if not migrated)
   - Deprecated API warnings
   - Missing package references

### 7. Test VR Functionality

- [ ] VR headset detected and displays correctly
- [ ] Controller tracking works
- [ ] Drawer system functions
- [ ] Syringe pull mechanics work
- [ ] Inventory system works
- [ ] Medicine spawning works
- [ ] Patient interaction works
- [ ] XML data loading works

## Known Issues

### SteamVR Plugin
- Update to latest SteamVR Unity Plugin if using OpenVR
- Compatible with Unity 6 when using OpenXR

### ZFBrowser
- Compatibility with Unity 6 unknown
- Test thoroughly after upgrade
- May need to update or replace

### Shader Compatibility
- Some shaders may need recompilation
- Check Console for shader errors
- Most standard shaders should auto-upgrade

## Performance Improvements in Unity 6

Unity 6 offers significant performance improvements:
- **GPU Resident Drawer**: Optimizes rendering of complex scenes
- **Adaptive Probe Volumes**: Better lighting performance
- **Improved multithreading**: Better CPU utilization
- **AssetBundle TypeTree optimization**: 97-99% reduction in runtime memory

## Rollback Instructions

If you encounter critical issues:

1. Close Unity
2. Restore from backup:
   ```cmd
   rmdir "C:\Users\skavi\Downloads\VR PROJECT\skills-lab" /S /Q
   xcopy "C:\Users\skavi\Downloads\VR PROJECT\skills-lab-backup" "C:\Users\skavi\Downloads\VR PROJECT\skills-lab" /E /I /H
   ```
3. Open with Unity 2022.3.62f3

## Resources

- [Unity 6.3 LTS Release Notes](https://unity.com/releases/editor/whats-new/6000.3.0)
- [Unity 6 Upgrade Guide](https://docs.unity3d.com/6000.3/Documentation/Manual/UpgradeGuides.html)
- [XR Interaction Toolkit 3.0 Documentation](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.0/manual/index.html)
- [OpenXR Plugin Documentation](https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.15/manual/index.html)
- [VRTK to XR Interaction Toolkit Migration](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest)

---

**Last Updated**: 2026-02-10  
**Target Unity Version**: 6000.3.0f1 (Unity 6.3 LTS)
