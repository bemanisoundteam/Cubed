using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public abstract partial class CubedSpriteEmote : Sprite, ICubedEmote {
        public ISample Sample { get; protected set; }

        private SampleChannel channel;

        public void Fire(bool expire) {
            if (expire)
                channel = Sample?.Play();
            else
                Sample?.Play();
        }

        protected override void Update() {
            if (channel?.HasCompleted ?? false)
                Expire();
        }
    }
}
