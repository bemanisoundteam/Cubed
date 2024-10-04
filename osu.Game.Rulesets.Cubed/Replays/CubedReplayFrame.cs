using osu.Game.Rulesets.Replays;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Replays {
    public class CubedReplayFrame : ReplayFrame {
        public List<CubedAction> Actions = new();

        public CubedReplayFrame(double time, params CubedAction [] buttons) : base(time) { Actions.AddRange(buttons); }
    }
}
