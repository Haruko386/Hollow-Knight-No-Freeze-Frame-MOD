using System.Collections.Generic;
using Modding;

namespace NoFreezeFrame
{
    public sealed partial class NoFreezeFrame : IMenuMod
    {
        public bool ToggleButtonInsideMenu => true;

        private static bool IsChinese => Language.Language.CurrentLanguage().ToString() == "ZH";

        public override string GetMenuButtonText() => IsChinese ? "消除冻结帧" : "No Freeze Frames";

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            var entries = new List<IMenuMod.MenuEntry>();
            if (toggleButtonEntry.HasValue)
            {
                // Keep the API's saver/loader: one switch controls both hooks and persistence.
                var toggle = toggleButtonEntry.Value;
                toggle.Name = IsChinese ? "消除冻结帧" : "Remove freeze frames";
                toggle.Description = IsChinese
                    ? "消除游戏顿帧与冻结慢动作，保留菜单暂停。返回后生效。"
                    : "Remove hitstop and freeze slowdowns; keep menu pause. Applied on leaving the menu.";
                entries.Add(toggle);
            }
            return entries;
        }
    }
}
