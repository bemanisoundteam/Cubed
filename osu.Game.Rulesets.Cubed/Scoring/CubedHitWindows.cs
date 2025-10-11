using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Scoring {
    public class CubedHitWindows : HitWindows {
        public static bool HitResultAllowed(HitResult result) {
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

        public override bool IsHitResultAllowed(HitResult result) => HitResultAllowed(result);

        public override void SetDifficulty(double difficulty) {  /* NO-OP */  }

        // With help from https://544332133981.hatenablog.com/entry/bemani-rank_4
        public override double WindowFor(HitResult result) => result switch {
            HitResult.Perfect => 42,
            HitResult.Great => 92,
            HitResult.Good => 166,
            HitResult.Meh => 250,
            HitResult.Miss => 400,
            _ => throw new System.ArgumentOutOfRangeException(nameof(result), result, null)
        };
    }
}
