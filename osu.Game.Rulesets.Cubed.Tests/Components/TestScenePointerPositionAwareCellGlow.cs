using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Tests.Components {
    // This doesn't test the use case of CubedCell well, but still is useful
    public partial class TestScenePointerPositionAwareCellGlow : OsuTestScene {
        [Cached]
        private Bindable<Vector2> PointerPosition = new ();

        [BackgroundDependencyLoader]
        private void load(IRenderer renderer, ShaderManager shaders) {
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

        // It's unlikely users will use another input method in test runner
        // So it's fine to just handle mouse (for now ?)
        // May work for touch (although I didn't test it)
        // as InputManager.MapMouseToLatestTouch's default value is true
        protected override bool OnMouseMove(MouseMoveEvent e) {
            PointerPosition.Value = e.ScreenSpaceMousePosition;
            return false;
        }
    }
}
