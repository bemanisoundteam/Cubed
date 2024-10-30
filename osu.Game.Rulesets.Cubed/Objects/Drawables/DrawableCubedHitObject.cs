using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public abstract partial class DrawableCubedHitObject(CubedHitObject hitObject) : DrawableHitObject<CubedHitObject>(hitObject) {
        protected override double InitialLifetimeOffset => HitObject.TimePreempt;

        [BackgroundDependencyLoader]
        private void load() {
            Alpha = 0;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
            Scale = Vector2.Zero;
        }

        public bool OnHit() => UpdateResult(true);
        public void OnRelease() { }

        protected override void UpdateInitialTransforms() {
            this.FadeInFromZero(250, Easing.OutQuint);
            this.ScaleTo(1, 500);
        }
    }
}
