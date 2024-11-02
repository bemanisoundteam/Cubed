using osu.Game.Rulesets.Cubed.Scoring;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Objects {
    public abstract class CubedHitObject : HitObject, IHasTimePreempt {
        public int Column { get; set; }
        public int Row { get; set; }

        public CubedAction Action {
            get => (CubedAction) (Column + Row * 4);
            set {
                Row = (int) value / 4;
                Column = (int) value % 4;
            }
        }

        protected override HitWindows CreateHitWindows() => new CubedHitWindows();

        public double TimePreempt => 500;
    }
}
