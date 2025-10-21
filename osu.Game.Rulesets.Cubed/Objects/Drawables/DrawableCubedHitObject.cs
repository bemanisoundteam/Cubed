using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public abstract partial class DrawableCubedHitObject(CubedHitObject hitObject) : DrawableHitObject<CubedHitObject>(hitObject) {
        public const double TransformsDuration = 250;

        protected override double InitialLifetimeOffset => HitObject.TimePreempt;

        [BackgroundDependencyLoader]
        private void load() {
            Alpha = 0;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
        }

        public virtual bool OnHit() => UpdateResult(true);
        public virtual void OnRelease() { }

        protected override void UpdateInitialTransforms() =>
            this.FadeInFromZero(TransformsDuration, Easing.OutQuint);

        protected override void UpdateHitStateTransforms(ArmedState state) {
            switch (state) {
                case ArmedState.Hit:
                    this.FadeOut(TransformsDuration).Expire();
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Colour4.Red);
                    this.FadeOut(TransformsDuration).Expire();
                    this.ScaleTo(.8f, TransformsDuration, Easing.OutExpo);
                    break;
            }
        }
    }
}
