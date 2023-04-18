using osu.Framework.Input.StateChanges;
using osu.Game.Replays;
using osu.Game.Rulesets.Replays;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Replays {
    public class CubedFramedReplayInputHandler : FramedReplayInputHandler<CubedReplayFrame> {
        public CubedFramedReplayInputHandler(Replay replay) : base(replay) {}

        protected override bool IsImportant(CubedReplayFrame frame) => frame.Actions.Any();

        protected override void CollectReplayInputs(List<IInput> inputs) {
            inputs.Add(new ReplayState<CubedAction> {
                PressedActions = CurrentFrame?.Actions ?? new List<CubedAction>(),
            });
        }
    }
}
