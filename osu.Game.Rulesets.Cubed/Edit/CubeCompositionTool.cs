using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Edit.Blueprints;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;

namespace osu.Game.Rulesets.Cubed.Edit {
    public class CubeCompositionTool() : CompositionTool<CubedAction>(nameof(Cube)) {
        public override Drawable CreateIcon() => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Circles);

        public override PlacementBlueprint CreatePlacementBlueprint() => new CubePlacementBlueprint();
    }
}
