using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Scoring {
    public class CubedHitWindows : HitWindows {
        public override bool IsHitResultAllowed(HitResult result) {
            switch (result) {
                case HitResult.Perfect:
                case HitResult.Great:
                case HitResult.Good:
                case HitResult.Meh:
                case HitResult.Miss:
                    return true;
            }
            return false;
        }

        // With help from https://544332133981.hatenablog.com/entry/bemani-rank_4
        protected override DifficultyRange [] GetRanges() => new [] {
            new DifficultyRange(HitResult.Perfect, 42, 42, 42),
            new DifficultyRange(HitResult.Great, 92, 92, 92),
            new DifficultyRange(HitResult.Good, 166, 166, 166),
            new DifficultyRange(HitResult.Meh, 250, 250, 250),
            new DifficultyRange(HitResult.Miss, 400, 400, 400)
        };
    }
}
