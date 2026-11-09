using System;
using System.Collections.Generic;
using Modding;

namespace NoFreezeFrame
{
    public sealed partial class NoFreezeFrame : IMenuMod
    {
        public bool ToggleButtonInsideMenu => true;

        private static bool IsChinese =>
            Language.Language.CurrentLanguage().ToString() == "ZH";

        public override string GetMenuButtonText() =>
            IsChinese ? "消除冻结帧" : "No Freeze Frames";

        public List<IMenuMod.MenuEntry> GetMenuData(
            IMenuMod.MenuEntry? toggleButtonEntry)
        {
            var entries = new List<IMenuMod.MenuEntry>();

            if (toggleButtonEntry.HasValue)
            {
                var toggle = toggleButtonEntry.Value;

                toggle.Name = IsChinese
                    ? "开启无冻结帧"
                    : "Enable mod";

                toggle.Description = IsChinese
                    ? "效果将会在返回时应用"
                    : "Applied on leaving the menu.";

                entries.Add(toggle);
            }

            entries.Add(Option(
                "消除类型 0 冻结", "Remove preset 0",
                "类型 0：约 0.35 秒的完全冻结，不含过渡时间。",
                "Preset 0: a roughly 0.35-second full freeze, excluding transitions.",
                () => settings.RemovePreset0,
                value => settings.RemovePreset0 = value));

            entries.Add(Option(
                "消除类型 1 短冻结", "Remove preset 1",
                "类型 1：敌人死亡、拉杆等可能触发的短暂冻结。",
                "Preset 1: a brief freeze used by enemy deaths, levers, and other events.",
                () => settings.RemovePreset1,
                value => settings.RemovePreset1 = value));

            entries.Add(Option(
                "消除类型 2 慢动作", "Remove preset 2",
                "类型 2：以 15% 速度持续约 2 秒的慢动作，不含过渡时间。",
                "Preset 2: roughly 2 seconds at 15% speed, excluding transitions.",
                () => settings.RemovePreset2,
                value => settings.RemovePreset2 = value));

            entries.Add(Option(
                "消除类型 3 冻结", "Remove preset 3",
                "类型 3：约 0.25 秒的完全冻结；与类型 4、5 独立设置。",
                "Preset 3: a roughly 0.25-second full freeze, controlled independently of 4 and 5.",
                () => settings.RemovePreset3,
                value => settings.RemovePreset3 = value));

            entries.Add(Option(
                "消除类型 4 冻结", "Remove preset 4",
                "类型 4：约 0.25 秒的完全冻结；与类型 3、5 独立设置。",
                "Preset 4: a roughly 0.25-second full freeze, controlled independently of 3 and 5.",
                () => settings.RemovePreset4,
                value => settings.RemovePreset4 = value));

            entries.Add(Option(
                "消除类型 5 冻结", "Remove preset 5",
                "类型 5：约 0.25 秒的完全冻结；与类型 3、4 独立设置。",
                "Preset 5: a roughly 0.25-second full freeze, controlled independently of 3 and 4.",
                () => settings.RemovePreset5,
                value => settings.RemovePreset5 = value));

            entries.Add(Option(
                "消除直接计时冻结", "Remove direct timed freezes",
                "骑士受伤及其他直接计时冻结；不影响类型 0–5 预设。",
                "Player damage and other direct timed freezes; does not control presets 0–5.",
                () => settings.RemoveDirectTimed,
                value => settings.RemoveDirectTimed = value));

            entries.Add(Option(
                "消除布尔参数冻结", "Remove bool-overload freezes",
                "通过布尔参数版本的冻结方法触发的停顿；场景取决于调用方。",
                "Freezes requested through the bool overload; usage depends on the caller.",
                () => settings.RemoveLegacy,
                value => settings.RemoveLegacy = value));

            entries.Add(Option(
                "消除 GC 冻结", "Remove GC freezes",
                "冻结与强制垃圾回收结合的特殊效果；开启时也会跳过该次强制回收。",
                "Freezes with forced garbage collection; removal also skips that collection.",
                () => settings.RemoveGc,
                value => settings.RemoveGc = value));

            return entries;
        }

        private IMenuMod.MenuEntry Option(
            string chineseName,
            string englishName,
            string chineseDescription,
            string englishDescription,
            Func<bool> read,
            Action<bool> write)
        {
            return new IMenuMod.MenuEntry
            {
                Name = IsChinese ? chineseName : englishName,

                Description = IsChinese ? chineseDescription : englishDescription,

                Values = IsChinese
                    ? new[] { "关闭", "开启" }
                    : new[] { "Off", "On" },

                Loader = () => read() ? 1 : 0,

                Saver = index =>
                {
                    write(index == 1);
                    SaveGlobalSettings();
                }
            };
        }
    }
}
