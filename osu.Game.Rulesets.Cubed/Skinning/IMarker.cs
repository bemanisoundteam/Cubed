using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public interface IMarker : IDrawable {
        /// <summary>
        /// Animate this marker until HitObject's StartTime
        /// </summary>
        /// <param name="time">Duration of the animation</param>
        public void AnimateApproach(double time);

        /// <summary>
        /// Called on object hit, to optionally animate object hit
        /// </summary>
        /// <remarks>Markers are drawn under the judgments</remarks>
        /// <param name="duration">Time before object fades out</param>
        public void AnimateHit(double duration) {}
    }
}
