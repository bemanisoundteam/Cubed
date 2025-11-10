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
            var groupedActions = generateActions().GroupBy(a => a.Time)
                // WARNING: AS TEMPTING AS IT MIGHT SEEM, DO NOT REMOVE !!! (I lost way, way too much time on this)
                // Because a release might be scheduled after other presses (on different cubes),
                // and release events are emitted alongside their associated press but not within the same time group,
                // a new time group is created immediately after the press's time group during the grouping process.
                // This new group MIGHT (generally doesn't in most cases) belong to later in the chronological ordering
                // which would cause frames to be generated out of order if left unsorted.
                // The game doesn't handle this case well, causing weird behavior that's hard to debug.
                .OrderBy(g => g.Key);

            List<CubedAction> actions = [];

            // This would greatly benefit from fine-grained unit testing, as this hasn't been properly edge case tested
            // Testing was really just about FC-ing centipede and razor sharp, which whilst being great test maps
            // aren't extensive edge cases tests. I might even want to test if the right amount of frames is made
            foreach (IGrouping<double, Action> actionGroup in groupedActions) {
                // These two arrays are meant to handle actions cancelling out previous ones
                // Frames only have input state, not events, so pressing then immediately releasing is NOOP
                // This happens either when an object starts at the time of the previous object's end
                // Therefore to avoid that case, we use these arrays to insert intermediary frames if needed
                bool[] pressedThisFrame = new bool[16];
                bool[] releasedThisFrame = new bool[16];

                /* TODO This makes a single frame per "debounced" event, which is inefficient
                 * but acceptable as (as of late 2025 and since the beginning) you can't export Cubed replays
                 * expected behavior is to have the minimal amount of intermediary frames */
                foreach (Action action in actionGroup)
                    if (action.Pressed) {
                        // Emmit an intermediary frame to avoid cancelling out the release
                        if (releasedThisFrame[(int) action.CellAction])
                            Frames.Add(new CubedReplayFrame(action.Time, actions.ToArray()));

                        actions.Add(action.CellAction);
                        pressedThisFrame[(int) action.CellAction] = true;
                    }
                    else {
                        // Emmit an intermediary frame to avoid cancelling out the press
                        if (pressedThisFrame[(int) action.CellAction])
                            Frames.Add(new CubedReplayFrame(action.Time, actions.ToArray()));

                        actions.Remove(action.CellAction);
                        releasedThisFrame[(int) action.CellAction] = true;
                    }

                Frames.Add(new CubedReplayFrame(actionGroup.Key, actions.ToArray()));
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
