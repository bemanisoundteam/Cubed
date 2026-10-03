using osu.Game.Rulesets.Replays;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Replays {
    public class CubedReplayFrame : ReplayFrame {
        public readonly List<CubedAction> Actions = new();

        public CubedReplayFrame(double time, params CubedAction [] buttons) : base(time) { Actions.AddRange(buttons); }

        public override bool IsEquivalentTo(ReplayFrame other) =>
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            other is CubedReplayFrame frame && frame.Time == Time && frame.Actions.SequenceEqual(Actions);
    }
}
