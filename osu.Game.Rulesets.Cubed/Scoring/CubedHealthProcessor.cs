using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Scoring {
    public partial class CubedHealthProcessor : HealthProcessor {
        private int ObjectCountCapped;

        protected override double GetHealthIncreaseFor(JudgementResult result) => result.Type switch {
            HitResult.Perfect or HitResult.Great => 4f / ObjectCountCapped,
            HitResult.Good => 2f / ObjectCountCapped,
            // Misses often look rather lenient, this could be made harsher, or I could implement the HP beatmap stat
            HitResult.Meh or HitResult.Miss => -16f / ObjectCountCapped,
            _ => 0
        };

        public override void ApplyBeatmap(IBeatmap beatmap) {
            base.ApplyBeatmap(beatmap);
            ObjectCountCapped = System.Math.Min(beatmap.HitObjects.Count, 256);
        }
    }
}
