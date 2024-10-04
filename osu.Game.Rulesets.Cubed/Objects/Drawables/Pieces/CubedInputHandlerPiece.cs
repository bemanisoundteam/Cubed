using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Screens.Play;
using System;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces {
    public partial class CubedInputHandlerPiece : Component, IKeyBindingHandler<CubedAction> {
        public CubedAction Action;
        public required Func<bool> Hit;
        public Action Release = () => { };
        private KeyBindingContainer<CubedAction> keyBindingContainer;

        [BackgroundDependencyLoader]
        private void load(CubedInputManager inputManager) {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;

            keyBindingContainer = inputManager.KeyBindingContainer;
        }

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) {
            if (e.Action != Action || (Parent as DrawableHitObject).Judged)
                return false;

            return Hit();
        }

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) {
            if (e.Action == Action && !(Clock as IGameplayClock).IsRewinding)
                Release();
        }

        protected override bool OnTouchDown(TouchDownEvent e) {
            keyBindingContainer.TriggerPressed(Action);
            return true;
        }

        protected override void OnTouchUp(TouchUpEvent e) =>
            keyBindingContainer.TriggerReleased(Action);

        protected override bool OnMouseDown(MouseDownEvent e) {
            keyBindingContainer.TriggerPressed(Action);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e) =>
            keyBindingContainer.TriggerReleased(Action);
    }
}
