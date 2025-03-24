using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;
using System.Collections.Generic;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI {
    // Code snippet from Meta-link

    /*  time_per_column = end_event_time / 120
     *  columns = [0] * 120
     *  for n in notes:
     *      index = math.floor(n / time_per_column)
     *      print(str(index) + " " + str(n))
     *      columns[index] = min(columns[index] + 1, 8)
     */

    public partial class CubedMusicBar : Container {
        const int ColumnCount = 120;
        private readonly int highestColumn;

        public CubedMusicBar(IReadOnlyList<HitEvent> hitEvents, IBeatmap beatmap) {
            int[] columns = new int[ColumnCount];
            BarState[] barStates = new BarState[ColumnCount];
            double startTime = beatmap.HitObjects[0].StartTime;
            double endTime = beatmap.HitObjects[^1].StartTime;

            // In case this file is causing an IndexOutOfRangeException, read the following :
            // I know that adding 1 is a hack, but I haven't experienced crashes using it
            // But using double.Epsilon causes it to crash
            double timePerColumn = (endTime - startTime + 1) / ColumnCount;
            foreach (HitObject hitObject in beatmap.HitObjects) {
                int index = (int) ((hitObject.StartTime - startTime) / timePerColumn);
                columns[index]++; // No clamping
            }

            foreach (HitEvent hitEvent in hitEvents) {
                int index = (int) ((hitEvent.HitObject.StartTime - startTime) / timePerColumn);
                if (hitEvent.Result == HitResult.Miss)
                    barStates[index] = BarState.Missed;
                else if (hitEvent.Result != HitResult.Perfect && barStates[index] != BarState.Missed)
                    barStates[index] = BarState.Missless;
            }

            // In case this feels claustrophobic, or if I want to stick to the original, uncomment the following :
            // highestColumn = Math.Max(columns.Max(), 8)
            highestColumn = columns.Max();
            // I swear GridContainer is so fucking retarded, I hate that fucking steaming pile of shit
            Box[][] boxes = new Box[highestColumn][];
            for (int y = 0; y < highestColumn; y++) {
                // Did I mention I fucking hate GridContainer ?
                boxes[y] = new Box[ColumnCount];
                for (int x = 0; x < ColumnCount; x++)
                    if (columns[x] >= highestColumn - y)
                        // SEE THAT MOTHERFUCKING SHIT IS FUCKING INVERTED
                        boxes[y][x] = new Box {
                            RelativeSizeAxes = Axes.Both,
                            Colour = getBarColor(barStates[x]),
                            FillMode = FillMode.Fit,
                            FillAspectRatio = 1
                        };
            }

            InternalChild = new GridContainer {
                RelativeSizeAxes = Axes.Both,
                Content = boxes
            };
        }

        // Stolen from Tzwcard
        private readonly Colour4 PerfectColor = new (255, 212, 39, 255);
        private readonly Colour4 MisslessColor = new (41, 187, 229, 255);
        private readonly Colour4 MissedColor = new (134, 130, 132, 255);

        private Colour4 getBarColor(BarState barState) => barState switch {
            BarState.Perfect => PerfectColor,
            BarState.Missless => MisslessColor,
            BarState.Missed => MissedColor,
            // IDK Why C# makes me write this, as all enum values are already accounted for
            // Microsoft moment I guess
            _ => throw new ArgumentOutOfRangeException(nameof(barState), barState, null)
        };

        protected override void Update() =>
            Height = (DrawWidth / ColumnCount) * highestColumn;
    }

    enum BarState {
        Perfect,
        Missless,
        Missed
    }
}
