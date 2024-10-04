using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Cubed.Replays {
    public class CubedAutoGenerator : AutoGenerator<CubedReplayFrame> {
        public CubedAutoGenerator(IBeatmap beatmap) : base(beatmap) {}

        protected override void GenerateFrames() {
            foreach (CubedHitObject hitObject in Beatmap.HitObjects) {
                Frames.Add(new CubedReplayFrame(hitObject.StartTime, hitObject.Action));
                Frames.Add(new CubedReplayFrame(hitObject.GetEndTime() + KEY_UP_DELAY));
            }
        }
    }
}
