﻿using ModSettingRegistry;
using SharedDefs;
using System.Collections.Generic;
using Verse;

namespace RabbieCoreSettings
{
    [StaticConstructorOnStartup]
    public static class SettingsRegistrar
    {
        static SettingsRegistrar() => RegisterSettings();

        private static void RegisterSettings()
        {
            var enableCarry = new SettingDefinition
            {
                Key = "EnableCarryWeight",
                Label = "<i>휴대중량 계산 변화 활성화</i>",
                Description = "기존의 몸크기만을 기반으로 한 휴대중량 방식에서 크기 및 운반수량 기반 휴대중량 공식으로 적용합니다.",
                Type = SettingType.Toggle,
                Getter = () => SettingsState.EnableCarryWeight,
                Setter = v => SettingsState.EnableCarryWeight = v
            };

            var excludeWeapon = new SettingDefinition
            {
                Key = "ExcludeMechWeaponWeight",
                Label = "메카노이드 무기 무게 제외",
                Description = "메카노이드 장착 무기의 무게를 휴대중량 계산에서 제외합니다.(ex.미호메카)",
                Type = SettingType.Toggle,
                Getter = () => SettingsState.ExcludeMechWeaponWeight,
                Setter = v => SettingsState.ExcludeMechWeaponWeight = v
            };

            Registry.RegisterGroup(
                new SettingGroup
                {
                    GroupKey = "CarryWeight",
                    GroupLabel = "휴대중량 계산",
                    GroupDescription = "몸크기와 운반수량으로 휴대중량을 다시 계산합니다.",
                    Section = ParentSection.Core,
                    Category = SettingCategory.Always,
                    Items = new List<SettingDefinition> { enableCarry, excludeWeapon }
                },
                SettingsState.Expose   // 저장/로드는 한 번만 등록
            );


            Registry.RegisterItem(new SettingDefinition
            {
                Key = "SleepfaceInfo",                
                Label = "<b>슬립 페이스 적용</b>",       
                Description = "이 패치는 AhnDemi님의 CloseYourEyesWhenSleeping 방식을 기반으로 표정을 추가하며,\n" +
                              "dll로 HAR의 바디애드온 conditions에 Sleeping을 추가하여 자는 상태를 구분합니다.\n" +
                              "타종족도 Sleeping을 사용할 수 있으며 xml편집으로 더 많은 얼굴을 추가할 수 있습니다.",
                Type = SettingType.Description,          
                Section = ParentSection.Core,              
                Category = SettingCategory.Always            
            }, null);

            Registry.RegisterItem(new SettingDefinition
            {
                Key = "RabbieMaskInfo",
                Label = "<b>래비 마스크 적용</b>",
                Description = "이 패치는 HAR 기능을 사용해 래비가 마스크를 사용시 전용 텍스처를 적용합니다.\n" +
                              "HAR 기능의 전용텍스쳐 기능을 활용하는 패치로 쉽게 텍스처 수정이 가능하며,\n" +
                              "xml을 참고하여 다른 의복도 추가할 수 있습니다.",
                Type = SettingType.Description,
                Section = ParentSection.Core,
                Category = SettingCategory.Always
            }, null);

            var EnbleDrugEffectControl = new SettingDefinition
            {
                Key = "EnbleDrugEffectControl",
                Label = "<i>플라네타륨 개화 조정</i>",
                Description = "활성화 시 보호된 플라네타륨 개화 관련 설정을 조정할 수 있습니다. \n\n※ Biotech DLC가 있을 경우 플라네타륨 피로 유전자도 보유해야 개화됩니다.",
                Type = SettingType.Toggle,
                Getter = () => SettingsState.EnbleDrugEffectControl,
                Setter = v => SettingsState.EnbleDrugEffectControl = v
            };

            var ignoreRaceRestriction = new SettingDefinition
            {
                Key = "IgnoreRaceRestrictionForDrugEffects",
                Label = "플라네타륨 개화 종족제한",
                Description = "활성화 시 종족 제한 조건을 무시하고 모든 종족에 개화 가능성을 부여합니다.\n\n※ Biotech DLC가 있을 경우 플라네타륨 피로 유전자도 보유해야 개화됩니다.",
                Type = SettingType.Toggle,
                Getter = () => SettingsState.IgnoreRaceRestrictionForDrugEffects,
                Setter = v => SettingsState.IgnoreRaceRestrictionForDrugEffects = v
            };

            var enableLongSightedFirstShot = new SettingDefinition
            {
                Key = "EnableLongSightedFirstShot",
                Label = "초탄 명중 보정 활성화",
                Description = "개화 상태(헤디프)가 있을 때, 첫 발의 명중률을 사격 기술 기반으로 보정합니다.\n" 
                              + "고정된 명중률 값에 따라 명중 성공시 필중하며,\n"
                              + " 고정명중률 값에서 명중 실패시 바닐라 명중식으로 새로 계산됩니다.\n"
                              +"(즉 최저값이여도 최소한의 명중률을 보장함)\n"
                              + "신중한 사수는 버스트의 첫 3발을 보정받습니다.\n난사광은 -5%p가 보정됩니다. \n\n※ Biotech DLC가 있을 경우 선천적인 저격수 유전자도 보유해야 적용됩니다.",
                Type = SettingType.Toggle,
                Getter = () => SettingsState.EnableLongSightedFirstShot,
                Setter = v => SettingsState.EnableLongSightedFirstShot = v
            };

            var firstShotMinAccuracy = new SettingDefinition
            {
                Key = "FirstShotMinAccuracy",
                Label = "최소 명중률 (%)",
                Description = "사격 Lv0 기준 첫 발 명중 확률입니다. 기본값 50%",
                Type = SettingType.Slider,
                SliderMin = 0f,
                SliderMax = 100f,
                SliderGetter = () => SettingsState.FirstShotMinAccuracy * 100f,
                SliderSetter = v => SettingsState.FirstShotMinAccuracy = v / 100f
            };

            var firstShotMaxAccuracy = new SettingDefinition
            {
                Key = "FirstShotMaxAccuracy",
                Label = "최대 명중률 (%)",
                Description = "사격 Lv20 기준 첫 발 명중 확률입니다. 기본값 100%",
                Type = SettingType.Slider,
                SliderMin = 0f,
                SliderMax = 100f,
                SliderGetter = () => SettingsState.FirstShotMaxAccuracy * 100f,
                SliderSetter = v => SettingsState.FirstShotMaxAccuracy = v / 100f
            };


            Registry.RegisterGroup(
                new SettingGroup
                {
                    GroupKey = "DrugEffectControl",
                    GroupLabel = "플라네타륨 개화",
                    GroupDescription = "플라네타륨 개화 관련 설정들을 조정합니다.",
                    Section = ParentSection.Core,
                    Category = SettingCategory.Always,
                    Items = new List<SettingDefinition> {EnbleDrugEffectControl,ignoreRaceRestriction,enableLongSightedFirstShot,firstShotMinAccuracy,firstShotMaxAccuracy }
                },
                SettingsState.Expose
            );

        }
    }
}
