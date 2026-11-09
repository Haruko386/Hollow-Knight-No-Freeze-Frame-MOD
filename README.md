# NoFreezeFrame — 空洞骑士消除冻结帧

适用于 **Hollow Knight 1.5.78.11833 + Modding API 74**，默认开启。只依赖已经安装的 Modding API，不需要 Satchel 或其他前置模组。

[下载 v1.0.0](https://github.com/Haruko386/Hollow-Knight-No-Freeze-Frame-MOD/releases/tag/v1.0.0) · [发布说明](docs/releases/v1.0.0.md)

## 使用

将发布包内 `NoFreezeFrame` 文件夹放到游戏的 `hollow_knight_Data/Managed/Mods/` 下。

进入 **选项 → 模组（Mods）→ 消除冻结帧（No Freeze Frames）**，切换开/关后返回菜单即可应用，无需重启。开关由 API 保存到存档目录的 `ModdingApi.GlobalSettings.json`，正常退出游戏后下次启动仍保留。关闭后恢复原版冻结效果。

卸载时退出游戏，移除 `Managed/Mods/NoFreezeFrame` 文件夹即可。

## 覆盖范围

拦截游戏的全部四个全局冻结入口，不按攻击种类、敌人或场景筛选：

| 入口 | 处理 |
| --- | --- |
| `GameManager.FreezeMoment(int)` | 跳过全部预设，包括 0–5 和其他整数参数 |
| `FreezeMoment(float, float, float, float)` | 立即结束，取消减速、停顿和恢复等待 |
| `FreezeMoment(float, float, float, bool)` | 立即结束，覆盖另一种协程入口 |
| `FreezeMomentGC(float, float, float, float)` | 立即结束，同时跳过该入口附带的强制 GC |

覆盖普通命中、击杀、玩家受伤，以及经这些入口触发的特殊演出与慢动作。PlayMaker 消息和其他模组调用这些方法也会被拦截。协程返回有效的空枚举器，允许原来的受伤等流程继续。

正常菜单暂停、切换场景、输入控制、敌人硬直/击退和演出动画本身保留。不会每帧强设 `Time.timeScale = 1`。其他模组如果绕过这些入口自行实现停顿，不在这四个钩子的覆盖范围内。性能掉帧、磁盘加载和一般 GC 卡顿也不是本模组消除的游戏冻结效果。开关应用前已经启动的原版冻结协程会正常结束。

## 构建

需要 .NET SDK，以及本地已安装 API 的游戏。引用游戏自身 DLL，但不在发布包中分发它们。

```powershell
.\build.ps1
.\build.ps1 -Install
.\build.ps1 -GamePath 'D:\Games\Hollow Knight' -Install
```

输出：`artifacts/NoFreezeFrame/NoFreezeFrame.dll` 和 `artifacts/NoFreezeFrame-1.0.0.zip`。

## 验证状态

已使用本机游戏程序集编译通过（0 警告、0 错误），游戏的 ModLog 已确认 API 成功加载 `NoFreezeFrame 1.0.0` 并安装四个钩子。尚未完成实战与菜单交互验证。

建议在同一场战斗分别打开、关闭模组，对比命中、击杀、玩家受伤及特殊冻结演出的停顿；再检查暂停/恢复、切换场景和重新启动后的开关记忆。

## 实现依据

- [Modding API 官方仓库](https://github.com/hk-modding/api)
- [IMenuMod 接口文档](https://hk-modding.github.io/api/api/Modding.IMenuMod.html)
- [ITogglableMod 接口文档](https://hk-modding.github.io/api/api/Modding.ITogglableMod.html)

具体签名和调用路径以本机 1.5.78.11833 / API 74 程序集为准。原游戏的 `ScaleTime` PlayMaker 动作已经禁用，`GameManager.SetTimeScale` 同时服务暂停菜单，因此只拦截专用冻结入口。
