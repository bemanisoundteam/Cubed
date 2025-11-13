using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Cubed.Configuration;

namespace osu.Game.Rulesets.Cubed.UI {
#pragma warning disable CS9107 // Parameter is captured into the state of the enclosing type and its value is also passed to the base constructor. The value might be captured by the base class as well.
    public partial class CubedRulesetSettingsSubsection(Ruleset ruleset) : RulesetSettingsSubsection(ruleset) {
        protected override LocalisableString Header => ruleset.Description;

        [BackgroundDependencyLoader]
        private void load() {
            var config = (CubedRulesetConfigManager) Config;

            Children = [
                new SettingsCheckbox {
                    LabelText = "Highlight cells",
                    Current = config.GetBindable<bool>(CubedRulesetSetting.HighlightCells)
                }
            ];
        }
    }
}
