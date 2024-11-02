using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Edit {
    public partial class CubedHitObjectComposer(Ruleset ruleset) : HitObjectComposer<CubedHitObject>(ruleset) {
        protected override IReadOnlyList<CompositionTool> CompositionTools => [
            new CubeCompositionTool(),
        ];

        protected override ComposeBlueprintContainer CreateBlueprintContainer() => new CubedBlueprintContainer(this);

        protected override Playfield PlayfieldAtScreenSpacePosition(Vector2 screenSpacePosition) =>
            (DrawableRuleset.Playfield as CubedPlayfield)!.GetCell(screenSpacePosition);
    }
}
