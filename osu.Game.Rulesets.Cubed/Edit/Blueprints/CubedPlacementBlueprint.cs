using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osuTK.Input;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public abstract partial class CubedPlacementBlueprint(CubedHitObject hitObject) : HitObjectPlacementBlueprint(hitObject) {
        private CubedPlayfield playfield;

        [BackgroundDependencyLoader]
        private void load(HitObjectComposer composer) =>
            playfield = (CubedPlayfield) composer.Playfield;

        protected CubedCell Cell {
            get;
            set {
                if (value == field)
                    return;

                field = value;
                if (field != null)
                    hitObject.Action = field.Action;
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (e.Button != MouseButton.Left || Cell == null)
                return false;

            BeginPlacement(true);
            return true;
        }

        public override bool ReplacesExistingObject(HitObject existing) =>
            base.ReplacesExistingObject(existing) && hitObject.Action == ((CubedHitObject) existing).Action;

        public override SnapResult UpdateTimeAndPosition(osuTK.Vector2 screenSpacePosition, double time) {
            base.UpdateTimeAndPosition(screenSpacePosition, time);
            var targetCell = playfield.GetCell(screenSpacePosition);

            if (PlacementActive == PlacementState.Waiting)
                Cell = targetCell;

            return new SnapResult(screenSpacePosition, time, targetCell);
        }
    }
}
