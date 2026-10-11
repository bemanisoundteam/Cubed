using osu.Game.Audio;
using osu.Game.Rulesets.Objects.Types;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Objects {
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

        public IList<HitSampleInfo> HeadSamples {
            get;
            set {
                if (Equals(field, value))
                    return;

                field = value;
                Head?.Samples = value;
            }
        } = [];

        protected override void CreateNestedHitObjects(System.Threading.CancellationToken cancellationToken) =>
            AddNested(Head = new CubedHoldHead {
                StartTime = StartTime,
                // Action doesn't need to be populated, as positioning logic doesn't run in nested objects
                Samples = HeadSamples
            });
    }

    public enum HoldDirection {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}
