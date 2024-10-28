using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osuTK;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Replays {
    public partial class CubedReplayRecorder(Score target) : ReplayRecorder<CubedAction>(target) {
        protected override ReplayFrame HandleFrame(Vector2 mousePosition, List<CubedAction> actions, ReplayFrame previousFrame) =>
            new CubedReplayFrame(Time.Current, actions.ToArray());
    }
}
