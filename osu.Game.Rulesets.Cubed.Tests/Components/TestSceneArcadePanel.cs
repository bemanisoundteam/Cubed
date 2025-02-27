using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Tests.Components {
    public partial class TestSceneArcadePanel : OsuTestScene {
        private CubedPanel Panel;

        [BackgroundDependencyLoader]
        private void load(OsuColour colors) {
            Child = Panel = new CubedPanel {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            AddStep("Reset panel", Panel.Reset);
            AddSliderStep("Panel Size Multiplier", 0f, 1f, 1f, e => Panel.Size = new Vector2(float.Min(Size.X, Size.Y) * e));
            AddToggleStep("Small Triangles", e => Panel.SmallTriangles.Value = e);
            AddSliderStep("Triangle Spawn Ratio", 0f, 5f, Panel.Triangles.SpawnRatio, e => Panel.Triangles.SpawnRatio = e);
            AddSliderStep("Triangle Scale Adjust", 0f, 4f, Panel.Triangles.ScaleAdjust, e => Panel.Triangles.ScaleAdjust = e);
            AddStep("Logo pink", () => Panel.Background.Colour = colors.Pink1);
            AddStep("Alternative color I considered", () => Panel.Background.Colour = colors.Pink3);
            AddStep("Main menu exit button color (arbitrary color ?)", () => Panel.Background.Colour = new osuTK.Graphics.Color4(238, 51, 153, 255));
        }
    }
}
