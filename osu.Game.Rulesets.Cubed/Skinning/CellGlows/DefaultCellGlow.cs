using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.Skinning.CellGlows {
    public partial class DefaultCellGlow() : BufferedContainer(pixelSnapping: true, cachedFrameBuffer: true) {
        [BackgroundDependencyLoader]
        private void hugeHack() {
            RelativeSizeAxes = Axes.Both;
            Masking = true;
            BlurSigma = new Vector2(25);
            BorderColour = ColourInfo.GradientHorizontal(Color4.Cyan, Color4.LimeGreen);
            BorderThickness = 2;
            DrawOriginal = true;
            Child = new Box {
                Colour = Color4.Transparent,
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
