using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Skinning;
using static osu.Game.Rulesets.Cubed.Skinning.CubedSkinComponents;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Tests.Components {
    public partial class TestSceneCellGlow : OsuTestScene {
        private readonly CubedSkinnableDrawable cellGlow = new(CellGlow) {
            FillMode = FillMode.Fit,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        };

        [BackgroundDependencyLoader]
        private void load() {
            Add(cellGlow);
            AddSliderStep("Size", 0f, 1f, .5f, s => cellGlow.Size = new Vector2(s));
        }
    }
}
