using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Skinning.CellGlows {
    public partial class PointerPositionAwareCellGlow : Drawable, ITexturedShaderDrawable {
        public override bool HandlePositionalInput => true;

        [Resolved]
        private Bindable<Vector2> PtrPos { get; set; }

        private Vector2 _pointerPosition = new (float.NegativeInfinity);
        private Vector2 PointerPosition {
            get => _pointerPosition;
            set {
                if (_pointerPosition == value)
                    return;

                _pointerPosition = value;

                if (IsLoaded)
                    Invalidate(Invalidation.DrawNode);
            }
        }

        public IShader TextureShader { get; private set; }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders, IRenderer renderer) {
            RelativeSizeAxes = Axes.Both;

            Colour = ColourInfo.GradientHorizontal(Colour4.Cyan, Colour4.LimeGreen);

            TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, "PointerPositionAwareCellGlow");
            ConciergeIcon.EnsureWhitePixel(renderer);
        }

        protected override void LoadComplete() => PtrPos.BindValueChanged(
            e => PointerPosition = PtrPos.IsDefault
                ? Vector2.One / 2
                : Vector2.Divide(ToLocalSpace(e.NewValue) - DrawPosition, DrawSize)
            , true);

        protected override DrawNode CreateDrawNode() => new PointerPositionAwareCellGlowDrawNode(this);

        private class PointerPositionAwareCellGlowDrawNode(PointerPositionAwareCellGlow source) : TexturedShaderDrawNode(source) {
            private Quad screenSpaceDrawQuad;
            private Vector2 pointerPosition;

            public override void ApplyState() {
                base.ApplyState();

                screenSpaceDrawQuad = source.ScreenSpaceDrawQuad;
                pointerPosition = source.PointerPosition;
            }

            protected override void Draw(IRenderer renderer) {
                base.Draw(renderer);

                BindTextureShader(renderer);

                // Hack: Here blendRangeOverride only ends up changing the "Blend Range" value of the TexturedVertex2D,
                // which the Texture_2 vs just passthroughs to my custom fs
                renderer.DrawQuad(ConciergeIcon.WhitePixel, screenSpaceDrawQuad, DrawColourInfo.Colour, new RectangleF(Vector2.Zero, Vector2.One), blendRangeOverride: pointerPosition);

                UnbindTextureShader(renderer);
            }
        }
    }
}
