using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Cubed.Tests {
    [TestFixture]
    public partial class TestSceneScaleUpCube : TestScene {
        [SetUpSteps]
        public void SetupSteps() {
            AddStep("Create CubePiece", () => {
                    Child = new CubePiece() {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        FillMode = FillMode.Fit,
                        FillAspectRatio = 1,
                        Width = 0.20f
                    };
            });
            AddStep("Scaleup Cube", () => { Child.ScaleTo(1.5f, 1000, Easing.OutQuint); });
        }
    }
}
