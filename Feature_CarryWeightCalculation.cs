using HarmonyLib;
using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RabbieCoreSettings
{
    [StaticConstructorOnStartup]
    public static class Feature_CarryWeightCalculation
    {
        static readonly bool HasVehicleFramework =
            ModLister.GetActiveModWithIdentifier("SmashPhil.VehicleFramework", false) != null;
        static readonly StatDef MassOffset =
            DefDatabase<StatDef>.GetNamedSilentFail("MassOffset");

        static Feature_CarryWeightCalculation()
        {
            var harmony = new Harmony("RabbieCoreSettings.CarryWeightPatch");
            harmony.Patch(
                AccessTools.Method(typeof(MassUtility), nameof(MassUtility.Capacity)),
                prefix: new HarmonyMethod(typeof(Feature_CarryWeightCalculation), nameof(Prefix)));
        }

        public static bool Prefix(Pawn p, ref float __result, ref StringBuilder explanation)
        {
            if (!SettingsState.EnableCarryWeight) return true;
            if (HasVehicleFramework &&
                p.def?.thingClass?.FullName?.Contains("Vehicles") == true) return true;

            if (!MassUtility.CanEverCarryAnything(p))
            {
                __result = 0f;
                return false;
            }

            float cap = p.GetStatValue(StatDefOf.CarryingCapacity);
            float size = p.BodySize;

            double baseSize =
                size < .5 ? 5 * size + 12 :
                size < 1.0 ? size + 14 :
                size < 2.0 ? 5 * (size - 1) + 15 :
                size < 3.0 ? 20 * (size - 2) + 20 :
                size < 4.0 ? 40 * (size - 3) + 40 : 80;

            double baseCap =
                cap < 0 ? 5 :
                cap < 75 ? cap / 5 + 5 :
                cap < 150 ? cap / 3 - 5 :
                cap < 225 ? cap / 5 + 15 : 60;

            float weaponMass = 0f;
            if (!SettingsState.ExcludeMechWeaponWeight &&
                p.RaceProps.IsMechanoid && p.equipment?.Primary != null)
                weaponMass = p.equipment.Primary.GetStatValue(StatDefOf.Mass);

            float extra = MassOffset != null ? p.GetStatValue(MassOffset) : 0f;
            __result = Mathf.Max(0f, (float)Math.Round(baseSize + baseCap, 2) + weaponMass + extra);

            if (explanation != null)
            {
                if (explanation.Length > 0) explanation.AppendLine();
                explanation.Append("  - ").Append(p.LabelShortCap).Append(": ")
                           .Append(GenText.ToStringMassOffset(__result));
            }
            return false;
        }
    }
}
 