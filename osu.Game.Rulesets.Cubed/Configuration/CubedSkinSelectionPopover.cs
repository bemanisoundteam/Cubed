using osu.Framework.Allocation;
using osu.Framework.Extensions;
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

        private RoundedButton closeButton;

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

            Container.Add(closeButton = new RoundedButton {
                Text = "Close",
                Action = this.HidePopover
            });
        }

        protected abstract IReadOnlyList<SkinElementCard> CreateItemCards();

        protected override void Update() {
            Container.MaximumSize = Parent!.DrawSize;
            scrollContainer.Height = float.Min(scrollContainer.ScrollContent.Height, Parent.DrawHeight - Content.Padding.TotalVertical);

            // We hide the button when there is only one skin selection option
            // It isn't even needed as there is largely enough space to click off the popover
            // RoundedButton seems to have some sort of minimum width mechanism
            // Which is causing the container to expand to it's max width
            // Which completely bypasses the point of having it auto size to begin with
            closeButton.Width = Container.Children.Count < 3 ? 0 : Container.Width;
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
