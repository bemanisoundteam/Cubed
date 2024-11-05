using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces {
    public partial class CubeSelectionPiece : Container {
        [BackgroundDependencyLoader]
        private void load(OsuColour colors) {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            Child = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Transparent
            };

            Masking = true;
            BorderColour = colors.Yellow;
            BorderThickness = 3;
        }
    }
}
