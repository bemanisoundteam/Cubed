using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Cubed.Scoring;
using osu.Game.Rulesets.Cubed.Skinning;
using static osu.Game.Rulesets.Cubed.Skinning.CubedSkinComponents;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedCell : Playfield, IKeyBindingHandler<CubedAction> {
        public required CubedAction Action;
        public int Column => (int) Action % 4;
        public int Row => (int) Action / 4;

        private readonly List<DrawableCubedHitObject> heldObjects = [];
        private uint pressCount;
        private Drawable glow;

        private KeyBindingContainer<CubedAction> KeyBindingContainer;

        private readonly JudgementContainer<DrawableCubedJudgement> judgements = new() { RelativeSizeAxes = Axes.Both };
        private readonly Container judgementsAboveHitObjects = new() { RelativeSizeAxes = Axes.Both };
        private JudgementPooler<DrawableCubedJudgement> judgementPool;

        [BackgroundDependencyLoader(true)]
        private void load(CubedInputManager manager) {
            KeyBindingContainer = manager?.KeyBindingContainer;
            AddInternal(glow = new CubedSkinnableDrawable(CellGlow) { Alpha = 0 });

            // TODO put numbers that make sense for std (~6*) converts here
            // Or even just precompute initialSize using an injected map (capped still probably)
            RegisterPool<Cube, DrawableCube>(20, 100);
            RegisterPool<CubedHoldNote, DrawableCubedHoldNote>(20, 100);
            RegisterPool<CubedHoldHead, DrawableCubedHoldHead>(20, 100);

            AddInternal(judgements);
            AddInternal(HitObjectContainer);
            AddInternal(judgementsAboveHitObjects);

            // I don't think placing the pool here does something about proxying above HitObjects,
            // but osu! does it like this and I can't test it yet
            // FIXME This registers a stupid amount of pools, possible performance regression (I didn't measure it, but stress tests stutter on high object spikes)
            // Requires RenderDoc + profiling (I have around 2500-3000 instances of ProxyDrawable on razor sharp, I need to test impact on unlimited framerate, but this is amongst top CPU consumers...)
            // And still we might want to preload the right amount of judgements
            AddInternal(judgementPool = new(System.Enum.GetValues<HitResult>().Where(CubedHitWindows.HitResultAllowed)
            , judgement => judgementsAboveHitObjects.Add(judgement.ProxiedAboveHitObjectsContent)));
        }

        protected override void LoadComplete() {
            base.LoadComplete();

            NewResult += OnNewResult;
        }

        private void OnNewResult(DrawableHitObject judgedObject, JudgementResult result) {
            if (!judgedObject.DisplayResult || !DisplayJudgements.Value)
                return;

            // Original clears here, if it's broken we just need to do
            // judgements.Clear(false);
            judgements.Add(judgementPool.Get(result.Type, j => j.Apply(result, judgedObject))!);
        }

        protected override HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new CubedHitObjectLifetimeEntry((CubedHitObject) hitObject);

        private bool Press() {
            pressCount++;
            glow.Alpha = 1;
            var heldObject = HitObjectContainer.AliveObjects.FirstOrDefault(obj => !obj.Judged) as DrawableCubedHitObject;

            if (heldObject == null || !heldObject.OnHit())
                return false;

            heldObjects.Add(heldObject);
            return true;

        }

        private void Release() {
            if (--pressCount != 0)
                return;

            glow.Alpha = 0;
            foreach (DrawableCubedHitObject heldObject in heldObjects)
                heldObject.OnRelease();

            heldObjects.Clear();
        }

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) =>
            e.Action == Action && Press();

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) {
            if (e.Action == Action)
                Release();
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            KeyBindingContainer?.TriggerPressed(Action);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e) =>
            KeyBindingContainer?.TriggerReleased(Action);

        protected override bool OnTouchDown(TouchDownEvent e) {
            KeyBindingContainer?.TriggerPressed(Action);
            return true;
        }
        protected override void OnTouchUp(TouchUpEvent e) =>
            KeyBindingContainer?.TriggerReleased(Action);

        protected override void Dispose(bool isDisposing) {
            // must happen before children are disposed in base call to prevent illegal accesses to the judgement pool.
            NewResult -= OnNewResult;

            base.Dispose(isDisposing);
        }
    }
}
