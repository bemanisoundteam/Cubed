using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public abstract partial class DrawableCubedHitObject : DrawableHitObject<CubedHitObject> {
        public DrawableCubedHitObject(CubedHitObject hitObject) : base(hitObject) { }

        [BackgroundDependencyLoader]
        private void load() {
            Alpha = 0;
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.25f);
        }

        protected override void OnApply() {
            base.OnApply();
            X = HitObject.Column / 4f;
            Y = HitObject.Row / 4f;
        }

        protected override void UpdateInitialTransforms() {
            this.FadeInFromZero(250, Easing.OutQuint);
            foreach (Drawable drawable in this.InternalChildren)
                drawable.ScaleTo(0.9f, 500);
        }
    }
}
