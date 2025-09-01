using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osuTK.Input;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public abstract partial class CubedPlacementBlueprint(CubedHitObject hitObject) : HitObjectPlacementBlueprint(hitObject) {
        [Resolved]
        private HitObjectComposer composer { get; set; }
        private CubedPlayfield playfield => (CubedPlayfield) composer.Playfield;

        private CubedCell cell;
        protected CubedCell Cell {
            get => cell;
            set {
                if (value == cell) return;

                cell = value;
                ((CubedHitObject) HitObject).Action = cell.Action;
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (e.Button != MouseButton.Left || Cell == null)
                return false;

            BeginPlacement(true);
            return true;
        }

        public override SnapResult UpdateTimeAndPosition(osuTK.Vector2 screenSpacePosition, double time) {
            base.UpdateTimeAndPosition(screenSpacePosition, time);
            SnapResult result = new (screenSpacePosition, time, playfield.GetCell(screenSpacePosition));

            if (result.Playfield is CubedCell targetCell && PlacementActive == PlacementState.Waiting)
                Cell = targetCell;

            return result;
        }
    }
}
