using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCube : DrawableCubedHitObject {
        public DrawableCube() : this(null) { }  // Required for pooling
        public DrawableCube(Cube cube) : base(cube) { }

        [BackgroundDependencyLoader]
        private void load() {
            AddInternal(new CubePiece() { Scale = Vector2.Zero });
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset) {
            if (timeOffset >= 0)
                // todo: implement judgement logic
                ApplyResult(HitResult.Perfect);
        }

        protected override void UpdateHitStateTransforms(ArmedState state) {
            const double duration = 250;

            switch (state) {
                case ArmedState.Hit:
                    this.FadeOut(duration).Expire();
                    foreach (Drawable drawable in this.InternalChildren)
                        drawable.ScaleTo(1.5f, duration, Easing.OutQuint);
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red);
                    this.FadeOut(duration).Expire();
                    foreach (Drawable drawable in this.InternalChildren)
                        drawable.ScaleTo(0.6f, duration, Easing.OutExpo);
                    break;
            }
        }
    }
}
