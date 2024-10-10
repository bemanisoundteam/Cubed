using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public abstract partial class DrawableCubedHitObject : DrawableHitObject<CubedHitObject> {
        protected CubedInputHandlerPiece inputHandlerPiece;

        public DrawableCubedHitObject(CubedHitObject hitObject) : base(hitObject) { }

        [BackgroundDependencyLoader]
        private void load() {
            Alpha = 0;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
            Scale = Vector2.Zero;

            AddInternal(inputHandlerPiece = new CubedInputHandlerPiece() {
                Hit = () => UpdateResult(true)
            });
        }

        protected override void OnApply() {
            base.OnApply();
            inputHandlerPiece.Action = HitObject.Action;
        }

        protected override void UpdateInitialTransforms() {
            this.FadeInFromZero(250, Easing.OutQuint);
            this.ScaleTo(1, 500);
        }
    }
}
