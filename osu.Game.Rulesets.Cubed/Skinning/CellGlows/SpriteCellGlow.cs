using osu.Framework.Allocation;
using osu.Framework.Graphics.Animations;
using osu.Framework.Logging;
using System.Threading;

namespace osu.Game.Rulesets.Cubed.Skinning.CellGlows {
    public partial class SpriteCellGlow : TextureAnimation {
        private readonly int frameCount;

        public SpriteCellGlow(int frameCount, double framerate = 0) : base(false) {
            this.frameCount = frameCount;

            // FP Division gives Infinity in case framerate is 0
            DefaultFrameLength = 1000 / framerate;
        }

        [BackgroundDependencyLoader]
        private void load(ICellGlowSkin skin, CancellationToken cancellation) {
            switch (frameCount) {
                case 0:
                    Logger.Log($"CellGlow \"{skin.SkinInfo}\" has 0 frames !");
                    return;

                case 1:
                    AddFrame(skin.GetTexture("CellGlow") ?? skin.GetTexture("CellGlow0"));
                    return;

                default:
                    for (int i = 0; i < frameCount && !cancellation.IsCancellationRequested; i++)
                        AddFrame(skin.GetTexture("CellGlow" + i));
                    return;
            }
        }
    }
}
