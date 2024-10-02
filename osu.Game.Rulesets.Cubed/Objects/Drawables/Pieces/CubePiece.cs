using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces {
    public partial class CubePiece : CompositeDrawable {
        [BackgroundDependencyLoader]
        private void load() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            AddInternal(new Box() { RelativeSizeAxes = Axes.Both });
        }
    }
}
