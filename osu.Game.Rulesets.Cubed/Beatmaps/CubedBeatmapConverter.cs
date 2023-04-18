using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace osu.Game.Rulesets.Cubed.Beatmaps {
    public class CubedBeatmapConverter : BeatmapConverter<CubedHitObject> {
        public CubedBeatmapConverter(IBeatmap beatmap, Ruleset ruleset) : base(beatmap, ruleset) {}

        public override bool CanConvert() => Beatmap.HitObjects.Any(o => o is IHasPosition);

        protected override IEnumerable<CubedHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken) {
            if (original is not IHasPosition) yield break;

            yield return new CubedHitObject {
                Samples = original.Samples,
                StartTime = original.StartTime,
            };
        }
    }
}
