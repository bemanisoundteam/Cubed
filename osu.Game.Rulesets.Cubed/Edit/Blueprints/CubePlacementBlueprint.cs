using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Edit;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public partial class CubePlacementBlueprint() : CubedPlacementBlueprint(new Cube()) {
        [BackgroundDependencyLoader]
        private void load() =>
            Child = new CubePlacementPiece();

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (!base.OnMouseDown(e))
                return false;

            EndPlacement(true);
            return true;
        }

        public override SnapResult UpdateTimeAndPosition(osuTK.Vector2 screenSpacePosition, double time) {
            SnapResult result = base.UpdateTimeAndPosition(screenSpacePosition, time);

            if (result.Playfield != null) {
                Child.Size = result.Playfield.DrawSize;
                Child.Scale = result.Playfield.Scale;
                Child.Position = result.Playfield.ToSpaceOfOtherDrawable(result.Playfield.Position, this);
            }

            return result;
        }
    }
}
