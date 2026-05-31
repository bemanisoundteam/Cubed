using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class MarkerSkinConfigPreviewer : MarkerSkinPreviewer, IHasPopover {
        private readonly Bindable<CubedGameplaySkinInfo> currentSkin = new();

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
            public required Bindable<CubedGameplaySkinInfo> MarkerSkin { get; init; }

            protected override IReadOnlyList<SkinElementCard> CreateItemCards() =>
                CubedSkinRegistry.GameplaySkins.Select(CreateCard).ToList();

            private SkinElementCard CreateCard(CubedGameplaySkinInfo skin) {
                var card = new SkinElementCard {
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
