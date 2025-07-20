using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace RA_Core
{
    [StaticConstructorOnStartup]
    public static class Feature_LongSightedFirstShot
    {
        static readonly TraitDef ShootingAccuracy = DefDatabase<TraitDef>.GetNamed("ShootingAccuracy");
        static readonly HediffDef HediffBloom1 = DefDatabase<HediffDef>.GetNamedSilentFail("Whisperofplanet");
        static readonly HediffDef HediffBloom2 = DefDatabase<HediffDef>.GetNamedSilentFail("WhisperofplanetTemp");
        static readonly GeneDef LongSightedGene = DefDatabase<GeneDef>.GetNamedSilentFail("longsighted");

        [ThreadStatic]
        static Verb currentVerb;

        static Feature_LongSightedFirstShot()
        {
            var harmony = new Harmony("RA_Core.Feature_LongSightedFirstShot");

            var tryCastShot = AccessTools.Method(typeof(Verb_LaunchProjectile), "TryCastShot");
            harmony.Patch(tryCastShot,
                prefix: new HarmonyMethod(typeof(Feature_LongSightedFirstShot), nameof(Prefix_TryCastShot)),
                transpiler: new HarmonyMethod(typeof(Feature_LongSightedFirstShot), nameof(Transpiler_TryCastShot)));
        }

        static void Prefix_TryCastShot(Verb __instance)
        {
            currentVerb = __instance;
        }

        static IEnumerable<CodeInstruction> Transpiler_TryCastShot(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var randChance = AccessTools.Method(typeof(Rand), nameof(Rand.Chance));
            var ourChance = AccessTools.Method(typeof(Feature_LongSightedFirstShot), nameof(OurRandChance));

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].Calls(randChance))
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, ourChance);
                }
            }

            return codes;
        }

        public static bool OurRandChance(float chance)
        {
            try
            {
                if (currentVerb == null || !RabbieCoreSettings.SettingsState.EnableLongSightedFirstShot)
                    return Rand.Chance(chance);

                Pawn casterPawn = currentVerb.CasterPawn;
                if (casterPawn == null)
                    return Rand.Chance(chance);

                // 1. 헤디프 검사
                bool hasHediff =
                    casterPawn.health?.hediffSet?.HasHediff(HediffBloom1) == true ||
                    casterPawn.health?.hediffSet?.HasHediff(HediffBloom2) == true;

                if (!hasHediff)
                    return Rand.Chance(chance);

                // 2. Biotech 유전자 검사
                bool geneOk = !ModsConfig.BiotechActive ||
                              (LongSightedGene != null && casterPawn.genes?.HasActiveGene(LongSightedGene) == true);

                if (!geneOk)
                    return Rand.Chance(chance);

                // 3. 사격 스킬 기반 보정
                int allowedShots = GetAllowedShots(casterPawn);
                int shotIndex = GetShotIndex(currentVerb);

                float skillLevel = casterPawn.skills?.GetSkill(SkillDefOf.Shooting)?.Level ?? 0;
                float baseChance = Lerp(
                    RabbieCoreSettings.SettingsState.FirstShotMinAccuracy,
                    RabbieCoreSettings.SettingsState.FirstShotMaxAccuracy,
                    skillLevel / 20f);

                Trait trait = casterPawn.story?.traits?.GetTrait(ShootingAccuracy);
                if (trait != null && trait.Degree == -1)
                    baseChance -= 0.05f;

                baseChance = Clamp01(baseChance);

                return (shotIndex <= allowedShots) ? Rand.Chance(baseChance) : Rand.Chance(chance);
            }
            catch
            {
                return Rand.Chance(chance);
            }
        }

        static int GetShotIndex(Verb verb)
        {
            try
            {
                FieldInfo fi = AccessTools.Field(typeof(Verb), "burstShotsLeft");
                int burstShotsLeft = (int)fi.GetValue(verb);
                return verb.verbProps.burstShotCount - burstShotsLeft + 1;
            }
            catch
            {
                return 1;
            }
        }

        static int GetAllowedShots(Pawn pawn)
        {
            Trait trait = pawn.story?.traits?.GetTrait(ShootingAccuracy);
            if (trait == null) return 1;
            return trait.Degree == 1 ? 3 : 1;
        }

        static float Lerp(float a, float b, float t)
        {
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;
            return a + (b - a) * t;
        }

        static float Clamp01(float v)
        {
            if (v < 0f) return 0f;
            if (v > 1f) return 1f;
            return v;
        }
    }
}
