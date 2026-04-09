using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class CubedRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null)
        : RulesetConfigManager<CubedRulesetSetting>(settings, ruleset, variant) {
        protected override void InitialiseDefaults() {
            base.InitialiseDefaults();

            SetDefault(CubedRulesetSetting.HighlightCells, false);
            SetDefault(CubedRulesetSetting.CellBorders, false);

            SetDefault(CubedRulesetSetting.CurrentCellGlow, new CellGlowSkin(null, "Cubed"));
            SetDefault(CubedRulesetSetting.CurrentGameplaySkin, new CubedGameplaySkin(null, "Cubed"));
        }
    }

    public enum CubedRulesetSetting {
        HighlightCells,
        CellBorders,

        // Skin items
        CurrentCellGlow,
        CurrentGameplaySkin,
    }
}
