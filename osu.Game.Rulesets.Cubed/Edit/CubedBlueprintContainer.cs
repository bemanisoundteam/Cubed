using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Edit.Blueprints;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Edit {
    public partial class CubedBlueprintContainer(HitObjectComposer composer) : ComposeBlueprintContainer(composer) {
        public override HitObjectSelectionBlueprint CreateHitObjectBlueprintFor(HitObject hitObject) => hitObject switch {
            Cube cube => new CubeSelectionBlueprint(cube),
            _ => base.CreateHitObjectBlueprintFor(hitObject)
        };

        protected override SelectionHandler<HitObject> CreateSelectionHandler() => new CubedSelectionHandler();

        // Copy pasted that, because I have no idea why did they make that breaking change, so I'll leave it at this
        // At least it works like it used to, but they AGAIN broke the editor
        protected override bool TryMoveBlueprints(DragEvent e, IList<(SelectionBlueprint<HitObject> blueprint, Vector2[] originalSnapPositions)> blueprints) {
            Vector2 distanceTravelled = e.ScreenSpaceMousePosition - e.ScreenSpaceMouseDownPosition;
            SelectionBlueprint<HitObject> referenceBlueprint = blueprints.First().blueprint;
            Vector2 movePosition = blueprints.First().originalSnapPositions.First() + distanceTravelled;

            return SelectionHandler.HandleMovement(new MoveSelectionEvent<HitObject>(referenceBlueprint,
                movePosition - referenceBlueprint.ScreenSpaceSelectionPoint));
        }
    }
}
