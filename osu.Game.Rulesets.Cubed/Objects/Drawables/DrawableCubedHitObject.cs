using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCubedHitObject : DrawableHitObject<CubedHitObject> {
        public DrawableCubedHitObject(CubedHitObject hitObject) : base(hitObject) {
            Origin = Anchor.Centre;

            // todo: add visuals.
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset) {
            if (timeOffset >= 0)
                // todo: implement judgement logic
                ApplyResult(r => r.Type = HitResult.Perfect);
        }

        protected override void UpdateHitStateTransforms(ArmedState state) {
            const double duration = 1000;

            switch (state) {
                case ArmedState.Hit:
                    this.FadeOut(duration, Easing.OutQuint).Expire();
                    break;

                case ArmedState.Miss:
                    this.FadeColour(Color4.Red, duration);
                    this.FadeOut(duration, Easing.InQuint).Expire();
                    break;
            }
        }
    }
}
