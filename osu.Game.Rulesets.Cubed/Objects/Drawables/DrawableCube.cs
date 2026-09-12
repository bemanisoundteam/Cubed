using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.Markers;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCube(Cube cube) : DrawableCubedHitObject(cube) {
        public DrawableCube() : this(null) { }  // Required for pooling

        private CubedSkinnableDrawable marker;
        public IMarker Marker => (IMarker) marker.Drawable;

        [BackgroundDependencyLoader]
        private void load() =>
            AddInternal(marker = new CubedSkinnableDrawable(CubedSkinComponents.Marker));

        protected override void ApplySkin(ISkinSource skin, bool allowFallback) {
            if (HitObject == null)
                return;

            marker.FlushPendingSkinChange();
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset) {
            if (!userTriggered) {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                    ApplyResult(HitResult.Miss);
                return;
            }

            HitResult result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            ApplyResult(result);
        }

        protected override void UpdateInitialTransforms() {
            base.UpdateInitialTransforms();

            Marker.AnimateApproach(InitialLifetimeOffset);
        }

        protected override void UpdateHitStateTransforms(ArmedState state) {
            base.UpdateHitStateTransforms(state);

            if (state == ArmedState.Hit)
                Marker.AnimateHit(TransformsDuration, Result);
        }
    }
}
