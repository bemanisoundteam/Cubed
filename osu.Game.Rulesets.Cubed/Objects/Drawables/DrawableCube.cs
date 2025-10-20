using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCube(Cube cube) : DrawableCubedHitObject(cube) {
        public DrawableCube() : this(null) { }  // Required for pooling

        private CubedSkinnableDrawable marker;
        public IMarker Marker => (IMarker) marker.Drawable;

        [BackgroundDependencyLoader]
        private void load() =>
            AddInternal(marker = new CubedSkinnableDrawable(CubedSkinComponents.Marker));

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
            const double duration = 250;

            switch (state) {
                case ArmedState.Hit:
                    Marker.AnimateHit(duration);
                    this.FadeOut(duration).Expire();
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red);
                    this.FadeOut(duration).Expire();
                    this.ScaleTo(0.6f, duration, Easing.OutExpo);
                    break;
            }
        }
    }
}
