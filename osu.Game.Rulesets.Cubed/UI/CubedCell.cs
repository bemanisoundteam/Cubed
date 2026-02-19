using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Cubed.Skinning;
using static osu.Game.Rulesets.Cubed.Skinning.CubedSkinComponents;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedCell : Playfield, IKeyBindingHandler<CubedAction> {
        public required CubedAction Action;
        public int Column => (int) Action % 4;
        public int Row => (int) Action / 4;

        [Cached]
        private Bindable<Vector2> PointerPosition = new ();
        // NOT Cached
        private PPSource ppSource;

        private readonly List<DrawableCubedHitObject> heldObjects = [];
        private uint pressCount;
        private uint positionalPressCount;

        private Drawable highlight;
        private Drawable glow;
        private Container borderContainer;

        // [Resolved], although injected through the BDL
        private CubedInputManager inputManager;
        private KeyBindingContainer<CubedAction> KeyBindingContainer;
        public Bindable<bool> HighlightCells { get; init; }
        public Bindable<bool> CellBorders { get; init; }

        public Container GlowProxyContainer { get; init; }

        [BackgroundDependencyLoader(true)]
        private void load(CubedInputManager manager) {
            inputManager = manager;
            KeyBindingContainer = manager?.KeyBindingContainer;

            AddInternal(highlight = new Box {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Cyan
            });
            HighlightCells?.BindValueChanged(e => highlight.Alpha = e.NewValue ? .1f : 0, true);

            AddInternal(borderContainer = new Container {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                BorderColour = Colour4.Black,
                BorderThickness = 3,

                Child = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Transparent
                },
                Alpha = 0
            });
            CellBorders?.BindValueChanged(e => borderContainer.Alpha = e.NewValue ? 1 : 0, true);

            AddInternal(glow = new CubedSkinnableDrawable(CellGlow) { Alpha = 0 });
            GlowProxyContainer?.Add(glow.CreateProxy());

            // TODO put numbers that make sense for std (~6*) converts here
            // Or even just precompute initialSize using an injected map (capped still probably)
            RegisterPool<Cube, DrawableCube>(20, 100);
            RegisterPool<CubedHoldNote, DrawableCubedHoldNote>(20, 100);
            RegisterPool<CubedHoldHead, DrawableCubedHoldHead>(20, 100);

            AddInternal(HitObjectContainer);
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
            positionalPressCount++;
            PointerPosition.Value = e.ScreenSpaceMousePosition;
            ppSource = PPSource.Mouse;

            KeyBindingContainer?.TriggerPressed(Action);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e) {
            positionalPressCount--;
            KeyBindingContainer?.TriggerReleased(Action);
        }

        protected override bool OnTouchDown(TouchDownEvent e) {
            positionalPressCount++;
            PointerPosition.Value = e.ScreenSpaceTouch.Position;
            ppSource = (PPSource) (int) PPSource.Touch1 + (int) e.ScreenSpaceTouch.Source;

            KeyBindingContainer?.TriggerPressed(Action);
            return true;
        }

        protected override void OnTouchUp(TouchUpEvent e) {
            positionalPressCount--;
            KeyBindingContainer?.TriggerReleased(Action);
        }

        protected override bool OnMouseMove(MouseMoveEvent e) {
            PointerPosition.Value = e.ScreenSpaceMousePosition;
            ppSource = PPSource.Mouse;
            return false;
        }

        protected override void OnTouchMove(TouchMoveEvent e) {
            PointerPosition.Value = e.ScreenSpaceTouch.Position;
            ppSource = (PPSource) (int) PPSource.Touch1 + (int) e.ScreenSpaceTouch.Source;
        }

        protected override void Update() {
            if (positionalPressCount == 0)
                // ppSource should be set to None here for correctness, but it's not going to be checked against anyway
                PointerPosition.Value = default;
            else
                // I didn't really test behaviors when releasing the input ppSource is set to
                // Touch theoretically fallbacks to mouse but mouse doesn't do so (and it might even be mapped to touch ?)
                PointerPosition.Value = ppSource switch {
                    PPSource.Mouse => inputManager.CurrentState.Mouse.Position,
                    // I didn't know you could have flying comparison operators, but it's pretty cool
                    >= PPSource.Touch1 and <= PPSource.TouchPen => inputManager.CurrentState.Touch.
                        GetTouchPosition((TouchSource) (int) ppSource - (int) PPSource.Touch1) ?? inputManager.CurrentState.Mouse.Position,
                    // This shouldn't be reached, but do the best we can do to have a viable-ish position
                    _ => DrawRectangle.Contains(inputManager.CurrentState.Mouse.Position) ? inputManager.CurrentState.Mouse.Position : default
                };
        }
    }

    internal enum PPSource {
        None,
        Mouse,
        Touch1,
        Touch2,
        Touch3,
        Touch4,
        Touch5,
        Touch6,
        Touch7,
        Touch8,
        Touch9,
        Touch10,
        TouchPen,
    }
}
