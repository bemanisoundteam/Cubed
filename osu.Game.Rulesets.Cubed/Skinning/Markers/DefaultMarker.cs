using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Cubed.Skinning.Markers {
    public partial class DefaultMarker : CompositeDrawable, IMarker {
        public osuTK.Vector2 PreviewScale => new(1f / ApproachScale.X, 1f / ApproachScale.Y);

        private static readonly osuTK.Vector2 ApproachScale = new(2.5f);

        private readonly Drawable approach = new Approach();

        private readonly Drawable marker = new Box {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        };

        [Resolved]
        private OsuColour colour { get; set; }

        [BackgroundDependencyLoader]
        private void load() =>
            InternalChildren = [marker, approach];

        public void AnimateApproach(double time) => approach.ScaleTo(1, time).Then().FadeOut();

        public void AnimateHit(double duration, JudgementResult judgement) {
            approach.FadeOut();
            marker.FadeColour(colour.ForHitResult(judgement.Type)).ScaleTo(1.5f, duration, Easing.OutQuint);
        }

        private partial class Approach : CompositeDrawable {
            [BackgroundDependencyLoader]
            private void iWishThereWasABetterWayToDoThis() {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.Both;
                Scale = ApproachScale;
                Masking = true;
                BorderColour = Colour4.Red;
                BorderThickness = 3;

                InternalChild = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Transparent
                };
            }
        }
    }
}
