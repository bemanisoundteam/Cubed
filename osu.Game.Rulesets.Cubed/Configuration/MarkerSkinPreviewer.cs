using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Timing;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class MarkerSkinPreviewer : Container {
        public readonly BindableBool ScaleDownToFit = new(true);

        private readonly Cube hitObject = new ();
        private double ApproachDuration => hitObject.TimePreempt;
        private double HitDuration => 1000;  // Exaggerated length so you have time to see

        public override bool RemoveCompletedTransforms => false;

        public MarkerSkinPreviewer() {
            ScaleDownToFit.ValueChanged += e => InternalChild.Scale = e.NewValue ? Marker.PreviewScale : Vector2.One;
        }

        public IMarker Marker {
            get => InternalChild as IMarker;
            set {
                InternalChild = (Drawable) value;
                InternalChild.RelativeSizeAxes = Axes.Both;
                InternalChild.Size = Vector2.One;
                InternalChild.Scale = ScaleDownToFit.Value ? Marker.PreviewScale : Vector2.One;
                InternalChild.Anchor = Anchor.Centre;
                InternalChild.Origin = Anchor.Centre;

                Scheduler.AddOnce(marker => {
                    using (BeginAbsoluteSequence(0))
                        marker.AnimateApproach(ApproachDuration);

                    using (BeginAbsoluteSequence(ApproachDuration))
                        marker.AnimateHit(HitDuration, new JudgementResult(hitObject, hitObject.Judgement) {
                            Type = HitResult.Perfect
                        });
                }, Marker);
            }
        }

        protected override void LoadComplete() =>
            Clock = new FramedClock(new LoopingClock(Parent!.Clock, ApproachDuration + HitDuration));

        private class LoopingClock(IClock parent, double period) : IClock {
            public double CurrentTime => parent.CurrentTime % period;
            public double Rate => parent.Rate;
            public bool IsRunning => parent.IsRunning;
        }
    }
}
