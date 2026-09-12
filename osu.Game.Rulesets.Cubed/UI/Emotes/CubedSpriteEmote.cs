using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public abstract partial class CubedSpriteEmote : Sprite, ICubedEmote {
        protected ISample Sample { get; set; }

        private SampleChannel channel;
        private bool shouldExpire;

        public void Fire(bool expire) {
            shouldExpire = expire;
            channel = Sample?.Play();
        }

        public bool IsPlaying => channel?.Playing ?? false;

        protected override void Update() {
            if (shouldExpire && (channel?.HasCompleted ?? false))
                Parent!.Expire();
        }
    }
}
