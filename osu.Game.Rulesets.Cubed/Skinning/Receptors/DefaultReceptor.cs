using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;

namespace osu.Game.Rulesets.Cubed.Skinning.Receptors {
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
                triangles[0] = new Triangle(
                    new(0, 0),
                    new(0, source.DrawHeight),
                    new(source.DrawWidth / 2f, source.DrawHeight)
                ) * DrawInfo.Matrix;
                triangles[1] = new Triangle(
                    new(source.DrawWidth / 2f, source.DrawHeight),
                    new(source.DrawWidth, source.DrawHeight),
                    new(source.DrawWidth, 0)
                ) * DrawInfo.Matrix;
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
