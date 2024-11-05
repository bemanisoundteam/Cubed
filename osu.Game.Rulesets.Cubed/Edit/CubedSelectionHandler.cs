using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using System.Linq;

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

            deltaCol = System.Math.Clamp(deltaCol,
                -SelectedItems.Min(obj => ((CubedHitObject) obj).Column),
                3 - SelectedItems.Max(obj => ((CubedHitObject) obj).Column));
            deltaRow = System.Math.Clamp(deltaRow,
                -SelectedItems.Min(obj => ((CubedHitObject) obj).Row),
                3 - SelectedItems.Max(obj => ((CubedHitObject) obj).Row));
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
