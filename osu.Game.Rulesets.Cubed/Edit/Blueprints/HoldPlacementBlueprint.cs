using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Utils;
using osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osuTK;
using osuTK.Input;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public partial class HoldPlacementBlueprint() : CubedPlacementBlueprint<CubedHoldNote>(new CubedHoldNote()) {
        private HoldBodyPlacementPiece indicatorPiece;

        protected override bool IsValidForPlacement {
            get {
                if (!base.IsValidForPlacement)
                    return false;

                if (PlacementActive == PlacementState.Waiting)
                    return true;

                return HitObject.TailLength > 0 && Precision.DefinitelyBigger(HitObject.Duration, 0);
            }
        }

        [BackgroundDependencyLoader]
        private void load() =>
            Children = [
                indicatorPiece = new HoldBodyPlacementPiece(HitObject) { Alpha = 0 },
                new CubePlacementPiece()
            ];

        protected override void OnMouseUp(MouseUpEvent e) {
            if (e.Button == MouseButton.Left)
                EndPlacement(true);
        }

        public override SnapResult UpdateTimeAndPosition(Vector2 screenSpacePosition, double time) {
            SnapResult result = base.UpdateTimeAndPosition(screenSpacePosition, time);

            if (PlacementActive != PlacementState.Active)
                return result;

            HitObject.EndTime = time;

            if (result.Playfield is not CubedCell targetCell)
                return result;

            if (targetCell != Cell) {
                bool rowMatches = targetCell.Row == HitObject.Row;
                bool columnMatches = targetCell.Column == HitObject.Column;

                if (rowMatches == columnMatches)
                    return result;

                if (rowMatches) {
                    HitObject.Direction = HitObject.Column - targetCell.Column > 0 ? HoldDirection.Left : HoldDirection.Right;
                    HitObject.TailLength = int.Abs(HitObject.Column - targetCell.Column);
                }
                if (columnMatches) {
                    HitObject.Direction = HitObject.Row - targetCell.Row > 0 ? HoldDirection.Up :  HoldDirection.Down;
                    HitObject.TailLength = int.Abs(HitObject.Row - targetCell.Row);
                }

                indicatorPiece.Alpha = 1;
                indicatorPiece.Refresh();
            }
            else {
                indicatorPiece.Alpha = 0;
                HitObject.TailLength = 0;
            }

            return result;
        }
    }
}
