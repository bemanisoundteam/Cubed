using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Cubed.Objects {
    // TODO I didn't understand what Samples does, and I just need this shipped already,
    // in case I'm brave enough to tackle this, don't forget about the nested HitObject's samples
    public class CubedHoldNote : CubedHitObject, IHasDuration {
        private CubedHoldHead Head;

        public double EndTime {
            get => StartTime + Duration;
            set => Duration = value - StartTime;
        }

        public double Duration { get; set; }

        public override double StartTime {
            get => base.StartTime;
            set {
                base.StartTime = value;

                if (Head != null)
                    Head.StartTime = value;
            }
        }

        public HoldDirection Direction;

        private int tailLength;
        public int TailLength {
            get => tailLength;
            set {
                System.Diagnostics.Debug.Assert(value >= 0);
                tailLength = value;

                int notePosInAxis = (int) Direction % 2 == 0 ? Row : Column;
                if (Direction is HoldDirection.Right or HoldDirection.Down)
                    notePosInAxis = 3 - notePosInAxis;

                // Make so holds won't go offscreen
                tailLength = int.Min(tailLength, notePosInAxis);
            }
        }

        protected override void CreateNestedHitObjects(System.Threading.CancellationToken cancellationToken) =>
            AddNested(Head = new CubedHoldHead {
                StartTime = StartTime,
                // Action doesn't need to be populated, as positioning logic doesn't run in nested objects
                Samples = Samples
            });
    }

    public enum HoldDirection {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}
