using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Overlays;
using osuTK;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public abstract partial class CubedSkinSelectionPopover : OsuPopover {
        private readonly FillFlowContainer Container = new FillFlowContainer {
            AutoSizeAxes = Axes.Both,
            Spacing = new Vector2(10),
        };

        private OsuScrollContainer scrollContainer;

        [BackgroundDependencyLoader]
        private void load() {
            Child = scrollContainer = new OsuScrollContainer(Direction.Vertical) {
                // AutoSize calculations are messed up on the scroll direction, so I compute Height in Update()
                AutoSizeAxes = Axes.X,
                Child = Container
            };
            scrollContainer.ScrollContent.RelativeSizeAxes = Axes.None;
            scrollContainer.ScrollContent.AutoSizeAxes = Axes.Both;

            // Either the scrollbar overlaps content and it looks ugly
            // Or you set scrollContainer.ScrollbarOverlapsContent = false;
            // But they messed up padding so it looks ugly still
            scrollContainer.ScrollbarVisible = false;

            Container.Children = CreateItemCards();
        }

        protected abstract IReadOnlyList<SkinElementCard> CreateItemCards();

        protected override void Update() {
            Container.MaximumSize = Parent!.DrawSize;
            scrollContainer.Height = float.Min(scrollContainer.ScrollContent.Height, Parent.DrawHeight - Content.Padding.TotalVertical);
        }

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
            private void load(OverlayColourProvider colors) {
                Size = new Vector2(100);
                CornerRadius = 10;
                BorderThickness = 3;
                BackgroundColor = colors.Background3;

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
