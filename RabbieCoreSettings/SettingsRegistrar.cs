using ModSettingRegistry;
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
                Label = "휴대중량 계산 패치 활성화",
                Description = "기본 중량 계산을 덮어써서 크기 기반 휴대량 공식을 적용합니다.",
                Type = SettingType.Toggle,
                Getter = () => SettingsState.EnableCarryWeight,
                Setter = v => SettingsState.EnableCarryWeight = v
            };

            var excludeWeapon = new SettingDefinition
            {
                Key = "ExcludeMechWeaponWeight",
                Label = "메카노이드 무기 무게 제외",
                Description = "메카노이드 장착 무기의 무게를 휴대중량 계산에서 제외합니다.",
                Type = SettingType.Toggle,
                Getter = () => SettingsState.ExcludeMechWeaponWeight,
                Setter = v => SettingsState.ExcludeMechWeaponWeight = v
            };

            Registry.RegisterGroup(
                new SettingGroup
                {
                    GroupKey = "CarryWeight",
                    GroupLabel = "휴대중량 계산",
                    GroupDescription = "몸 크기와 운반용량으로 Carrying Capacity를 다시 계산합니다.",
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
        }
    }
}
