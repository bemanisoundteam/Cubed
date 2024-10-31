using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCube : DrawableCubedHitObject {
        public DrawableCube() : this(null) { } // Required for pooling
        public DrawableCube(Cube cube) : base(cube) { }

        public CubePiece Piece { get; private set; }

        [BackgroundDependencyLoader]
        private void load() {
            AddInternal(Piece = new CubePiece());
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
            Piece.Approach.ScaleTo(1, InitialLifetimeOffset).Then().FadeOut();
        }

        protected override void UpdateHitStateTransforms(ArmedState state) {
            const double duration = 250;

            switch (state) {
                case ArmedState.Hit:
                    // Piece.Approach.Alpha = 0;  this is disabled because I fade the entire object and not it's individual parts, causing approaches to never appear
                    // Instead I scale it down to zero (has the same effect in terms of making it invisible and making IsPresent false)
                    Piece.Approach.ScaleTo(1);
                    this.FadeOut(duration).Expire();
                    this.ScaleTo(1.5f, duration, Easing.OutQuint);
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
