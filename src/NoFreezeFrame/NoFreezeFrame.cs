using System.Collections;
using Modding;

namespace NoFreezeFrame
{
    public sealed partial class NoFreezeFrame : Mod, ITogglableMod
    {
        private bool hooksInstalled;

        public NoFreezeFrame() : base("NoFreezeFrame") { }

        public override string GetVersion() => "1.0.0";

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
                Log("Enabled: all four GameManager freeze entry points are blocked.");
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

        private static void SkipPreset(On.GameManager.orig_FreezeMoment_int orig, GameManager self, int type)
        {
            // Do not start a coroutine, change timeSlowedCount, or write the global time scale.
        }

        private static IEnumerator SkipTimed(
            On.GameManager.orig_FreezeMoment_float_float_float_float orig, GameManager self,
            float rampDownTime, float waitTime, float rampUpTime, float targetSpeed)
        {
            return CompletedCoroutine.Instance;
        }

        private static IEnumerator SkipLegacy(
            On.GameManager.orig_FreezeMoment_float_float_float_bool orig, GameManager self,
            float rampDownTime, float waitTime, float rampUpTime, bool runGc)
        {
            return CompletedCoroutine.Instance;
        }

        private static IEnumerator SkipWithGc(
            On.GameManager.orig_FreezeMomentGC orig, GameManager self,
            float rampDownTime, float waitTime, float rampUpTime, float targetSpeed)
        {
            // The freeze-specific forced collection is skipped as well; normal GC is untouched.
            return CompletedCoroutine.Instance;
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
