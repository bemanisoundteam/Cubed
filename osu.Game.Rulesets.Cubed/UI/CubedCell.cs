using osu.Framework.Allocation;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedCell : Playfield, IKeyBindingHandler<CubedAction> {
        public required CubedAction Action;
        public int Column => (int) Action % 4;
        public int Row => (int) Action / 4;

        private DrawableCubedHitObject heldObject;
        private CellGlow glow;

        private KeyBindingContainer<CubedAction> KeyBindingContainer;

        [BackgroundDependencyLoader]
        private void load(CubedInputManager manager) {
            KeyBindingContainer = manager.KeyBindingContainer;
            AddInternal(glow = new CellGlow { Alpha = 0 });

            // TODO put numbers that make sense for std (~6*) converts here
            RegisterPool<Cube, DrawableCube>(20, 100);
            RegisterPool<CubedHoldNote, DrawableCubedHoldNote>(20, 100);
            RegisterPool<CubedHoldHead, DrawableCubedHoldHead>(20, 100);
        }

        protected override HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new CubedHitObjectLifetimeEntry((CubedHitObject) hitObject);

        private bool Press() {
            glow.Alpha = 1;
            heldObject = (HitObjectContainer.AliveObjects.FirstOrDefault(obj => !obj.Judged) as DrawableCubedHitObject)!;
            return heldObject?.OnHit() ?? false;
        }

        private void Release() {
            heldObject?.OnRelease();
            heldObject = null;
        }

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) =>
            e.Action == Action && IsNotRewinding && Press();

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) {
            if (e.Action != Action)
                return;

            glow.Alpha = 0;
            if (IsNotRewinding)
                Release();
        }

        private bool IsNotRewinding => Clock is not IGameplayClock clock || !clock.IsRewinding;

        private int pressCount;

        protected override bool OnMouseDown(MouseDownEvent e) {
            pressCount++;
            KeyBindingContainer.TriggerPressed(Action);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e) {
            if (--pressCount == 0)
                KeyBindingContainer.TriggerReleased(Action);
        }

        protected override bool OnTouchDown(TouchDownEvent e) {
            pressCount++;
            KeyBindingContainer.TriggerPressed(Action);
            return true;
        }

        protected override void OnTouchUp(TouchUpEvent e) {
            if (--pressCount == 0)
                KeyBindingContainer.TriggerReleased(Action);
        }
    }
}
