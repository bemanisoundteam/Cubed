using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Skinning.Indicators {
    public partial class DefaultIndicator : ConnectedPointerIndicator {
        protected override Drawable Connection => new HoldLine();

        protected override Drawable Pointer => new Triangle();

        private partial class HoldLine : CompositeDrawable {
            private const float LineThickness = .01f;
            private const int GlowRadius = 3;

            public HoldLine() {
                InternalChild = new Box { RelativeSizeAxes = Axes.Both };

                Masking = true;
                EdgeEffect = new EdgeEffectParameters {
                    Colour = Colour4.Cyan,
                    Type = EdgeEffectType.Glow,
                    Radius = GlowRadius,
                    // Offset to not bleed behind the receptor, also hides better the hold arrow not connecting properly
                    Offset = new Vector2(0, GlowRadius)
                };

                Width = LineThickness;
            }
        }
    }
}
