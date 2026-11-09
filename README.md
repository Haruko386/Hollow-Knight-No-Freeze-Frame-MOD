# NoFreezeFrame

Removes global freeze frames and hitstop in **Hollow Knight 1.5.78.11833**. Requires **Modding API 74**.

## Usage

1. Download [v1.0.1](https://github.com/Haruko386/Hollow-Knight-No-Freeze-Frame-MOD/releases/tag/v1.0.1).
2. Extract the `NoFreezeFrame` folder into `hollow_knight_Data/Managed/Mods/`.
3. Open **Options → Mods → No Freeze Frames** to enable the mod and choose which freezes to remove. The mod toggle applies when leaving the menu. Settings are saved between sessions.

All removal options are enabled by default. Each option removes a freeze type when On and preserves it when Off:

- **Presets 0–5** control the game's six numbered freeze presets. Different events may use the same preset.
- **Direct timed freezes** include the player's damage freeze and other direct calls to the timed freeze method.
- **Bool-overload freezes** and **GC freezes** control the corresponding freeze methods.

Normal menu pause is preserved. Performance stutters and freezes implemented outside these methods are not covered. To uninstall, close the game and remove the `NoFreezeFrame` folder.

## Build

Requires the .NET SDK and a local game installation with Modding API installed.

```powershell
# Build and package
.\build.ps1 -GamePath 'D:\Games\Hollow Knight'

# Build and install (close the game first)
.\build.ps1 -GamePath 'D:\Games\Hollow Knight' -Install
```

Output: `artifacts/NoFreezeFrame/NoFreezeFrame.dll` and `artifacts/NoFreezeFrame-1.0.1.zip`.

## References

- [Hollow Knight Modding API](https://github.com/hk-modding/api)
- [IMenuMod](https://hk-modding.github.io/api/api/Modding.IMenuMod.html)
- [ITogglableMod](https://hk-modding.github.io/api/api/Modding.ITogglableMod.html)

## License

[Apache License 2.0](LICENSE).
