using Verse;

namespace RabbieCoreSettings
{
    public static class SettingsState
    {
        public static bool EnableCarryWeight = true;
        public static bool ExcludeMechWeaponWeight = true;

        public static void Expose()
        {
            Scribe_Values.Look(ref EnableCarryWeight, "EnableCarryWeight", true);
            Scribe_Values.Look(ref ExcludeMechWeaponWeight, "ExcludeMechWeaponWeight", true);
        }
    }
}
