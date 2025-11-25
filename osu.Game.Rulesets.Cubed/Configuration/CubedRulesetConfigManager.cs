using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public class CubedRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null)
        : RulesetConfigManager<CubedRulesetSetting>(settings, ruleset, variant) {
        protected override void InitialiseDefaults() {
            base.InitialiseDefaults();

            SetDefault(CubedRulesetSetting.HighlightCells, false);
            SetDefault(CubedRulesetSetting.CellBorders, false);
        }
    }

    public enum CubedRulesetSetting {
        HighlightCells,
        CellBorders,
    }
}
