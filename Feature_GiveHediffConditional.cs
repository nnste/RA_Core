using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RA_Core
{
    internal class Feature_GiveHediffConditional
    {
        public class IngestionOutcomeDoer_GiveHediffConditional : IngestionOutcomeDoer
        {
            public HediffDef hediffDef;
            public float severity = -1f;

            public List<ThingDef> allowedRaces;
            public List<GeneDef> requiredGenes;
            public float applyChance = 1f;

            protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
            {
                // 1. 모드 설정에서 종족 제한 무시 여부 확인
                bool ignoreRaceRestriction = RabbieCoreSettings.SettingsState.IgnoreRaceRestrictionForDrugEffects;

                // 2. 종족 제한 검사 (단, 무시 설정시 스킵)
                if (!ignoreRaceRestriction && allowedRaces != null && allowedRaces.Count > 0)
                {
                    if (!allowedRaces.Contains(pawn.def))
                        return;
                }

                // 3. BioTech가 있고 유전자 조건이 있을 경우 검사
                if (ModsConfig.BiotechActive && requiredGenes != null && requiredGenes.Count > 0)
                {
                    if (pawn.genes == null || !requiredGenes.All(g => pawn.genes.HasActiveGene(g)))
                        return;
                }

                // 4. 확률 검사
                if (!Rand.Chance(applyChance))
                    return;

                // 5. Hediff 적용
                Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
                float effect = (severity > 0f) ? severity : hediffDef.initialSeverity;
                hediff.Severity = effect;
                pawn.health.AddHediff(hediff);
            }
        }

    }
}
