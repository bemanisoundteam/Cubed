using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public abstract partial class DrawableCubedHitObject(CubedHitObject hitObject) : DrawableHitObject<CubedHitObject>(hitObject) {
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
            this.FadeInFromZero(250, Easing.OutQuint);
    }
}
