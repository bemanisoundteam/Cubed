using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces {
    public partial class CubedHoldLine : CompositeDrawable {
        private const int GlowRadius = 3;

        [BackgroundDependencyLoader]
        private void load() {
            InternalChild = new Box { RelativeSizeAxes = Axes.Both };

            Masking = true;
            EdgeEffect = new EdgeEffectParameters {
                Colour = Colour4.Cyan,
                Type = EdgeEffectType.Glow,
                Radius = GlowRadius,
                // Offset to not bleed behind the receptor, also hides better the hold arrow not connecting properly
                Offset = new Vector2(0, GlowRadius)
            };
        }
    }
}
