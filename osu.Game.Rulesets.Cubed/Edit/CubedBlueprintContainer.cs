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

        protected override IEnumerable<SelectionBlueprint<HitObject>> SortForMovement(IReadOnlyList<SelectionBlueprint<HitObject>> blueprints) =>
            base.SortForMovement(blueprints).OrderByDescending(b => b.IsHovered);

        // This is terrible design from upstream, it's full of unclear elements and pitfalls :
        // - Blueprints are sorted and paired with the blueprint's ScreenSpaceSnapPoints as of drag start
        //   We sort blueprints ourselves so that the first blueprint is the earliest hovered blueprint
        // - ScreenSpaceSelectionPoint is the center of the DHO :
        //   Whilst originally meant for drag selection (wtf?), it's abused as originalSnapPositions's first element
        // - You can't even pass drag origin in the MoveSelectionEvent... It's inferred from the reference blueprint...
        protected override bool TryMoveBlueprints(DragEvent e, IList<(SelectionBlueprint<HitObject> blueprint, Vector2[] originalSnapPositions)> blueprints) =>
            // Handler uses ScreenSpaceSelectionPoint as a substitute drag origin, but it moves alongside the blueprint
            SelectionHandler.HandleMovement(
                new MoveSelectionEvent<HitObject>(
                    blueprints[0].blueprint,
                    // Compensate for the blueprint's ScreenSpaceSelectionPoint having potentially moved
                    blueprints[0].originalSnapPositions[0] - blueprints[0].blueprint.ScreenSpaceSelectionPoint
                    // And add offset to drag origin, so that selection moves alongside the pointer
                    + e.ScreenSpaceMouseDownPosition - blueprints[0].originalSnapPositions[0]
                    + e.ScreenSpaceMousePosition - e.ScreenSpaceMouseDownPosition
                )
            );
    }
}
