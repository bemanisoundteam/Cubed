using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Edit.Blueprints;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Edit {
    public partial class CubedBlueprintContainer(HitObjectComposer composer) : ComposeBlueprintContainer(composer) {
        public override HitObjectSelectionBlueprint CreateHitObjectBlueprintFor(HitObject hitObject) => hitObject switch {
            Cube cube => new CubeSelectionBlueprint(cube),
            _ => base.CreateHitObjectBlueprintFor(hitObject)
        };

        protected override SelectionHandler<HitObject> CreateSelectionHandler() => new CubedSelectionHandler();

        // This is terrible design from upstream, it's full of unclear elements and pitfalls :
        // - Blueprints are sorted by StartTime, and paired with the blueprint's ScreenSpaceSnapPoints as of drag start
        // - ScreenSpaceSelectionPoint is the center of the DHO :
        //   Whilst originally meant for drag selection (wtf?), it's abused as the first element of the SnapPoints array
        // - You cannot determine which object was dragged, so you cannot make move delta relative to drag origin
        // - You can't even pass drag origin in the MoveSelectionEvent... It's inferred from the reference blueprint...
        protected override bool TryMoveBlueprints(DragEvent e, IList<(SelectionBlueprint<HitObject> blueprint, Vector2[] originalSnapPositions)> blueprints) =>
            // Handler uses ScreenSpaceSelectionPoint as a substitute drag origin, but it moves alongside the blueprint
            SelectionHandler.HandleMovement(
                new MoveSelectionEvent<HitObject>(
                    blueprints[0].blueprint,
                    // Compensate for the blueprint's ScreenSpaceSelectionPoint having potentially moved
                    blueprints[0].originalSnapPositions[0] - blueprints[0].blueprint.ScreenSpaceSelectionPoint
                    + e.ScreenSpaceMousePosition - e.ScreenSpaceMouseDownPosition
                )
            );
    }
}
