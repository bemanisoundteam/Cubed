using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public abstract partial class CubedEmote : CompositeDrawable {
        public Drawable Emote { get; protected set; }
        public DrawableSample Sample { get; protected set; }
        public SampleChannel Channel;

        [BackgroundDependencyLoader]
        private void load() {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void Update() {
            if (Channel is not null && Channel.HasCompleted)
                Expire();
        }
    }
}
