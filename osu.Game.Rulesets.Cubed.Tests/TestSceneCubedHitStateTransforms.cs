using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Tests {
    [TestFixture]
    public partial class TestSceneCubedHitStateTransforms : TestScene {
        private DrawableCube drawableCube;

        [SetUpSteps]
        public void SetUpSteps() =>
            AddStep("Create Cube", () => {
                // Start time is at an arbitrary value
                Cube cube = new() { StartTime = Time.Current + 1000 };
                // This is required to make it work
                cube.ApplyDefaults(new ControlPointInfo(), new BeatmapDifficulty());
                Child = drawableCube = new DrawableCube(cube) {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    FillMode = FillMode.Fit,
                    FillAspectRatio = 1,
                    Width = 0.20f
                };
            });

        [Test]
        public void TestPerfect() {
            AddStep("Schedule perfect hit", () => Scheduler.AddDelayed(HitCube, drawableCube.HitObject.StartTime - Time.Current));
            AddUntilStep("Object is Judged", () => drawableCube.Judged);
            AddAssert("Result is Perfect", () => drawableCube.Result.Type == HitResult.Perfect);
            AddUntilStep("Object Expired", (() => drawableCube.LifetimeEnd < Time.Current));
            AddAssert("Object's scale is 1.5", () => drawableCube.Scale.Equals(new Vector2(1.5f)));
            AddAssert("Object is invisible", CubePracticallyInvisible);
        }

        [Test]
        public void TestMiss() {
            AddUntilStep("Object is Judged", () => drawableCube.Judged);
            AddAssert("Result is Miss", () => drawableCube.Result.Type == HitResult.Miss);
            AddAssert("Object is Red", () => drawableCube.Colour == osuTK.Graphics.Color4.Red);
            AddUntilStep("Object Expired", () => drawableCube.LifetimeEnd < Time.Current);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            AddAssert("Object's scale is 60%", () => drawableCube.Scale.X == drawableCube.Scale.Y && Precision.AlmostEquals(drawableCube.Scale.X, .6, .01));
            AddAssert("Object is invisible", CubePracticallyInvisible);
        }

        private void HitCube() => drawableCube.OnHit();

        private bool CubePracticallyInvisible() => !drawableCube.IsPresent ||//;  This is the check we're supposed to run
                                                   drawableCube.Alpha < .02f;  // This is a workaround around AN ANNOYING framework bug, this is to avoid spurious test failures, AND YES THE VALUE IS THAT STUPID
    }
}
