using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;
using osuTK;
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

    public partial class CubedMusicBar : Drawable {
        const int ColumnCount = 120;
        public readonly int[] ColumnHeights = new int[ColumnCount];
        public readonly BarState[] BarStates = new BarState[ColumnCount];
        public readonly int HighestColumn;

        private IShader Shader;

        public CubedMusicBar(IReadOnlyList<HitEvent> hitEvents, IBeatmap beatmap) {
            double startTime = beatmap.HitObjects[0].StartTime;
            double endTime = beatmap.HitObjects[^1].StartTime;

            // In case this file is causing an IndexOutOfRangeException, read the following :
            // I know that adding 1 is a hack, but I haven't experienced crashes using it
            // But using double.Epsilon causes it to crash
            double timePerColumn = (endTime - startTime + 1) / ColumnCount;
            foreach (HitObject hitObject in beatmap.HitObjects) {
                int index = (int) ((hitObject.StartTime - startTime) / timePerColumn);
                ColumnHeights[index]++;  // No clamping
            }

            HighestColumn = ColumnHeights.Max();

            foreach (HitEvent hitEvent in hitEvents) {
                int index = (int) ((hitEvent.HitObject.StartTime - startTime) / timePerColumn);
                if (hitEvent.Result == HitResult.Miss)
                    BarStates[index] = BarState.Missed;
                else if (hitEvent.Result != HitResult.Perfect && BarStates[index] != BarState.Missed)
                    BarStates[index] = BarState.Missless;
            }
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders) =>
            Shader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);

        // Stolen from Tzwcard
        private static readonly Colour4 PerfectColor = new(255, 212, 39, 255);
        private static readonly Colour4 MisslessColor = new(41, 187, 229, 255);
        private static readonly Colour4 MissedColor = new(134, 130, 132, 255);

        private static Colour4 getBarColor(BarState barState) => barState switch {
            BarState.Perfect => PerfectColor,
            BarState.Missless => MisslessColor,
            BarState.Missed => MissedColor,
            // IDK Why C# makes me write this, as all enum values are already accounted for
            // Microsoft moment I guess
            _ => throw new ArgumentOutOfRangeException(nameof(barState), barState, null)
        };

        protected override DrawNode CreateDrawNode() => new MusicBarDrawNode(this);

        protected override void Update() =>
            Height = (DrawWidth / ColumnCount) * HighestColumn;

        private class MusicBarDrawNode(CubedMusicBar source) : DrawNode(source) {
            private IShader shader;
            private Vector2 cellSize;

            // Could optimise further by bring vertex count down from 4 * ColumnCount to a few, but it's currently not a bottleneck
            private readonly Quad[] bars = new Quad[ColumnCount];

            public override void ApplyState() {
                base.ApplyState();

                shader = source.Shader;
                cellSize = new Vector2(source.DrawWidth / ColumnCount, source.DrawHeight / source.HighestColumn);

                for (int i = 0; i < ColumnCount; i++) {
                    float height = cellSize.Y * source.ColumnHeights[i];

                    Vector2 topLeft = new(cellSize.X * i, source.DrawHeight - height);
                    Vector2 topRight = new(cellSize.X * (i + 1), source.DrawHeight - height);
                    Vector2 bottomLeft = new(cellSize.X * i, source.DrawHeight);
                    Vector2 bottomRight = new(cellSize.X * (i + 1), source.DrawHeight);
                    bars[i] = new Quad(topLeft, topRight, bottomLeft, bottomRight) * DrawInfo.Matrix;
                }
            }

            protected override bool CanDrawOpaqueInterior => DrawColourInfo.Colour.TopLeft.Alpha >= 1;

            protected override void Draw(IRenderer renderer) {
                base.Draw(renderer);

                DrawBars(renderer);
            }

            protected override void DrawOpaqueInterior(IRenderer renderer) {
                base.DrawOpaqueInterior(renderer);

                DrawBars(renderer);
            }

            private void DrawBars(IRenderer renderer) {
                shader.Bind();

                for (int i = 0; i < ColumnCount; i++) {
                    renderer.DrawQuad(
                        renderer.WhitePixel,
                        bars[i],
                        // This is ok, as bar states are readonly in the source
                        getBarColor(source.BarStates[i])
                            .MultiplyAlpha(DrawColourInfo.Colour.TopLeft.Alpha)
                    );
                }

                shader.Unbind();
            }
        }
    }

    public enum BarState {
        Perfect,
        Missless,
        Missed
    }
}
