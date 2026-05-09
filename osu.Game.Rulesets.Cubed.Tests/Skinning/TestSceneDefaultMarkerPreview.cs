using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Cubed.Configuration;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Tests.Skinning {
    public partial class TestSceneDefaultMarkerPreview : OsuTestScene {
        private MarkerSkinPreviewer Marker;

        [BackgroundDependencyLoader]
        private void load() {
            Add(Marker = new MarkerSkinPreviewer {
                Marker = new DefaultMarker(),

                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(.2f),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillMode = FillMode.Fit,
                FillAspectRatio = 1,
            });
            Add(new ClockDisplay(Marker.Clock));
        }

        private partial class ClockDisplay : CompositeDrawable {
            private readonly OsuSpriteText text = new ();
            private readonly IClock clock;

            public ClockDisplay(IClock clock) {
                this.clock = clock;
                InternalChild = text;
            }

            protected override void Update() => text.Text = $"Preview Clock: {clock.CurrentTime}";
        }
    }
}
