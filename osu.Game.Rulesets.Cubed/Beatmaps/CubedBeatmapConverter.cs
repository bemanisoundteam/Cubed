// Use this to remove non-hold notes and zero length holds
// This will also remove the minimum size for holds
// #define KEEP_ONLY_NONZERO_HOLDS

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osuTK;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace osu.Game.Rulesets.Cubed.Beatmaps {
    public class CubedBeatmapConverter(IBeatmap beatmap, Ruleset ruleset) : BeatmapConverter<CubedHitObject>(beatmap, ruleset) {
        public override bool CanConvert() => Beatmap.HitObjects.Any(o => o is IHasPosition);

        protected override IEnumerable<CubedHitObject> ConvertHitObject(HitObject hitObject, IBeatmap beatmap, CancellationToken cancellationToken) {
            if (hitObject is not IHasPosition objPos) yield break;

            // All the following is assuming objects are in osu!pixel space

            int Column = int.Min((int) (objPos.X / 128), 3);
            int Row = int.Min((int) (objPos.Y / 96), 3);

            if (hitObject is IHasDuration objDur) {
                float length = 2;
                HoldDirection direction = Column >= Row
                        ? Column > 1 ? HoldDirection.Left : HoldDirection.Right
                        : Row > 1 ? HoldDirection.Up : HoldDirection.Down;

                if (hitObject is IHasDistance objDist)
                    length = (float) objDist.Distance / 128f;
                if (hitObject is IHasPath objPath) {
                    Vector2 firstPoint = objPath.Path.PositionAt(0);
                    Vector2 secondPoint = objPath.Path.PositionAt(1);
                    Vector2 deltaPoint = secondPoint - firstPoint;

                    bool isHorizontal = float.Abs(deltaPoint.X) >= float.Abs(deltaPoint.Y);
                    if (isHorizontal)
                        direction = deltaPoint.X >= 0 ? HoldDirection.Right : HoldDirection.Left;
                    else
                        direction = deltaPoint.Y >= 0 ? HoldDirection.Down : HoldDirection.Up;

                    // Old implementation, sliders subjectively sometimes feel too short
                    // length = float.Max(float.Abs(deltaPoint.X), float.Abs(deltaPoint.Y)) / (isHorizontal ? 128 : 96);

                    // New implementation, tries to break this impression by using distance instead
                    // Not really much more effective, but the difference sometimes can be felt
                    length = Vector2.Distance(firstPoint, secondPoint) / (isHorizontal ? 128 : 96);

                    // TODO, this feels really weird, average slider being either too short or too long
                    // Multiply length to make it less boring, might need to use a non-linear function
                    length *= 2f;

                    #if !KEEP_ONLY_NONZERO_HOLDS
                    length = float.Max(1, length);
                    #endif

                    // Check that the object isn't facing the wall, flip direction if it does
                    // Ugly, but I'm tired and it's straightforward
                    if (direction == HoldDirection.Left && Column == 0)
                        direction = HoldDirection.Right;
                    if (direction == HoldDirection.Right && Column == 3)
                        direction = HoldDirection.Left;
                    if (direction == HoldDirection.Up && Row == 0)
                        direction = HoldDirection.Down;
                    if (direction == HoldDirection.Down && Row == 3)
                        direction = HoldDirection.Up;

                }

                #if KEEP_ONLY_NONZERO_HOLDS
                if (length >= 1)
                #endif
                // Make sure nothing replaces this gap, or goes between it, else the if breaks
                yield return new CubedHoldNote {
                    Samples = hitObject.Samples,
                    StartTime = hitObject.StartTime,
                    EndTime = objDur.EndTime,
                    Column = Column,
                    Row = Row,
                    Direction = direction,
                    // Hold note maxes hold lengths by itself
                    TailLength = (int) length
                };
            }

            else
            #if !KEEP_ONLY_NONZERO_HOLDS
                yield return new Cube {
                    Samples = hitObject.Samples,
                    StartTime = hitObject.StartTime,
                    Column = Column,
                    Row = Row
                };
            #else
                ;
            #endif
        }
    }
}
