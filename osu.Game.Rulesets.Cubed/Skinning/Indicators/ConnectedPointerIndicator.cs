using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osuTK;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Skinning.Indicators {
    public abstract partial class ConnectedPointerIndicator : CompositeDrawable, IHoldIndicator {
        public CubedHoldNote Object { get; set; }
        [Resolved]
        public Dictionary<CubedHoldNote, double> PressTimes { get; private set; }

        // Pointer Starting Point
        private Vector2 PointerPosition;

        private Drawable pointer;
        private Drawable line;

        [BackgroundDependencyLoader]
        private void load() {
            RelativeSizeAxes = Axes.Both;

            line = Connection;
            line.RelativeSizeAxes = Axes.Both;
            line.RelativePositionAxes = Axes.Both;
            line.Anchor = Anchor.Centre;
            line.Origin = Anchor.TopCentre;
            AddInternal(line);

            pointer = Pointer;
            pointer.RelativeSizeAxes = Axes.Both;
            pointer.RelativePositionAxes = Axes.Both;
            pointer.Anchor = Anchor.Centre;
            pointer.Origin = Anchor.Centre;
            AddInternal(pointer);
        }

        protected abstract Drawable Pointer { get; }

        protected abstract Drawable Connection { get; }

        private HoldDirection _direction;
        public HoldDirection Direction {
            get => _direction;
            set {
                _direction = value;

                pointer.Rotation = (int) value * 90 - 180;
                line.Rotation = (int) value * 90 - 180;

                if ((int) value % 2 == 1)
                    line.Position = new Vector2(value is HoldDirection.Left ? .5f : -.5f, 0);
                else
                    line.Position = new Vector2(0, value is HoldDirection.Up ? .5f : -.5f);

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
                line.Height = (value + .5f) / CubedPlayfield.CellScale;

                // If there shouldn't be a line, at least make it so it doesn't draw one
                if (value == 0)
                    line.Height = 0;

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

            line.Scale = new Vector2(1, 1f - (float) progress);
            pointer.Position = PointerPosition * (1f - (float) progress);
        }
    }
}
