using osu.Game.Rulesets.Cubed.Skinning.CellGlows;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class CubedRulesetConfigManager {
        private void SetDefault(CubedRulesetSetting setting, CellGlowSkin value) {
            if (GetOriginalBindable<CellGlowSkin>(setting) is not BindableCellGlowSkin bindable) {
                bindable = new BindableCellGlowSkin(value);
                AddBindable(setting, bindable);
            }

            bindable.Value = value;
            bindable.Default = value;
        }
    }
}
