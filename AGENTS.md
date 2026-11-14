# APU Assault

VR FPS for Meta Quest (standalone). Unity 6000.6.0f1, URP, OpenXR + Meta OpenXR, XR Interaction Toolkit 3.7.0.

## Layout
- `Assets/_Project/` : all game content (Scripts, Scenes, Prefabs, ...). Scripts use the `APUAssault.*` namespaces (`Core`, `Weapons`, `Enemies`, `Editor`).
- `Assets/Samples/` : imported package samples (XRI Starter Assets: XR Origin prefab, input actions). Do not edit directly; copy to `_Project` first.
- `Assets/XR`, `Assets/XRI`: generated XR settings.

## Conventions
- Commit `.meta` files with their assets. Never commit `Library/`, `Temp/`, `Logs/`, `UserSettings/`.
- Heavy binaries (textures, models, audio, ...) go through Git LFS (see `.gitattributes`).
- Quest target: Android, IL2CPP, ARM64, Vulkan, min API 29, Single Pass Instanced.

## Start scene
Menu `APU Assault > Create Start Scene` builds `Assets/_Project/Scenes/Main.unity` (XR rig, ground, grabbable gun, one enemy).
