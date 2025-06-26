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
            AddInternal(glow = new CellGlow() { Alpha = 0 });

            // TODO put numbers that make sense for std (~6*) converts here
            RegisterPool<Cube, DrawableCube>(20, 100);
        }

        protected override HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new CubedHitObjectLifetimeEntry((CubedHitObject) hitObject);

        private bool Press() {
            glow.Alpha = 1;
            heldObject = (HitObjectContainer.AliveObjects.FirstOrDefault(obj => !obj.Judged) as DrawableCubedHitObject)!;
            return heldObject?.OnHit() ?? false;
        }

        private void Release() {
            glow.Alpha = 0;
            heldObject?.OnRelease();
            heldObject = null;
        }

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) =>
            (e.Action == Action && IsNotRewinding) && Press();

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) {
            if (e.Action == Action && IsNotRewinding)
                Release();
        }

        private bool IsNotRewinding => Clock is not IGameplayClock clock || !clock.IsRewinding;

        protected override bool OnMouseDown(MouseDownEvent e) {
            KeyBindingContainer.TriggerPressed(Action);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e) =>
            KeyBindingContainer.TriggerReleased(Action);

        protected override bool OnTouchDown(TouchDownEvent e) {
            KeyBindingContainer.TriggerPressed(Action);
            return true;
        }

        protected override void OnTouchUp(TouchUpEvent e) =>
            KeyBindingContainer.TriggerReleased(Action);
    }
}
