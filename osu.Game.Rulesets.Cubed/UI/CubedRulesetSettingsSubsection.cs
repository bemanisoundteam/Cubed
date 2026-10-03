using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Cubed.Configuration;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osuTK;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedRulesetSettingsSubsection(Ruleset ruleset) : RulesetSettingsSubsection(ruleset) {
        [BackgroundDependencyLoader]
        private void load() {
            var config = (CubedRulesetConfigManager) Config;

            Children = [
                new CubedConfigPaddedContainer(new MarkerSkinConfigPreviewer(config)),
                new SettingsDropdown<CubedGameplaySkinInfo> {
                    LabelText = "Skin",
                    Current = config.GetBindable<CubedGameplaySkinInfo>(CubedRulesetSetting.CurrentGameplaySkin),
                    ItemSource = CubedSkinRegistry.GameplaySkins
                },

                new CubedConfigPaddedContainer(new CellGlowConfigPreviewer(config)),
                new SettingsDropdown<CellGlowSkinInfo> {
                    LabelText = "Cell glow",
                    Current = config.GetBindable<CellGlowSkinInfo>(CubedRulesetSetting.CurrentCellGlow),
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

        private sealed partial class CubedConfigPaddedContainer : Container {
            public CubedConfigPaddedContainer(Drawable child) {
                Size = new Vector2(SettingsPanel.PANEL_WIDTH, SettingsPanel.PANEL_WIDTH - SettingsPanel.CONTENT_MARGINS * 2);
                Padding = new MarginPadding { Horizontal = SettingsPanel.CONTENT_MARGINS };
                Masking = true;
                Child = child.With(c => c.RelativeSizeAxes = Axes.Both);
            }
        }
    }
}
