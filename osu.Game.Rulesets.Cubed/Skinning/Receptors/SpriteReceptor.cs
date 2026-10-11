using osu.Framework.Allocation;
using osu.Framework.Graphics.Animations;
using osu.Framework.Logging;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Skinning.Receptors {
    public partial class SpriteReceptor : TextureAnimation {
        private readonly int frameCount;

        public SpriteReceptor(int frameCount, double framerate = 0) {
            this.frameCount = frameCount;

            // FP Division gives Infinity in case framerate is 0
            DefaultFrameLength = 1000 / framerate;
        }

        [BackgroundDependencyLoader]
        private void load(ICubedGameplaySkin skin) {
            switch (frameCount) {
                case 0:
                    Logger.Log($"Receptor \"{skin.SkinInfo}\" has 0 frames !");
                    return;

                case 1:
                    AddFrame(skin.GetTexture("Receptor") ?? skin.GetTexture("Receptor0"));
                    return;

                default:
                    for (int x = 0; x < FrameCount; x++)
                        AddFrame(skin.GetTexture("Receptor" + x));
                    return;
            }
        }
    }
}
