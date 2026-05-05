using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Overlays;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class MarkerSkinConfigPreviewer : MarkerSkinPreviewer, IHasPopover {
        private readonly Bindable<CubedGameplaySkin> currentSkin = new();

        public MarkerSkinConfigPreviewer(CubedRulesetConfigManager config) {
            config.BindWith(CubedRulesetSetting.CurrentGameplaySkin, currentSkin);
            currentSkin.BindValueChanged(e => Marker = (IMarker) e.NewValue.CreateComponent(CubedSkinComponents.Marker), true);
        }

        protected override bool OnClick(ClickEvent e) {
            this.ShowPopover();
            return true;
        }

        public Popover GetPopover() => new MarkerSelectionPopover {
            MarkerSkin = currentSkin.GetBoundCopy()
        };

        private partial class MarkerSelectionPopover : CubedSkinSelectionPopover {
            public required Bindable<CubedGameplaySkin> MarkerSkin { get; init; }

            [BackgroundDependencyLoader]
            private void load() =>
                Container.Children = CubedSkinRegistry.GameplaySkins.Select(CreateCard).ToList();

            [Resolved]
            private OverlayColourProvider Colors { get; set; }

            private Drawable CreateCard(CubedGameplaySkin skin) {
                var card = new SkinElementCard {
                    Size = new Vector2(100),
                    CornerRadius = 10,
                    BorderThickness = 3,
                    BackgroundColor = Colors.Background3,
                    Child = new MarkerSkinPreviewer {
                        RelativeSizeAxes = Axes.Both,
                        Marker = (IMarker) skin.CreateComponent(CubedSkinComponents.Marker)
                    },
                    OnSelection = () => MarkerSkin.Value = skin
                };
                MarkerSkin.BindValueChanged(e => card.IsSelected = e.NewValue == skin, true);
                return card;
            }
        }
    }
}
