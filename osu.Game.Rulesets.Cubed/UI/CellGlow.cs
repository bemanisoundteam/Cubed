using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CellGlow : BufferedContainer {
        [BackgroundDependencyLoader]
        private void load() {
            RelativeSizeAxes = Axes.Both;
            Masking = true;
            BlurSigma = new Vector2(25);
            BorderColour = Color4.Cyan;
            BorderThickness = 2;
            DrawOriginal = true;
            Child = new Box {
                Colour = Color4.Transparent,
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
