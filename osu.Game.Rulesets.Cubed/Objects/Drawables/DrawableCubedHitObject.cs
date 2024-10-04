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
            RelativePositionAxes = Axes.Both;
            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.25f);

            AddInternal(inputHandlerPiece = new CubedInputHandlerPiece() {
                Scale = Vector2.Zero,
                Hit = () => UpdateResult(true)
            });
        }

        protected override void OnApply() {
            base.OnApply();
            X = HitObject.Column / 4f;
            Y = HitObject.Row / 4f;
            inputHandlerPiece.Action = HitObject.Action;
        }

        protected override void UpdateInitialTransforms() {
            this.FadeInFromZero(250, Easing.OutQuint);
            foreach (Drawable drawable in this.InternalChildren)
                drawable.ScaleTo(0.9f, 500);
        }
    }
}
