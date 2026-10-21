using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Cubed.Objects;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public partial class CubeSelectionBlueprint(Cube cube) : CubedSelectionBlueprint(cube) {
        [BackgroundDependencyLoader]
        private void load() =>
            InternalChild = new CubeSelectionPiece();
    }
}
