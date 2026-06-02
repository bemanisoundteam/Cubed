using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Skinning.Receptors {
    public partial class SpriteReceptor(int frameCount) : Sprite {
        private readonly Texture[] textures = new Texture[frameCount];

        [BackgroundDependencyLoader]
        private void load(ICubedGameplaySkin skin) {
            switch (frameCount) {
                case 0:
                    Logger.Log($"CellGlow \"{skin.SkinInfo}\" has 0 frames !");
                    return;

                case 1:
                    textures[0] = skin.GetTexture("Receptor") ?? skin.GetTexture("Receptor0");
                    Texture = textures[0];
                    return;

                default:
                    // What is cracking my brochacho, if you're watching this diff right now
                    // That means you've been selected for https://www.youtube.com/watch?v=dpXaBVKO8r8
                    throw new System.NotImplementedException("Animations coming soon on Video and DVD");
            }
        }
    }
}
