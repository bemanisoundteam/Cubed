using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedCell : Playfield, IKeyBindingHandler<CubedAction> {
        public required CubedAction Action;
        private DrawableCubedHitObject heldObject;
        private CellGlow glow;

        [BackgroundDependencyLoader]
        private void load() {
            AddInternal(glow = new CellGlow() { Alpha = 0 });

            // TODO put numbers that make sense for std (~6*) converts here
            RegisterPool<Cube, DrawableCube>(20, 100);
        }

        private bool Press() {
            glow.FadeIn();
            var _ = (HitObjectContainer.AliveObjects.FirstOrDefault(obj => !obj.Judged) as DrawableCubedHitObject)!;
            heldObject = _;
            return _?.OnHit() ?? false;
        }

        private void Release() {
            glow.FadeOut();
            heldObject?.OnRelease();
            heldObject = null;
        }

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) =>
            (e.Action == Action && !(Clock as IGameplayClock)!.IsRewinding) && Press();

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) {
            if (e.Action == Action && !(Clock as IGameplayClock)!.IsRewinding)
                Release();
        }
    }
}
