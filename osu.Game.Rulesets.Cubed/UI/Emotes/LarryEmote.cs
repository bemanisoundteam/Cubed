using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class LarryEmote : CubedEmote {
        // ⬜🟥🟥🟥
        // 🟥🟥⬜⬜
        // 🟥🟥🟥🟥
        // ⬜🟥⬜🟥
        public const uint Trigger = 0b1010111100111110;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, ISampleStore samples) {
            AddInternal(Emote = new Sprite { Texture = textures.Get("Emotes/Larry") });
            AddInternal(Sample = new DrawableSample(samples.Get("Emotes/Larry")));
        }
    }
}
