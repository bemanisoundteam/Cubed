using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces;
using osu.Game.Screens;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Cubed.Tests {
    [TestFixture]
    public partial class TestSceneScaleUpCube : ScreenTestScene {
        private CubePiece Cube;

        [SetUpSteps]
        public void SetupSteps() {
            AddStep("Setup screen", () => { LoadScreen(new CubeScaleTestScreen(Cube = new CubePiece())); });
            AddStep("Scaleup Cube", () => { Cube.ScaleTo(1.5f, 1000, Easing.OutQuint); });
        }

        private partial class CubeScaleTestScreen : OsuScreen {
            private CubePiece Cube;

            public CubeScaleTestScreen(CubePiece Cube) { this.Cube = Cube; }

            [BackgroundDependencyLoader]
            private void load() {
                Cube.Anchor = Anchor.Centre;
                Cube.Origin = Anchor.Centre;
                Cube.FillMode = FillMode.Fit;
                Cube.FillAspectRatio = 1;
                Cube.Width = 0.20f;

                AddInternal(Cube);
            }
        }
    }
}
