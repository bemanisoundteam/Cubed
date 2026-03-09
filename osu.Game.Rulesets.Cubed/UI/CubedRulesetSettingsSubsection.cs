using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Cubed.Configuration;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedRulesetSettingsSubsection(Ruleset ruleset) : RulesetSettingsSubsection(ruleset) {
        // Matches ruleset.Description, but I didn't want to make a field, and the compiler complains if I use ruleset directly
        protected override LocalisableString Header => "Cubed";

        [BackgroundDependencyLoader]
        private void load() {
            var config = (CubedRulesetConfigManager) Config;

            Children = [
                new SettingsCheckbox {
                    LabelText = "Cell borders",
                    Current = config.GetBindable<bool>(CubedRulesetSetting.CellBorders)
                },
                new SettingsCheckbox {
                    LabelText = "Highlight cells",
                    Current = config.GetBindable<bool>(CubedRulesetSetting.HighlightCells)
                },
            ];
        }
    }
}
