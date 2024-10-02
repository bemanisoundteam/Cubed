using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System;

namespace osu.Game.Rulesets.Cubed.Beatmaps {
    public class CubedBeatmapConverter : BeatmapConverter<CubedHitObject> {
        public CubedBeatmapConverter(IBeatmap beatmap, Ruleset ruleset) : base(beatmap, ruleset) {}

        public override bool CanConvert() => Beatmap.HitObjects.Any(o => o is IHasPosition);

        protected override IEnumerable<CubedHitObject> ConvertHitObject(HitObject hitObject, IBeatmap beatmap, CancellationToken cancellationToken) {
            if (hitObject is not IHasPosition objPos) yield break;

            yield return new Cube {
                Samples = hitObject.Samples,
                StartTime = hitObject.StartTime,
                Column = Math.Min((int) (objPos.X / 128), 3),
                Row = Math.Min((int) (objPos.Y / 96), 3)
            };
        }
    }
}
