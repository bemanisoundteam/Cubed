using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Cubed.Objects;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public partial class CubeSelectionBlueprint(Cube cube) : CubedSelectionBlueprint(cube) {
        protected override bool AlwaysShowWhenSelected => true;

        [BackgroundDependencyLoader]
        private void load() =>
            InternalChild = new CubeSelectionPiece();

        protected override void Update() {
            base.Update();

            Size = DrawableObject.DrawSize;
            Position = DrawableObject.ToSpaceOfOtherDrawable(DrawableObject.DrawPosition, Parent!);
        }
    }
}
