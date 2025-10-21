using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCubedHoldNote(CubedHoldNote hold) : DrawableCubedHitObject(hold) {
        public DrawableCubedHoldNote() : this(null) { }  // Required for pooling
        public new CubedHoldNote HitObject => (CubedHoldNote) base.HitObject;

        public readonly Bindable<HoldDirection> Direction = new ();
        public readonly BindableInt TailDistance = new() {
            MinValue = 0,
            MaxValue = 3
        };

        public CubedHoldReceptor Receptor { get; set; }
        public CubedHoldIndicator Indicator { get; set; }

        [BackgroundDependencyLoader]
        private void load() {
            // Order is important, Head above Receptor above Indicator

            AddInternal(Indicator = new CubedHoldIndicator(Direction, TailDistance));
            AddInternal(Receptor = new CubedHoldReceptor(Direction));

            AddInternal(headContainer = new Container { RelativeSizeAxes = Axes.Both });
        }

        private DrawableCubedHoldHead head => (DrawableCubedHoldHead) headContainer.Child;

        private Container headContainer;

        protected override void AddNestedHitObject(DrawableHitObject hitObject) {
            if (hitObject is DrawableCubedHoldHead dHead)
                headContainer.Child = dHead;
        }

        protected override void ClearNestedHitObjects() =>
            headContainer.Clear(false);

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject) =>
            hitObject switch {
                CubedHoldHead dHead => new DrawableCubedHoldHead(dHead),
                _ => base.CreateNestedHitObject(hitObject)
            };

        protected override void OnApply() {
            Indicator.SetObject(HitObject);
            Direction.Value = HitObject.Direction;
            TailDistance.Value = HitObject.TailLength;
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset) {
            if (head.Result.Type == HitResult.Miss)
                ApplyMinResult();

            if (!userTriggered) {
                if (timeOffset >= 0 && head.IsHit)
                    ApplyMaxResult();

                return;
            }

            // Only reached on user releases done when timeOffset < 0
            HitResult result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None) {
                ApplyMinResult();
                return;
            }

            ApplyResult(result);
        }

        public override bool OnHit() {
            if (!head.OnHit())
                return false;

            Indicator.OnHit();
            return true;
        }

        public override void OnRelease() {
            // Remove this check for the drake note experience, I dare you
            if (HitObject != null)
                UpdateResult(true);
        }
    }
}
