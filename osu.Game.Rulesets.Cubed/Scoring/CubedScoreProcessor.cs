using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Screens.Play;
using System.Reflection;

namespace osu.Game.Rulesets.Cubed.Scoring {
    public partial class CubedScoreProcessor : ScoreProcessor {
        private readonly BindableDouble bonus = new();
        private double bonusDivisor;
        private bool applyBonus;

        public CubedScoreProcessor(Ruleset ruleset) : base(ruleset) {
            bonus.BindValueChanged((e) =>
                typeof(ScoreProcessor).GetField("currentBonusPortion", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(this, e.NewValue));
            bonus.BindValueChanged((e) =>
                bonus.Value = System.Math.Clamp(e.NewValue, 0, 1024));
        }

        [Resolved]
        private Player Player { get; set; }

        protected override double ComputeTotalScore(double comboProgress, double accuracyProgress, double bonusPortion) =>
            900000 * Accuracy.Value * accuracyProgress +
            (applyBonus ? 100000 : 0) * bonusPortion / 1024;

        public override int GetBaseScoreForResult(HitResult result) => result switch {
            HitResult.Perfect => 10,
            HitResult.Great => 7,
            HitResult.Good => 4,
            HitResult.Meh => 1,
            _ => 0
        };

        protected override double GetBonusScoreChange(JudgementResult result) => result.Type switch {
            HitResult.Perfect or HitResult.Great => 2f / bonusDivisor,
            HitResult.Good => 1f / bonusDivisor,
            HitResult.Meh or HitResult.Miss => -8f / bonusDivisor,
            _ => 0
        };

        protected override void ApplyScoreChange(JudgementResult result) {
            bonus.Value += GetBonusScoreChange(result);

            if (JudgedHits != 0 && JudgedHits == MaxHits)
                Scheduler.AddDelayed(ApplyBonusAndMoveOn, 2000);
        }

        protected override void RemoveScoreChange(JudgementResult result) =>
            /* This makes bonus technically behave differently when reverting
             * Because of the edge case that's reverting a capped bonus increase
             * Currently not an issue as bonus is only added at the last object
             */
            bonus.Value -= GetBonusScoreChange(result);

        protected override void Reset(bool storeResults) {
            base.Reset(storeResults);

            bonusDivisor = System.Math.Min(MaxHits / 1024f, 1);
            bonus.Value = 0;
        }

        private void ApplyBonusAndMoveOn() {
            // This might happen if the user rewinds
            if (JudgedHits != MaxHits)
                return;

            applyBonus = true;
            typeof(ScoreProcessor).GetMethod("updateScore", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(this, null);
            PopulateScore(Player.Score.ScoreInfo);
            base.Update();
        }

        protected override void Update() {}
    }
}
