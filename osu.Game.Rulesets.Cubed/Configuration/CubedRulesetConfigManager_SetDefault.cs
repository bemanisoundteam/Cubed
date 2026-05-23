using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class CubedRulesetConfigManager {
        private void SetDefault(CubedRulesetSetting setting, CellGlowSkin value) {
            if (GetOriginalBindable<CellGlowSkin>(setting) is not BindableCellGlowSkin bindable) {
                bindable = new BindableCellGlowSkin(value);
                AddBindable(setting, bindable);
            }

            bindable.Default = value;
        }

        private void SetDefault(CubedRulesetSetting setting, CubedGameplaySkin value) {
            if (GetOriginalBindable<CubedGameplaySkin>(setting) is not BindableCubedGameplaySkin bindable) {
                bindable = new BindableCubedGameplaySkin(value);
                AddBindable(setting, bindable);
            }

            bindable.Default = value;
        }
    }
}
