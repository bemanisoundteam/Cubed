using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Scoring {
    public class CubedScoreProcessor : ScoreProcessor {
        public CubedScoreProcessor(Ruleset ruleset) : base(ruleset) {}

        // TODO This function is just to make so it compiles again, score needs to be implemented better !!!!!!
        protected override double ComputeTotalScore(double comboProgress, double accuracyProgress, double bonusPortion) => 1000000 * accuracyProgress;
    }
}
