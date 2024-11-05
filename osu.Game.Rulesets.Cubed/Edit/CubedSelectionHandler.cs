using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Edit {
    public partial class CubedSelectionHandler : EditorSelectionHandler {
        private CubedPlayfield playfield;

        [BackgroundDependencyLoader]
        private void load(HitObjectComposer composer) =>
            playfield = (CubedPlayfield) composer.Playfield;

        protected override void OnSelectionChanged() {
            SelectionBox.CanFlipX = canFlipX(SelectedItems);
            SelectionBox.CanFlipY = canFlipY(SelectedItems);
        }

        private static bool canFlipX(IReadOnlyList<HitObject> selected) => selected.Select(obj => ((CubedHitObject) obj).Column).Distinct().Count() > 1;
        private static bool canFlipY(IReadOnlyList<HitObject> selected) => selected.Select(obj => ((CubedHitObject) obj).Row).Distinct().Count() > 1;

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

        public override bool HandleFlip(Direction direction, bool flipOverOrigin) {
            if (SelectedItems.Count == 0)
                return false;

            switch (direction) {
                case Direction.Horizontal:
                    if (canFlipX(SelectedItems) == false)
                        return false;

                    int minCol = flipOverOrigin ? 0 : SelectedItems.Min(obj => ((CubedHitObject) obj).Column);
                    int maxCol = flipOverOrigin ? 3 : SelectedItems.Max(obj => ((CubedHitObject) obj).Column);

                    foreach (CubedHitObject hitObject in SelectedItems) {
                        playfield.Remove(hitObject);
                        hitObject.Column = minCol + (maxCol - hitObject.Column);
                        playfield.Add(hitObject);
                    }

                    break;

                case Direction.Vertical:
                    if (canFlipY(SelectedItems) == false)
                        return false;

                    int minRow = flipOverOrigin ? 0 : SelectedItems.Min(obj => ((CubedHitObject) obj).Row);
                    int maxRow = flipOverOrigin ? 3 : SelectedItems.Max(obj => ((CubedHitObject) obj).Row);

                    foreach (CubedHitObject hitObject in SelectedItems) {
                        playfield.Remove(hitObject);
                        hitObject.Row = minRow + (maxRow - hitObject.Row);
                        playfield.Add(hitObject);
                    }

                    break;
            }

            return true;
        }
    }
}
