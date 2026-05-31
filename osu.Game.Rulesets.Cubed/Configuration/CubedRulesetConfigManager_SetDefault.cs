using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class CubedRulesetConfigManager {
        private void SetDefault(CubedRulesetSetting setting, CellGlowSkinInfo value) {
            if (GetOriginalBindable<CellGlowSkinInfo>(setting) is not BindableCellGlowSkinInfo bindable) {
                bindable = new BindableCellGlowSkinInfo(value);
                AddBindable(setting, bindable);
            }

            bindable.Default = value;
        }

        private void SetDefault(CubedRulesetSetting setting, CubedGameplaySkinInfo value) {
            if (GetOriginalBindable<CubedGameplaySkinInfo>(setting) is not BindableCubedGameplaySkinInfo bindable) {
                bindable = new BindableCubedGameplaySkinInfo(value);
                AddBindable(setting, bindable);
            }

            bindable.Default = value;
        }
    }
}
