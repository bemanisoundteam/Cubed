using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.Indicators;
using osu.Game.Rulesets.Cubed.Skinning.Markers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCubedHoldNote(CubedHoldNote hold) : DrawableCubedHitObject(hold) {
        public DrawableCubedHoldNote() : this(null) { }  // Required for pooling
        public new CubedHoldNote HitObject => (CubedHoldNote) base.HitObject;

        public CubedSkinnableDrawable Receptor { get; set; }

        private CubedSkinnableDrawable indicator;
        public IHoldIndicator Indicator => (IHoldIndicator) indicator.Drawable;

        private CubedSkinnableDrawable judgement;
        public IMarker JudgementMarker => (IMarker) judgement.Drawable;

        [BackgroundDependencyLoader]
        private void load() {
            // Order is important, Head above Receptor above Indicator

            AddInternal(indicator = new CubedSkinnableDrawable(CubedSkinComponents.Indicator));
            AddInternal(Receptor = new CubedSkinnableDrawable(CubedSkinComponents.Receptor));
            AddInternal(judgement = new CubedSkinnableDrawable(CubedSkinComponents.Marker) { Alpha = 0 });

            AddInternal(headContainer = new Container { RelativeSizeAxes = Axes.Both });
        }

        private void RefreshIndicator() {
            Indicator.Direction = HitObject.Direction;
            Indicator.Length = HitObject.TailLength;

            Indicator.ComputeProgress = () => {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (head.Result.TimeAbsolute == HitObject.EndTime)
                    return 0;

                return double.Clamp((Time.Current - head.Result.TimeAbsolute) / (HitObject.EndTime - head.Result.TimeAbsolute), 0, 1);
            };
        }

        private void RefreshReceptor() => Receptor.Rotation = (int) HitObject.Direction * 90;

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
            RefreshIndicator();
            RefreshReceptor();
        }

        protected override void ApplySkin(ISkinSource skin, bool allowFallback) {
            if (HitObject == null)
                return;

            indicator.FlushPendingSkinChange();
            RefreshIndicator();

            judgement.FlushPendingSkinChange();
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

        public override bool OnHit() => head.OnHit();

        public override void OnRelease() {
            // Remove this check for the drake note experience, I dare you
            if (HitObject != null)
                UpdateResult(true);
        }

        protected override void UpdateHitStateTransforms(ArmedState state) {
            base.UpdateHitStateTransforms(state);

            if (state == ArmedState.Hit) {
                judgement.Show();
                JudgementMarker.AnimateHit(TransformsDuration, Result);
            }
        }
    }
}
