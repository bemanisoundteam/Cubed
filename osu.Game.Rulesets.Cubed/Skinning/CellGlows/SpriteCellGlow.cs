using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;

namespace osu.Game.Rulesets.Cubed.Skinning.CellGlows {
    public partial class SpriteCellGlow(int frameCount, double framerate = default) : CompositeDrawable {
        [BackgroundDependencyLoader]
        private void load(ICellGlowSkin skin) {
            switch (frameCount) {
                case 0:
                    Logger.Log($"CellGlow \"{skin.SkinInfo}\" has 0 frames !");
                    return;

                case 1:
                    InternalChild = new Sprite {
                        RelativeSizeAxes = Axes.Both,
                        Texture = skin.GetTexture("CellGlow") ?? skin.GetTexture("CellGlow0")
                    };
                    return;

                default:
                    TextureAnimation animation = new(false) {
                        RelativeSizeAxes = Axes.Both,
                        Loop = true
                    };

                    for (int i = 0; i < frameCount; i++)
                        animation.AddFrame(skin.GetTexture($"CellGlow{i}"), 1000 / framerate);

                    InternalChild = animation;
                    return;
            }
        }
    }
}
