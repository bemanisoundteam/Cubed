using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics.UserInterfaceV2;
using osuTK;
using System;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public abstract partial class CubedSkinSelectionPopover : OsuPopover {
        protected readonly FillFlowContainer Container = new FillFlowContainer {
            AutoSizeAxes = Axes.Both,
            Spacing = new Vector2(10),
        };

        protected CubedSkinSelectionPopover() {
            Child = Container;
        }

        protected override void Update() =>
            Container.MaximumSize = Parent!.DrawSize;

        protected partial class SkinElementCard : Container {
            public Colour4 BackgroundColor {
                get => bg.Colour;
                set => bg.Colour = value;
            }

            public bool IsSelected {
                set => BorderColour = value ? Colour4.LightGreen : Colour4.Gray;
            }

            public required Action OnSelection { get; init; }

            private readonly Container content = new Container {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(10)
            };

            protected override Container<Drawable> Content => content;

            private readonly Box bg = new Box { RelativeSizeAxes = Axes.Both };

            [BackgroundDependencyLoader]
            private void load() {
                Masking = true;

                InternalChildren = [bg, Content];
            }

            protected override bool OnClick(ClickEvent e) {
                OnSelection();
                return true;
            }
        }
    }
}
