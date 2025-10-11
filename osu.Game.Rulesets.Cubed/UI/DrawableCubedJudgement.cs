using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class DrawableCubedJudgement : DrawableJudgement {
        public DrawableCubedJudgement() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
        }

        // protected override Drawable CreateDefaultJudgement(HitResult result) => new CubedJudgementPiece(result);
    }
}
