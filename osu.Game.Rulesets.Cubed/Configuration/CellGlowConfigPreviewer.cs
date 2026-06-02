using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Dependencies;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Configuration {
    [Cached(typeof(ICellGlowSkin))]
    public partial class CellGlowConfigPreviewer : CellGlowPreviewer, ICellGlowSkin, IHasPopover {
        private readonly Bindable<CellGlowSkinInfo> currentSkinInfo = new ();
        private readonly IBindable<CellGlowSkin> currentSkin;

        public CellGlowConfigPreviewer(CubedRulesetConfigManager config) {
            config.BindWith(CubedRulesetSetting.CurrentCellGlow, currentSkinInfo);

            currentSkin = config.CellGlowSkin;
            currentSkin.BindValueChanged(e => CellGlow = e.NewValue.CreateCellGlow(), true);
        }

        protected override bool OnClick(ClickEvent e) {
            this.ShowPopover();
            return true;
        }

        public Popover GetPopover() => new CellGlowSelectionPopover {
            CellGlowSkin = currentSkinInfo.GetBoundCopy(),
            Shaders = Shaders
        };

       public CellGlowSkinInfo SkinInfo => currentSkinInfo.Value;

        public Texture GetTexture(string name, WrapMode wrapModeS, WrapMode wrapModeT) => currentSkin.Value.GetTexture(name, wrapModeS, wrapModeT);

        private partial class CellGlowSelectionPopover : CubedSkinSelectionPopover {
            public required Bindable<CellGlowSkinInfo> CellGlowSkin { get; init; }
            public required CubedShaderManager Shaders { get; init; }

            [Cached]
            private readonly Bindable<Vector2> ptrPos = new();

            protected override void OnHoverLost(HoverLostEvent e) => ptrPos.SetDefault();

            protected override bool OnMouseMove(MouseMoveEvent e) {
                ptrPos.Value = e.ScreenSpaceMousePosition;
                return true;
            }

            protected override IReadOnlyList<SkinElementCard> CreateItemCards() =>
                CubedSkinRegistry.CellGlows.Select(s => CreateCard(s.CreateSkin())).ToList();

            private SkinElementCard CreateCard(CellGlowSkin skin) {
                var card = new SkinElementCard<ICellGlowSkin>(skin) {
                    Child = new CellGlowPreviewer {
                        RelativeSizeAxes = Axes.Both,
                        CellGlow = skin.CreateCellGlow()
                    },
                    OnSelection = () => CellGlowSkin.Value = skin.SkinInfo
                };
                CellGlowSkin.BindValueChanged(e => card.IsSelected = e.NewValue == skin.SkinInfo, true);
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
