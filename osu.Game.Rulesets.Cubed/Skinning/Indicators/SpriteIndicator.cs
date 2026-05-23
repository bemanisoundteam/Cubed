using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Cubed.Skinning.Indicators {
    public partial class SpriteIndicator(TextureStore textures) : ConnectedPointerIndicator {
        private readonly Sprite connection = new() {
            RelativeSizeAxes = Axes.Both
        };

        private readonly Sprite pointer = new() {
            RelativeSizeAxes = Axes.Both
        };

        protected override Drawable Connection => connection;

        protected override Drawable Pointer => pointer;

        [BackgroundDependencyLoader]
        private void load() {
            connection.Texture = textures.Get("Connection");
            pointer.Texture = textures.Get("Pointer");
        }
    }
}
