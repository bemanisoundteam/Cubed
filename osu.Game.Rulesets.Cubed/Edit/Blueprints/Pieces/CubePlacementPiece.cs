using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces {
    public partial class CubePlacementPiece : Sprite {
        [BackgroundDependencyLoader]
        private void load(TextureStore textures) =>
            Texture = textures.Get("Cubed-logo");
    }
}
