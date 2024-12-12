using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPanel : CompositeDrawable {
        public Drawable Background;
        public TrianglesV2 Triangles;
        public readonly CubedPanelCell[][] Cells = new CubedPanelCell[4][];

        [BackgroundDependencyLoader]
        private void load(OsuColour colors) {
            FillMode = FillMode.Fit;
            FillAspectRatio = 1;
            Masking = true;
            CornerRadius = 10;

            InternalChildren = [
                Background = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = colors.Pink1  // sadly not Pinky Crush, but logo pink
                    // But like seriously I'm stopping you for a minute here, have you seen HOW MUCH OF A BANGER IT IS ?
                },
                Triangles = new TrianglesV2 {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Thickness = 0.025f,
                    ScaleAdjust = 0.727f,
                    SpawnRatio = 3.69f,
                    Colour = colors.Pink3
                },
                new GridContainer {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(10),
                    Content = initializeCells()
                }
            ];
        }

        private CubedPanelCell[][] initializeCells() {
            for (int row = 0; row < 4; row++) {
                Cells[row] = new CubedPanelCell[4];
                for (int c = 0; c < 4; c++ /* C what I did here */)
                    Cells[row][c] = new CubedPanelCell();
            }
            return Cells;
        }

        // WARNING: This will cause a crash if the value is equal to or more than 2^16
        public void PressKeys(uint keyMask) {
            // That's an assembly nerd's trick
            for (; keyMask != 0; keyMask &= keyMask - 1) {
                int targetCell = System.Numerics.BitOperations.TrailingZeroCount(keyMask);
                Cells[targetCell / 4][targetCell % 4].Active.Value = true;
            }
        }

        public void Reset() {
            foreach (CubedPanelCell[] cells in Cells)
                foreach (CubedPanelCell cell in cells)
                    cell.Active.Value = false;
        }

        public partial class CubedPanelCell : CompositeDrawable {
            public readonly BindableBool Active = new();
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            // I actually want it to be transparent
            private Color4 idleColor;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
            private Color4 activeColor;

            [BackgroundDependencyLoader]
            private void load() {
                RelativeSizeAxes = Axes.Both;
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                Scale = new Vector2(.9f);

                Masking = true;
                CornerRadius = 7.27f;
                BorderThickness = 4;
                BorderColour = Color4.White;
                activeColor = Color4.White;

                InternalChild = new Box { RelativeSizeAxes = Axes.Both };
                Active.BindValueChanged(e => InternalChild.Colour = e.NewValue ? activeColor : idleColor, true);
            }

            protected override bool OnClick(ClickEvent e) {
                Active.Toggle();
                return true;
            }
        }
    }
}
