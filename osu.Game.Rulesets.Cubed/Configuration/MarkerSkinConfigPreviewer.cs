using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osu.Game.Rulesets.Cubed.Skinning.Markers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace osu.Game.Rulesets.Cubed.Configuration {
    [Cached(typeof(ICubedGameplaySkin))]
    public partial class MarkerSkinConfigPreviewer : MarkerSkinPreviewer, ICubedGameplaySkin, IHasPopover {
        private readonly Bindable<CubedGameplaySkinInfo> currentSkinInfo = new();
        private readonly IBindable<CubedGameplaySkin> currentSkin;

        private CancellationTokenSource cts;

        public MarkerSkinConfigPreviewer(CubedRulesetConfigManager config) {
            config.BindWith(CubedRulesetSetting.CurrentGameplaySkin, currentSkinInfo);

            currentSkin = config.GameplaySkin;
            currentSkin.BindValueChanged(e => {
                cts?.Cancel();
                cts?.Dispose();
                cts = null;

                IMarker marker = (IMarker) e.NewValue.CreateComponent(CubedSkinComponents.Marker);

                if (marker is AnimatedMarker animatedMarker) {
                    cts = new CancellationTokenSource();

                    animatedMarker.IsForPreview = true;
                    LoadMarkerAsynchronously(animatedMarker, cts.Token);
                }
                else
                    Marker = marker;
            }, true);
        }

        protected override bool OnClick(ClickEvent e) {
            this.ShowPopover();
            return true;
        }

        public Popover GetPopover() => new MarkerSelectionPopover {
            MarkerSkin = currentSkinInfo.GetBoundCopy()
        };

        private void LoadMarkerAsynchronously(Drawable marker, CancellationToken cancellationToken) =>
            Scheduler.Add(p => {
                var spinner = new LoadingSpinner(true);
                spinner.Clock = p.Parent!.Clock;
                spinner.Show();
                InternalChild = spinner;

                LoadComponentAsync(marker, loaded => {
                    p.Marker = (IMarker) loaded;
                }, cancellationToken);
            }, this, false);

        public CubedGameplaySkinInfo SkinInfo => currentSkinInfo.Value;

        public Texture GetTexture(string name, WrapMode wrapModeS, WrapMode wrapModeT) => currentSkin.Value.GetTexture(name, wrapModeS, wrapModeT);

        private partial class MarkerSelectionPopover : CubedSkinSelectionPopover {
            public required Bindable<CubedGameplaySkinInfo> MarkerSkin { get; init; }

            protected override IReadOnlyList<SkinElementCard> CreateItemCards() =>
                CubedSkinRegistry.GameplaySkins.Select(s => CreateCard(s.CreateSkin())).ToList();

            private SkinElementCard CreateCard(CubedGameplaySkin skin) {
                var card = new SkinElementCard<ICubedGameplaySkin>(skin) {
                    Child = new MarkerSkinPreviewer {
                        RelativeSizeAxes = Axes.Both,
                        Marker = (IMarker) skin.CreateComponent(CubedSkinComponents.Marker)
                    },
                    OnSelection = () => MarkerSkin.Value = skin.SkinInfo
                };
                MarkerSkin.BindValueChanged(e => card.IsSelected = e.NewValue == skin.SkinInfo, true);
                return card;
            }
        }
    }
}
