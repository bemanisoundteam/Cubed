using osu.Game.Rulesets.Cubed.Edit.Blueprints;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Edit {
    public partial class CubedHitObjectComposer(Ruleset ruleset) : HitObjectComposer<CubedHitObject>(ruleset) {
        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => [];

        protected override ComposeBlueprintContainer CreateBlueprintContainer() => new CubedBlueprintContainer(this);
    }

    public partial class CubedBlueprintContainer(HitObjectComposer composer) : ComposeBlueprintContainer(composer) {
        public override HitObjectSelectionBlueprint CreateHitObjectBlueprintFor(HitObject hitObject) =>
            new CubedSelectionBlueprint((CubedHitObject) hitObject);
    }
}
