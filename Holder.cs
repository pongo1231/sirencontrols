using CitizenFX.Core;

namespace SirenControls {
    static class Holder {
        // SilentSiren Config
        public static string SILENTSIREN_PNAME = "_IS_SIREN_SILENT";
        public static Control SILENTSIREN_HOTKEY = Control.ThrowGrenade;
        public static int SILENTHOTKEY_MAXTIMEOUT = 15;

        public static bool IsSirenMuted(Vehicle car) {
            if (!EntityDecoration.ExistOn(car, SILENTSIREN_PNAME)) {
                return false;
            } else {
                return EntityDecoration.Get<bool>(car, SILENTSIREN_PNAME);
            }
        }

        // BlipSiren Config
        public static string BLIPSIREN_PNAME = "_IS_SIREN_BLIP";

        // SirenSound Config
        public static string SIRENSOUND_PNAME = "_IS_SIREN_ALT_SOUND";
        public static Control SIRENSOUND_HOTKEY = Control.Sprint;
    }
}
