using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Skinning.Indicators {
    public partial class SpriteIndicator : ConnectedPointerIndicator {
        private readonly Sprite connection = new() {
            RelativeSizeAxes = Axes.Both
        };

        private readonly Sprite pointer = new() {
            RelativeSizeAxes = Axes.Both
        };

        protected override Drawable Connection => connection;

        protected override Drawable Pointer => pointer;

        [BackgroundDependencyLoader]
        private void load(ICubedGameplaySkin skin) {
            connection.Texture = skin.GetTexture("Connection");
            pointer.Texture = skin.GetTexture("Pointer");
        }
    }
}
