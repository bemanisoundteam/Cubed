using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Edit {
    public partial class CubedSelectionHandler : EditorSelectionHandler {
        private CubedPlayfield playfield;

        [BackgroundDependencyLoader]
        private void load(HitObjectComposer composer) =>
            playfield = (CubedPlayfield) composer.Playfield;

        protected override void OnSelectionChanged() {
            SelectionBox.CanFlipX = canFlipX(SelectedItems.Cast<CubedHitObject>());
            SelectionBox.CanFlipY = canFlipY(SelectedItems.Cast<CubedHitObject>());
        }

        private static bool canFlipX(IEnumerable<CubedHitObject> selection) =>
            selection.Select(obj => obj.Column).Distinct().Count() > 1;

        private static bool canFlipY(IEnumerable<CubedHitObject> selection) =>
            selection.Select(obj => obj.Row).Distinct().Count() > 1;

        public override bool HandleMovement(MoveSelectionEvent<HitObject> moveEvent) {
            // We do not know where did the move start from, best we can do is to abuse ScreenSpaceSelectionPoint
            CubedCell cell = playfield.GetCell(moveEvent.Blueprint.ScreenSpaceSelectionPoint + moveEvent.ScreenSpaceDelta);

            if (cell == null)
                return false;

            var referenceObject = (CubedHitObject) moveEvent.Blueprint.Item;
            CubedHitObject[] selectedObjects = [.. SelectedItems.Cast<CubedHitObject>()];

            int deltaCol = int.Clamp(cell.Column - referenceObject.Column,
                - selectedObjects.Min(obj => obj.Column),
                3 - selectedObjects.Max(obj => obj.Column)
            );
            int deltaRow = int.Clamp(cell.Row - referenceObject.Row,
                - selectedObjects.Min(obj => obj.Row),
                3 - selectedObjects.Max(obj => obj.Row)
            );

            if (deltaCol == 0 && deltaRow == 0)
                return false;

            updateSelection(selectedObjects, hitObject => {
                hitObject.Column += deltaCol;
                hitObject.Row += deltaRow;
            });

            return true;
        }

        public override bool HandleFlip(Direction direction, bool flipOverOrigin) {
            if (SelectedItems.Count == 0)
                return false;

            CubedHitObject[] selectedObjects = [.. SelectedItems.Cast<CubedHitObject>()];

            if (direction == Direction.Horizontal) {
                if (!canFlipX(selectedObjects))
                    return false;

                int minCol = flipOverOrigin ? 0 : selectedObjects.Min(obj => obj.Column);
                int maxCol = flipOverOrigin ? 3 : selectedObjects.Max(obj => obj.Column);

                updateSelection(selectedObjects, hitObject =>
                    hitObject.Column = minCol + (maxCol - hitObject.Column)
                );
            }
            else if (direction == Direction.Vertical) {
                if (!canFlipY(selectedObjects))
                    return false;

                int minRow = flipOverOrigin ? 0 : selectedObjects.Min(obj => obj.Row);
                int maxRow = flipOverOrigin ? 3 : selectedObjects.Max(obj => obj.Row);

                updateSelection(selectedObjects, hitObject =>
                    hitObject.Row = minRow + (maxRow - hitObject.Row)
                );
            }

            return true;
        }

        private void updateSelection(CubedHitObject[] selectedObjects, Action<CubedHitObject> updateObject) {
            foreach (CubedHitObject hitObject in selectedObjects) {
                playfield.Remove(hitObject);
                updateObject(hitObject);
                playfield.Add(hitObject);
            }
        }
    }
}
