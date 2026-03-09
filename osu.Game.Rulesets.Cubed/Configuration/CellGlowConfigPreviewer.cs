using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Overlays;
using osu.Game.Rulesets.Cubed.Dependencies;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class CellGlowConfigPreviewer : CellGlowPreviewer, IHasPopover {
        private readonly Bindable<CellGlowSkin> currentSkin = new ();

        public CellGlowConfigPreviewer(CubedRulesetConfigManager config) {
            config.BindWith(CubedRulesetSetting.CurrentCellGlow, currentSkin);
            currentSkin.BindValueChanged(e => CellGlow = e.NewValue.CreateCellGlow(), true);
        }

        protected override bool OnClick(ClickEvent e) {
            this.ShowPopover();
            return true;
        }

        public Popover GetPopover() => new CellGlowSelectionPopover {
            CellGlowSkin = currentSkin.GetBoundCopy(),
            Shaders = Shaders
        };

        private partial class CellGlowSelectionPopover : CubedSkinSelectionPopover {
            public required Bindable<CellGlowSkin> CellGlowSkin { get; init; }
            public required CubedShaderManager Shaders { get; init; }

            [Cached]
            private readonly Bindable<Vector2> ptrPos = new();

            protected override void OnHoverLost(HoverLostEvent e) => ptrPos.SetDefault();

            protected override bool OnMouseMove(MouseMoveEvent e) {
                ptrPos.Value = e.ScreenSpaceMousePosition;
                return true;
            }

            [BackgroundDependencyLoader]
            private void load() =>
                Container.Children = CubedSkinRegistry.CellGlows.Select(CreateCard).ToList();

            [Resolved]
            private OverlayColourProvider Colors { get; set; }

            private Drawable CreateCard(CellGlowSkin skin) {
                var card = new SkinElementCard {
                    Size = new Vector2(100),
                    CornerRadius = 10,
                    BorderThickness = 3,
                    BackgroundColor = Colors.Background3,
                    Child = skin.CreateCellGlow(),
                    OnSelection = () => CellGlowSkin.Value = skin
                };
                CellGlowSkin.BindValueChanged(e => card.IsSelected = e.NewValue == skin, true);
                return card;
            }

            protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) {
                var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
                dependencies.CacheAs<ShaderManager>(Shaders);
                return dependencies;
            }
        }
    }
}
