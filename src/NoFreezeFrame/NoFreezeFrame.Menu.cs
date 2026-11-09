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
                () => settings.RemovePreset0,
                value => settings.RemovePreset0 = value));

            entries.Add(Option(
                "消除类型 1 短冻结", "Remove preset 1",
                () => settings.RemovePreset1,
                value => settings.RemovePreset1 = value));

            entries.Add(Option(
                "消除类型 2 慢动作", "Remove preset 2",
                () => settings.RemovePreset2,
                value => settings.RemovePreset2 = value));

            entries.Add(Option(
                "消除类型 3 冻结", "Remove preset 3",
                () => settings.RemovePreset3,
                value => settings.RemovePreset3 = value));

            entries.Add(Option(
                "消除类型 4 冻结", "Remove preset 4",
                () => settings.RemovePreset4,
                value => settings.RemovePreset4 = value));

            entries.Add(Option(
                "消除类型 5 冻结", "Remove preset 5",
                () => settings.RemovePreset5,
                value => settings.RemovePreset5 = value));

            entries.Add(Option(
                "消除直接计时冻结", "Remove direct timed freezes",
                () => settings.RemoveDirectTimed,
                value => settings.RemoveDirectTimed = value));

            entries.Add(Option(
                "消除布尔参数冻结", "Remove bool-overload freezes",
                () => settings.RemoveLegacy,
                value => settings.RemoveLegacy = value));

            entries.Add(Option(
                "消除 GC 冻结", "Remove GC freezes",
                () => settings.RemoveGc,
                value => settings.RemoveGc = value));

            return entries;
        }

        private IMenuMod.MenuEntry Option(
            string chineseName,
            string englishName,
            Func<bool> read,
            Action<bool> write)
        {
            return new IMenuMod.MenuEntry
            {
                Name = IsChinese ? chineseName : englishName,

                Description = IsChinese
                    ? "开启：消除此类冻结。关闭：保留。需要开启模组总开关。"
                    : "On removes this freeze; Off preserves it. Requires the mod enabled.",

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