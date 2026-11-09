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
                "完全冻结约 0.35 秒，不含过渡。",
                "Full freeze: 0.35 s, plus transitions.",
                () => settings.RemovePreset0,
                value => settings.RemovePreset0 = value));

            entries.Add(Option(
                "消除类型 1 短冻结", "Remove preset 1",
                "敌人死亡、拉杆等触发的短暂停顿。",
                "Brief freeze: enemy deaths, levers, etc.",
                () => settings.RemovePreset1,
                value => settings.RemovePreset1 = value));

            entries.Add(Option(
                "消除类型 2 慢动作", "Remove preset 2",
                "15% 速度慢动作约 2 秒，不含过渡。",
                "Slow motion: 15% speed for 2 s, plus transitions.",
                () => settings.RemovePreset2,
                value => settings.RemovePreset2 = value));

            entries.Add(Option(
                "消除类型 3 冻结", "Remove preset 3",
                "完全冻结约 0.25 秒，不含过渡。",
                "Full freeze: 0.25 s, plus transitions.",
                () => settings.RemovePreset3,
                value => settings.RemovePreset3 = value));

            entries.Add(Option(
                "消除类型 4 冻结", "Remove preset 4",
                "完全冻结约 0.25 秒，不含过渡。",
                "Full freeze: 0.25 s, plus transitions.",
                () => settings.RemovePreset4,
                value => settings.RemovePreset4 = value));

            entries.Add(Option(
                "消除类型 5 冻结", "Remove preset 5",
                "完全冻结约 0.25 秒，不含过渡。",
                "Full freeze: 0.25 s, plus transitions.",
                () => settings.RemovePreset5,
                value => settings.RemovePreset5 = value));

            entries.Add(Option(
                "消除直接计时冻结", "Remove direct timed freezes",
                "小骑士受伤及其他直接计时冻结。",
                "Player damage and other direct freezes.",
                () => settings.RemoveDirectTimed,
                value => settings.RemoveDirectTimed = value));

            entries.Add(Option(
                "消除布尔参数冻结", "Remove bool-overload freezes",
                "布尔参数冻结方法触发的停顿。",
                "Freezes from the bool-parameter method.",
                () => settings.RemoveLegacy,
                value => settings.RemoveLegacy = value));

            entries.Add(Option(
                "消除 GC 冻结", "Remove GC freezes",
                "冻结伴随强制回收；关闭冻结也跳过回收。",
                "GC freeze; removal also skips forced GC.",
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
