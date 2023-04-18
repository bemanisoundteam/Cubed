using osu.Game.Rulesets.Replays;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Replays {
    public class CubedReplayFrame : ReplayFrame {
        public List<CubedAction> Actions = new();

        public CubedReplayFrame(CubedAction? button = null) {
            if (button.HasValue)
                Actions.Add(button.Value);
        }
    }
}
