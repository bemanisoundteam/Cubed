using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterfaceV2;
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
                new SettingsItemV2(new FormDropdown<CubedGameplaySkinInfo> {
                    Caption = "Skin",
                    Current = config.GetBindable<CubedGameplaySkinInfo>(CubedRulesetSetting.CurrentGameplaySkin),
                    ItemSource = CubedSkinRegistry.GameplaySkins
                }),

                new CubedConfigPaddedContainer(new CellGlowConfigPreviewer(config)),
                new SettingsItemV2(new FormDropdown<CellGlowSkinInfo> {
                    Caption = "Cell glow",
                    Current = config.GetBindable<CellGlowSkinInfo>(CubedRulesetSetting.CurrentCellGlow),
                    ItemSource = CubedSkinRegistry.CellGlows
                }),

                new SettingsItemV2(new FormCheckBox {
                    Caption = "Cell borders",
                    Current = config.GetBindable<bool>(CubedRulesetSetting.CellBorders)
                }),
                new SettingsItemV2(new FormCheckBox {
                    Caption = "Highlight cells",
                    Current = config.GetBindable<bool>(CubedRulesetSetting.HighlightCells)
                }),
            ];
        }

        private sealed partial class CubedConfigPaddedContainer : Container {
            private readonly FormControlBackground bg;

            public CubedConfigPaddedContainer(Drawable child) {
                Size = new Vector2(SettingsPanel.PANEL_WIDTH, SettingsPanel.PANEL_WIDTH - SettingsPanel.CONTENT_MARGINS);
                Padding = new MarginPadding { Horizontal = SettingsPanel.CONTENT_MARGINS / 2 };
                Masking = true;
                Children = [
                    bg = new FormControlBackground(),
                    new Container {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding(SettingsPanel.CONTENT_MARGINS / 2),
                        Child = child.With(c => c.RelativeSizeAxes = Axes.Both)
                    }
                ];
            }

            protected override bool OnHover(HoverEvent e) {
                bg.VisualStyle = VisualStyle.Hovered;
                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e) =>
                bg.VisualStyle = VisualStyle.Normal;
        }
    }
}
