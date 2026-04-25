using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace CombatAI
{
    public class BannerlordCombatAISettings : AttributeGlobalSettings<BannerlordCombatAISettings>
    {
        public override string Id => "BannerlordCombatAI";
        public override string DisplayName => "Bannerlord Combat AI";
        public override string FolderName => "BannerlordCombatAI";
        public override string FormatType => "json";

        [SettingPropertyFloatingInteger("Difficulty", 0f, 1f, "#0.00", Order = 0, RequireRestart = false, HintText = "Adjust AI difficulty from 0 to 1")]
        [SettingPropertyGroup("General")]
        public float Difficulty { get; set; } = 0.75f;

        [SettingPropertyBool("Enable Mod", Order = 1, RequireRestart = false, HintText = "Enable or disable the mod's AI improvements")]
        [SettingPropertyGroup("General")]
        public bool EnableMod { get; set; } = true;
    }
}
