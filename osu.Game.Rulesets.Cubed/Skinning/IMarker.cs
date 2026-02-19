using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public interface IMarker : IDrawable {
        /// <summary>
        /// Transform this marker until HitObject's StartTime
        /// </summary>
        /// <param name="time">Duration of the transform</param>
        public void AnimateApproach(double time);

        /// <summary>
        /// Apply Transforms to be displayed when the object gets hit
        /// </summary>
        /// <param name="duration">Duration of transforms</param>
        /// <param name="judgement">Judgement resulting from the hit</param>
        public void AnimateHit(double duration, JudgementResult judgement);
    }
}
