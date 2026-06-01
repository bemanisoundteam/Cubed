using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Indicators;
using osu.Game.Rulesets.Cubed.Skinning.Markers;
using osu.Game.Rulesets.Judgements;
using osu.Game.Skinning;
using System;

using Triangle = osu.Framework.Graphics.Primitives.Triangle;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public partial class CubedSkinnableDrawable(CubedSkinComponents lookup)
        : SkinnableDrawable(new CubedSkinComponentLookup(lookup), DefaultFunction(lookup), ConfineMode.ScaleToFit) {
        protected override bool ApplySizeRestrictionsToDefault => true;

        public void FlushPendingSkinChange() => FlushPendingSkinChanges();

        [BackgroundDependencyLoader]
        private void load() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        private static Func<ISkinComponentLookup, Drawable> DefaultFunction(CubedSkinComponents l) => l switch {
            CubedSkinComponents.Marker => _ => new DefaultMarker(),
            CubedSkinComponents.Receptor => _ => new DefaultReceptor(),
            CubedSkinComponents.Indicator => _ => new DefaultIndicator(),
            CubedSkinComponents.CellGlow => _ => new DefaultCellGlow(),
            _ => null
        };
    }

    public partial class DefaultMarker : CompositeDrawable, IMarker {
        public osuTK.Vector2 PreviewScale => new (1f / ApproachScale.X, 1f / ApproachScale.Y);

        private static readonly osuTK.Vector2 ApproachScale = new (2.5f);

        private readonly Drawable approach = new Approach();
        private readonly Drawable marker = new Box {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        };

        [Resolved]
        private OsuColour colour { get; set; }

        [BackgroundDependencyLoader]
        private void load() =>
            InternalChildren = [marker, approach];

        public void AnimateApproach(double time) => approach.ScaleTo(1, time).Then().FadeOut();

        public void AnimateHit(double duration, JudgementResult judgement) {
            approach.FadeOut();
            marker.FadeColour(colour.ForHitResult(judgement.Type)).ScaleTo(1.5f, duration, Easing.OutQuint);
        }

        private partial class Approach : CompositeDrawable {
            [BackgroundDependencyLoader]
            private void iWishThereWasABetterWayToDoThis() {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.Both;
                Scale = ApproachScale;
                Masking = true;
                BorderColour = Colour4.Red;
                BorderThickness = 3;

                InternalChild = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Transparent
                };
            }
        }
    }

    public partial class DefaultReceptor : Drawable {
        private IShader shader;

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders) {
            shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);

            // Not the best color I agree, I don't really know what else to put there,
            // Green was too distracting for gameplay, and the others don't seem good to use in gameplay for me
            // But that's all subjective and up for discussion
            Colour = Colour4.Orange;
        }

        protected override DrawNode CreateDrawNode() => new ReceptorDrawNode(this);

        private class ReceptorDrawNode(DefaultReceptor source) : DrawNode(source) {
            private IShader shader;
            private readonly Triangle[] triangles = new Triangle[2];

            public override void ApplyState() {
                base.ApplyState();

                shader = source.shader;
                triangles[0] = new Triangle(new(0, 0), new(0, source.DrawHeight), new(source.DrawWidth  / 2f, source.DrawHeight)) * DrawInfo.Matrix;
                triangles[1] = new Triangle(new(source.DrawWidth / 2f, source.DrawHeight), new(source.DrawWidth, source.DrawHeight), new(source.DrawWidth, 0)) * DrawInfo.Matrix;
            }

            protected override bool CanDrawOpaqueInterior => DrawColourInfo.Colour.MinAlpha >= 1;

            protected override void Draw(IRenderer renderer) {
                base.Draw(renderer);

                DrawTriangles(renderer);
            }

            protected override void DrawOpaqueInterior(IRenderer renderer) {
                base.DrawOpaqueInterior(renderer);

                DrawTriangles(renderer);
            }

            private void DrawTriangles(IRenderer renderer) {
                shader.Bind();

                foreach (Triangle triangle in triangles)
                    renderer.DrawTriangle(
                        renderer.WhitePixel,
                        triangle,
                        DrawColourInfo.Colour
                    );

                shader.Unbind();
            }
        }
    }
}

