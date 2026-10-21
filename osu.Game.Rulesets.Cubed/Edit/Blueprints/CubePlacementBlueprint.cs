using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Cubed.Objects;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public partial class CubePlacementBlueprint() : CubedPlacementBlueprint<Cube>(new Cube()) {
        [BackgroundDependencyLoader]
        private void load() =>
            Child = new CubePlacementPiece();

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (!base.OnMouseDown(e))
                return false;

            EndPlacement(true);
            return true;
        }
    }
}
