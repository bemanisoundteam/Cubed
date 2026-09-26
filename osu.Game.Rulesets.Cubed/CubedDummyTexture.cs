using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Game.Rulesets.Cubed {
    public class CubedDummyTexture : Texture {
        private readonly Texture parent;

        public CubedDummyTexture(Texture parent)
            : base(parent, parent.WrapModeS, parent.WrapModeT) {
            this.parent = parent;

            Opacity = parent.Opacity;
        }

        public override int Width => parent.Width;

        public override int Height => parent.Height;

        public override RectangleF GetTextureRect(RectangleF? area = null) =>
            area ?? new RectangleF(Vector2.Zero, Vector2.One);
    }
}
