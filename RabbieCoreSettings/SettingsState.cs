using Verse;

namespace RabbieCoreSettings
{
    public static class SettingsState
    {
        public static bool EnableCarryWeight = true;
        public static bool ExcludeMechWeaponWeight = true;
        public static bool EnbleDrugEffectControl = true;
        public static bool IgnoreRaceRestrictionForDrugEffects = false;
        public static bool EnableLongSightedFirstShot = true;
        public static float FirstShotMinAccuracy = 0.5f;
        public static float FirstShotMaxAccuracy = 1.0f;

        public static void Expose()
        {
            Scribe_Values.Look(ref EnableCarryWeight, "EnableCarryWeight", true);
            Scribe_Values.Look(ref ExcludeMechWeaponWeight, "ExcludeMechWeaponWeight", true);
            Scribe_Values.Look(ref EnableCarryWeight, "EnbleDrugEffectControl", true);
            Scribe_Values.Look(ref IgnoreRaceRestrictionForDrugEffects, "IgnoreRaceRestrictionForDrugEffects", false);
            Scribe_Values.Look(ref EnableLongSightedFirstShot, "EnableLongSightedFirstShot", true);
            Scribe_Values.Look(ref FirstShotMinAccuracy, "FirstShotMinAccuracy", 0.5f);
            Scribe_Values.Look(ref FirstShotMaxAccuracy, "FirstShotMaxAccuracy", 1.0f);
        }
    }
}
