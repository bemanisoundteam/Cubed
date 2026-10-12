using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Skinning.Receptors {
    public partial class SpriteReceptor : Sprite {
        [BackgroundDependencyLoader]
        private void load(ICubedGameplaySkin skin) =>
            Texture = skin.GetTexture("Receptor");
    }
}
