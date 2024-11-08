using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Replays;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Replays {
    // Heavily inspired by ManiaAutoGenerator
    public class CubedAutoGenerator(IBeatmap beatmap) : AutoGenerator<CubedReplayFrame>(beatmap) {
        protected override void GenerateFrames() {
            var groupedActions = generateActions().GroupBy(a => a.Time);
            List<CubedAction> actions = [];

            foreach (var actionGroup in groupedActions) {
                foreach (Action action in actionGroup) {
                    if (action.Pressed)
                        actions.Add(action.CellAction);
                    else
                        actions.Remove(action.CellAction);
                }
                Frames.Add(new CubedReplayFrame(actionGroup.First().Time, actions.ToArray()));
            }
        }

        private IEnumerable<Action> generateActions() {
            for (int i = 0; i < Beatmap.HitObjects.Count; i++) {
                CubedHitObject hitObject = (CubedHitObject) Beatmap.HitObjects[i];

                yield return new Action(hitObject.StartTime, hitObject.Action, true);
                yield return new Action(ComputeReleaseTime(hitObject, GetFollowingObject(i)), hitObject.Action, false);
            }
        }

        private HitObject GetFollowingObject(int i) {
            CubedAction targetCell = ((CubedHitObject) Beatmap.HitObjects[i]).Action;

            for (i++; i < Beatmap.HitObjects.Count; i++)
                if (((CubedHitObject) Beatmap.HitObjects[i]).Action == targetCell)
                    return Beatmap.HitObjects[i];

            return null;
        }

        private static double ComputeReleaseTime(HitObject hitObject, HitObject nextObject) {
            double endTime = hitObject.GetEndTime();
            double offset = KEY_UP_DELAY;

            if (nextObject != null && nextObject.StartTime < endTime + offset)
                offset = (nextObject.StartTime - endTime) * .8;
            return endTime + offset;
        }

        private record Action(double Time, CubedAction CellAction, bool Pressed);
    }
}
