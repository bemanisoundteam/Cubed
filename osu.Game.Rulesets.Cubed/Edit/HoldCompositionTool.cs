using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Edit.Blueprints;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;

namespace osu.Game.Rulesets.Cubed.Edit {
    public class HoldCompositionTool() : CompositionTool<CubedAction>("Hold") {
        public override Drawable CreateIcon() => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Sliders);

        public override PlacementBlueprint CreatePlacementBlueprint() => new HoldPlacementBlueprint();
    }
}
