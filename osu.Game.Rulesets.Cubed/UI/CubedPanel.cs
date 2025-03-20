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
using System.Diagnostics;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPanel : CompositeDrawable {
        public Drawable Background;
        public TrianglesV2 Triangles;
        public BindableBool SmallTriangles = new();
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
                    ScaleAdjust = 1.42f,
                    SpawnRatio = 3.69f,
                    // Pink3 Contrasts so good but it's been called distracting
                    Colour = colors.Pink2
                },
                new GridContainer {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(10),
                    Content = initializeCells()
                }
            ];
            SmallTriangles.BindValueChanged(SizeClassChanged, true);
        }

        private CubedPanelCell[][] initializeCells() {
            for (int row = 0; row < 4; row++) {
                Cells[row] = new CubedPanelCell[4];
                for (int c = 0; c < 4; c++ /* C what I did here */) {
                    CubedPanelCell cell = new (CellCornerRadius);
                    CellCornerRadius.BindValueChanged(e => cell.CornerRadius = e.NewValue, true);
                    CellBorderThickness.BindValueChanged(e => cell.BorderThickness = e.NewValue, true);
                    AccentColor.BindValueChanged(e => cell.BorderColour = e.NewValue, true);
                    AccentColor.BindValueChanged(e => cell.ActiveColor = e.NewValue, true);
                    Cells[row][c] = cell;
                }
            }
            return Cells;
        }

        private void SizeClassChanged(ValueChangedEvent<bool> e) => Triangles.ScaleAdjust = e.NewValue ? 0.727f : 1.42f;

        // WARNING: This will cause a crash if the value is equal to or more than 2^16
        public void PressKeys(uint keyMask) {
            Debug.Assert(keyMask < 65536, "Cannot set keyMask above 2^16, as the panel is a 4x4 square");

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

        public readonly BindableFloat CellCornerRadius = new(7.27f) {
            MinValue = 0,
            MaxValue = 100,
            Precision = .1f
        };

        public readonly BindableFloat CellBorderThickness = new(4) {
            MinValue = 0,
            MaxValue = 5,
            Precision = .1f
        };

        public readonly BindableColour4 AccentColor = new (Colour4.White);

        public partial class CubedPanelCell(BindableFloat cellCornerRadius = null) : Container {
            public readonly BindableBool Active = new();
            // LMAO Rider's spellcheck thinks it's read "I dle Color"
            public Color4 IdleColor = Color4.Transparent;
            public Color4 ActiveColor = Color4.White;

            private readonly Box box = new() { RelativeSizeAxes = Axes.Both };

            [BackgroundDependencyLoader]
            private void load() {
                RelativeSizeAxes = Axes.Both;
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                Scale = new Vector2(.9f);

                Masking = true;
                // Border/corner settings are handled using bindables

                InternalChild = box;
                Active.BindValueChanged(e => box.Colour = e.NewValue ? ActiveColor : IdleColor, true);
            }

            protected override bool OnClick(ClickEvent e) {
                Active.Toggle();
                return true;
            }

            protected override void Update() {
                if (cellCornerRadius != null)
                    cellCornerRadius.MaxValue = DrawHeight * .5f;
            }
        }
    }
}
