using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class CubedEmotesContainer : Container {
        [BackgroundDependencyLoader]
        private void load() {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public void Fire(CubedEmote emote) {
            AddInternal(emote);
            emote.Size = new Vector2(.40f);
            // Enforce this, as sprites set their size according to their texture
            emote.Emote.RelativeSizeAxes = Axes.Both;
            emote.Emote.Size = Vector2.One;
            emote.Channel = emote.Sample.Play();
        }
    }
}
