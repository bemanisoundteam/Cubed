using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using osuTK;

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

        /// <summary>
        /// The scale at which the marker should be previewed at
        /// Should be the inverse of the maximum scale this marker can reach
        /// </summary>
        public Vector2 PreviewScale => Vector2.One;
    }
}
