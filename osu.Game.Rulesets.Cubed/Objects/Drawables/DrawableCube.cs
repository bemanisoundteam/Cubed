using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCube(Cube cube) : DrawableCubedHitObject(cube) {
        public DrawableCube() : this(null) { }  // Required for pooling

        private CubedSkinnableDrawable marker;
        public IMarker Marker => (IMarker) marker.Drawable;

        [BackgroundDependencyLoader]
        private void load() =>
            AddInternal(marker = new CubedSkinnableDrawable(CubedSkinComponents.Marker));

        protected override void LoadComplete() {
            base.LoadComplete();

            marker.OnSkinChanged += RefreshStateTransforms;
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

        protected override void Dispose(bool isDisposing) {
            base.Dispose(isDisposing);

            marker.OnSkinChanged -= RefreshStateTransforms;
        }
    }
}
