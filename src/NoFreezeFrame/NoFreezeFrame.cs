using System.Collections;
using Modding;

namespace NoFreezeFrame
{
    public sealed partial class NoFreezeFrame : Mod, ITogglableMod, IGlobalSettings<FreezeSettings>
    {
        private bool hooksInstalled;

        private FreezeSettings settings = new FreezeSettings();
        private int presetDispatchDepth;

        public NoFreezeFrame() : base("NoFreezeFrame") { }

        public override string GetVersion() => "1.0.1";

        public override void Initialize()
        {
            if (hooksInstalled)
                return;

            // Cover every overload, including direct calls and PlayMaker SendMessage calls.
            // Roll back partial registration if the API cannot install a hook.
            try
            {
                On.GameManager.FreezeMoment_int += SkipPreset;
                On.GameManager.FreezeMoment_float_float_float_float += SkipTimed;
                On.GameManager.FreezeMoment_float_float_float_bool += SkipLegacy;
                On.GameManager.FreezeMomentGC += SkipWithGc;
                hooksInstalled = true;
                Log("Enabled: freeze filters installed.");
            }
            catch
            {
                RemoveHooks();
                throw;
            }
        }

        public void Unload()
        {
            if (!hooksInstalled)
                return;

            RemoveHooks();
            hooksInstalled = false;
            Log("Disabled: original freeze behavior restored.");
        }

        private void RemoveHooks()
        {
            On.GameManager.FreezeMoment_int -= SkipPreset;
            On.GameManager.FreezeMoment_float_float_float_float -= SkipTimed;
            On.GameManager.FreezeMoment_float_float_float_bool -= SkipLegacy;
            On.GameManager.FreezeMomentGC -= SkipWithGc;
        }

        public void OnLoadGlobal(FreezeSettings loaded)
        {
            settings = loaded ?? new FreezeSettings();
        }

        public FreezeSettings OnSaveGlobal()
        {
            return settings;
        }

        private void SkipPreset(On.GameManager.orig_FreezeMoment_int orig, GameManager self, int type)
        {
            if (settings.ShouldRemovePreset(type)) return;

            presetDispatchDepth++;

            try
            {
                orig(self, type);
            }
            finally
            {
                presetDispatchDepth--;
            }
        }

        private IEnumerator SkipLegacy(On.GameManager.orig_FreezeMoment_float_float_float_bool orig, GameManager self, float rampDownTime, float waitTime, float rampUpTime, bool runGc)
        {
            if (presetDispatchDepth == 0 && settings.RemoveLegacy) return CompletedCoroutine.Instance;

            return orig(self, rampDownTime, waitTime, rampUpTime, runGc);
        }

        private IEnumerator SkipWithGc(On.GameManager.orig_FreezeMomentGC orig, GameManager self, float rampDownTime, float waitTime, float rampUpTime, float targetSpeed)
        {
            if (presetDispatchDepth == 0 && settings.RemoveGc)
                return CompletedCoroutine.Instance;

            return orig(self, rampDownTime, waitTime, rampUpTime, targetSpeed);
        }

        private IEnumerator SkipTimed(On.GameManager.orig_FreezeMoment_float_float_float_float orig, GameManager self, float rampDownTime, float waitTime, float rampUpTime, float targetSpeed)
        {
            if (presetDispatchDepth == 0 && settings.RemoveDirectTimed) return CompletedCoroutine.Instance;

            return orig(self, rampDownTime, waitTime, rampUpTime, targetSpeed);
        }

        // Stateless and reusable, with no yields and no per-hit allocation. Never return null:
        // callers are allowed to pass this result straight to StartCoroutine.
        private sealed class CompletedCoroutine : IEnumerator
        {
            internal static readonly IEnumerator Instance = new CompletedCoroutine();
            public object Current => null;
            public bool MoveNext() => false;
            public void Reset() { }
        }
    }
}
