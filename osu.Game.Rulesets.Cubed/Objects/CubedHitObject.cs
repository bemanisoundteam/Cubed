using osu.Game.Rulesets.Cubed.Scoring;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Objects {
    public abstract class CubedHitObject : HitObject {
        public int Column { get; set; }
        public int Row { get; set; }

        protected override HitWindows CreateHitWindows() => new CubedHitWindows();
    }
}
