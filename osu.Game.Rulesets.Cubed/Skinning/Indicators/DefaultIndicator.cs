using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Skinning.Indicators {
    public partial class DefaultIndicator : CompositeDrawable, IHoldIndicator {
        private const float LineThickness = .01f;

        public CubedHoldNote Object { get; set; }

        // Pointer Starting Point
        private Vector2 PointerPosition;

        private readonly Drawable Pointer;
        private readonly Drawable Line;

        public DefaultIndicator() {
            RelativeSizeAxes = Axes.Both;

            AddInternal(Line = new HoldLine {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                Width = LineThickness
            });
            AddInternal(Pointer = new Triangle {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            });
        }

        private HoldDirection _direction;
        public HoldDirection Direction {
            get => _direction;
            set {
                _direction = value;

                Pointer.Rotation = (int) value * 90 - 180;
                Line.Rotation = (int) value * 90 - 180;

                if ((int) value % 2 == 1)
                    Line.Position = new Vector2(value is HoldDirection.Left ? .5f : -.5f, 0);
                else
                    Line.Position = new Vector2(0, value is HoldDirection.Up ? .5f : -.5f);

                // Uncomment to make so instead of filling the hold it stops at it
                // Line.Position *= -1;

                // And if you instead want it to be centered on the hold:
                // Line.Position = Vector2.Zero

                UpdatePointerPosition();
            }
        }

        private int _length;
        public int Length {
            get => _length;
            set {
                _length = value;

                // The .5f is to make it render under the arrow
                Line.Height = (value + .5f) / CubedPlayfield.CellScale;

                // If there shouldn't be a line, at least make it so it doesn't draw one
                if (value == 0)
                    Line.Height = 0;

                UpdatePointerPosition();
            }
        }

        private void UpdatePointerPosition() {
            if ((int) Direction % 2 == 0)
                PointerPosition = new Vector2(0, Length * (Direction == HoldDirection.Up ? -1 : 1));
            else
                PointerPosition = new Vector2(Length * (Direction == HoldDirection.Left ? -1 : 1), 0);

            // Scale up so that it aligns with cells
            PointerPosition /= CubedPlayfield.CellScale;
        }

        // Transforms being a mess forced my hand
        protected override void Update() {
            // C# is retarded, I have to cast as the interface to use the default implementation...
            double progress = ((IHoldIndicator) this).ComputeProgress();

            Line.Scale = new Vector2(1, 1f - (float) progress);
            Pointer.Position = PointerPosition * (1f - (float) progress);
        }

        private partial class HoldLine : CompositeDrawable {
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
            }
        }
    }
}
