using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Cubed.Tests.Components {
    public partial class TestScenePointerPositionAwareCellGlow : OsuTestScene {
        [BackgroundDependencyLoader]
        private void load(IRenderer renderer, ShaderManager shaders) {
            // Hack: I load ConciergeIcon to initialise ConciergeIcon.WhitePixel
            // This deserves to be handled better, but it's good enough for now
            Add(new ConciergeIcon { Alpha = 0 });

            Dependencies.CacheAs<ShaderManager>(new CubedShaderManager(renderer, shaders));

            PointerPositionAwareCellGlow[][] cells = new PointerPositionAwareCellGlow[4][];
            for (int i = 0; i < 4; i++) {
                cells[i] = new PointerPositionAwareCellGlow[4];
                for (int j = 0; j < 4; j++)
                    cells[i][j] = new PointerPositionAwareCellGlow {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Scale = new (CubedPlayfield.CellScale)
                    };
            }
            Add(new GridContainer {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fit,
                FillAspectRatio = 1,

                Content = cells
            });
        }
    }
}
