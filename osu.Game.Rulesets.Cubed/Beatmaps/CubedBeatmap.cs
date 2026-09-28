using osu.Game.Beatmaps;
using osu.Game.Localisation;
using osu.Game.Rulesets.Cubed.Objects;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Beatmaps {
    public class CubedBeatmap : Beatmap<CubedHitObject> {
        public override IEnumerable<BeatmapStatistic> GetStatistics() {
            int notes = HitObjects.Count(ho => ho is Cube);
            int holds = HitObjects.Count(ho => ho is CubedHoldNote);
            int sum = int.Max(1, notes + holds);

            return [
                new BeatmapStatistic {
                    Name = BeatmapStatisticStrings.Notes,
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Circles),
                    Content = notes.ToString(),
                    BarDisplayLength = notes / (float) sum,
                },
                new BeatmapStatistic {
                    Name = BeatmapStatisticStrings.HoldNotes,
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Sliders),
                    Content = holds.ToString(),
                    BarDisplayLength = holds / (float) sum,
                }
            ];
        }
    }
}
