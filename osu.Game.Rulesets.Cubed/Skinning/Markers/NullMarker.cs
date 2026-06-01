using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Cubed.Skinning.Markers {
    public partial class NullMarker : Drawable, IMarker {
        public void AnimateApproach(double time) {}

        public void AnimateHit(double duration, JudgementResult judgement) {}
    }
}
