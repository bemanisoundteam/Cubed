using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;

namespace osu.Game.Rulesets.Cubed.Edit {
    public partial class CubedSelectionHandler : EditorSelectionHandler {
        private CubedPlayfield playfield;

        [BackgroundDependencyLoader]
        private void load(HitObjectComposer composer) =>
            playfield = (CubedPlayfield) composer.Playfield;

        public override bool HandleMovement(MoveSelectionEvent<HitObject> moveEvent) {
            CubedCell cell = playfield.GetCell(moveEvent.Blueprint.ScreenSpaceSelectionPoint + moveEvent.ScreenSpaceDelta);
            if (cell == null)
                return false;

            int deltaCol = cell.Column - ((CubedHitObject) moveEvent.Blueprint.Item).Column;
            int deltaRow = cell.Row - ((CubedHitObject) moveEvent.Blueprint.Item).Row;
            if (deltaCol == 0 && deltaRow == 0)
                return false;

            int minCol = int.MaxValue;
            int minRow = int.MaxValue;
            int maxCol = int.MinValue;
            int maxRow = int.MinValue;

            foreach (CubedHitObject hitObject in SelectedItems) {
                if (hitObject.Column < minCol)
                    minCol = hitObject.Column;
                if (hitObject.Column > maxCol)
                    maxCol = hitObject.Column;
                if (hitObject.Row < minRow)
                    minRow = hitObject.Row;
                if (hitObject.Row > maxRow)
                    maxRow = hitObject.Row;
            }

            deltaCol = System.Math.Clamp(deltaCol, -minCol, 3 - maxCol);
            deltaRow = System.Math.Clamp(deltaRow, -minRow, 3 - maxRow);
            if (deltaCol == 0 && deltaRow == 0)
                return false;

            foreach (CubedHitObject hitObject in SelectedItems) {
                playfield.Remove(hitObject);
                hitObject.Column += deltaCol;
                hitObject.Row += deltaRow;
                playfield.Add(hitObject);
            }

            return true;
        }
    }
}
