# NoFreezeFrame

Removes global freeze frames and hitstop in **Hollow Knight 1.5.78.11833**. Requires **Modding API 74**.

## Usage

1. Download [v1.0.0](https://github.com/Haruko386/Hollow-Knight-No-Freeze-Frame-MOD/releases/tag/v1.0.0).
2. Extract the `NoFreezeFrame` folder into `hollow_knight_Data/Managed/Mods/`.
3. Open **Options → Mods → No Freeze Frames** to enable or disable the mod. Changes apply when leaving the menu, and the setting is saved between sessions.

Enabled by default. Normal menu pause is preserved. To uninstall, close the game and remove the `NoFreezeFrame` folder.

## Build

Requires the .NET SDK and a local game installation with Modding API installed.

```powershell
# Build and package
.\build.ps1 -GamePath 'D:\Games\Hollow Knight'

# Build and install (close the game first)
.\build.ps1 -GamePath 'D:\Games\Hollow Knight' -Install
```

Output: `artifacts/NoFreezeFrame/NoFreezeFrame.dll` and `artifacts/NoFreezeFrame-1.0.0.zip`.

## References

- [Hollow Knight Modding API](https://github.com/hk-modding/api)
- [IMenuMod](https://hk-modding.github.io/api/api/Modding.IMenuMod.html)
- [ITogglableMod](https://hk-modding.github.io/api/api/Modding.ITogglableMod.html)

## License

[Apache License 2.0](LICENSE).
