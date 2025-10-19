using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class EGaddEmote : CubedSpriteEmote {
        // ⬜⬜⬜⬜
        // 🟥⬜⬜🟥
        // 🟥⬜⬜🟥
        // ⬜⬜⬜⬜
        public const uint Trigger = 0b0000100110010000;
        public new const string Name = "Professor E. Gadd";

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, ISampleStore samples) {
            Texture = textures.Get("Emotes/Elvin Gadd");
            Sample = samples.Get("Emotes/Elvin Gadd's Lab");
        }
    }
}
