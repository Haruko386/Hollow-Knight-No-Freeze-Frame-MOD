namespace NoFreezeFrame
{
    public sealed class FreezeSettings
    {
        public bool RemovePreset0 = true;
        public bool RemovePreset1 = true;
        public bool RemovePreset2 = true;
        public bool RemovePreset3 = true;
        public bool RemovePreset4 = true;
        public bool RemovePreset5 = true;

        public bool RemoveDirectTimed = true;
        public bool RemoveLegacy = true;
        public bool RemoveGc = true;

        public bool ShouldRemovePreset(int type)
        {
            switch (type)
            {
                case 0:
                    return RemovePreset0;
                case 1:
                    return RemovePreset1;
                case 2:
                    return RemovePreset2;
                case 3:
                    return RemovePreset3;
                case 4:
                    return RemovePreset4;
                case 5:
                    return RemovePreset5;
                default:
                    return false;
            }
        }
    }
}
