using osu.Framework.Allocation;
using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osuTK;
using System;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public partial class HoldSelectionBlueprint(CubedHoldNote hold) : CubedSelectionBlueprint(hold) {
        public override Quad SelectionQuad =>
            RectangleF.Union(ScreenSpaceDrawQuad.AABBFloat, tailPiece.ScreenSpaceDrawQuad.AABBFloat);

        private HoldLinkSelectionPiece linkPiece;
        private CubeSelectionPiece tailPiece;

        [BackgroundDependencyLoader]
        private void load() =>
            InternalChildren = [
                linkPiece = new HoldLinkSelectionPiece(),
                tailPiece = new CubeSelectionPiece(),
                new CubeSelectionPiece()
            ];

        protected override void Update() {
            base.Update();

            linkPiece.Position = getUnitVector(hold.Direction) * .5f;
            linkPiece.Size = new Vector2(.1f, hold.TailLength / CubedPlayfield.CellScale - 1);
            linkPiece.Rotation = (int) hold.Direction * 90 - 180;

            tailPiece.Rotation = (int) hold.Direction * 90 - 180;
            tailPiece.Position = getUnitVector(hold.Direction) * hold.TailLength / CubedPlayfield.CellScale;
        }

        private static Vector2 getUnitVector(HoldDirection direction) =>
            direction switch {
                HoldDirection.Up => -Vector2.UnitY,
                HoldDirection.Down => Vector2.UnitY,
                HoldDirection.Left => -Vector2.UnitX,
                HoldDirection.Right => Vector2.UnitX,
                _ => throw new ArgumentOutOfRangeException(nameof(direction))
            };
    }
}
