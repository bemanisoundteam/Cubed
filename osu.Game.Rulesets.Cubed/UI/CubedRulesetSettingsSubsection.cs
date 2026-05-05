using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Cubed.Configuration;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osuTK;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedRulesetSettingsSubsection(Ruleset ruleset) : RulesetSettingsSubsection(ruleset) {
        // Matches ruleset.Description, but I didn't want to make a field, and the compiler complains if I use ruleset directly
        protected override LocalisableString Header => "Cubed";

        [BackgroundDependencyLoader]
        private void load() {
            var config = (CubedRulesetConfigManager) Config;

            Children = [
                new MarkerSkinConfigPreviewer(config) {
                    Size = new Vector2(SettingsPanel.PANEL_WIDTH, SettingsPanel.PANEL_WIDTH - SettingsPanel.CONTENT_MARGINS * 2),
                    Padding = new MarginPadding { Horizontal = SettingsPanel.CONTENT_MARGINS }
                },
                new SettingsDropdown<CubedGameplaySkin> {
                    LabelText = "Skin",
                    Current = config.GetBindable<CubedGameplaySkin>(CubedRulesetSetting.CurrentGameplaySkin),
                    ItemSource = CubedSkinRegistry.GameplaySkins
                },

                new CellGlowConfigPreviewer(config) {
                    Size = new Vector2(SettingsPanel.PANEL_WIDTH, SettingsPanel.PANEL_WIDTH - SettingsPanel.CONTENT_MARGINS * 2),
                    Padding = new MarginPadding { Horizontal = SettingsPanel.CONTENT_MARGINS }
                },
                new SettingsDropdown<CellGlowSkin> {
                    LabelText = "Cell glow",
                    Current = config.GetBindable<CellGlowSkin>(CubedRulesetSetting.CurrentCellGlow),
                    ItemSource = CubedSkinRegistry.CellGlows
                },

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
