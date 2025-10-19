using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class LarryEmote : CubedSpriteEmote {
        // ⬜🟥🟥🟥
        // 🟥🟥⬜⬜
        // 🟥🟥🟥🟥
        // ⬜🟥⬜🟥
        public const uint Trigger = 0b1010111100111110;
        public const string Quote = "YEEEEEHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        public new const string Name = "Larry da bird";

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, ISampleStore samples) {
            Texture = textures.Get("Emotes/Larry");
            Sample = samples.Get("Emotes/Larry");
        }
    }
}
