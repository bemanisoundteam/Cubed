using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces {
    public partial class CubePiece : CompositeDrawable {
        public Drawable Marker;
        public Drawable Approach;

        [BackgroundDependencyLoader]
        private void load() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            AddInternal(Marker = new Box { RelativeSizeAxes = Axes.Both });
            AddInternal(Approach = new Container {
                Child = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Transparent,
                },
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Scale = new Vector2(2.5f),
                Masking = true,
                BorderColour = Color4.Red,
                BorderThickness = 3,
            });
        }
    }
}
