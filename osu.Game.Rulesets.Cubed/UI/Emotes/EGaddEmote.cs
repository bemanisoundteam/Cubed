using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class EGaddEmote : CubedEmote {
        // ⬜⬜⬜⬜
        // 🟥⬜⬜🟥
        // 🟥⬜⬜🟥
        // ⬜⬜⬜⬜
        public const uint Trigger = 0b0000100110010000;
        public new const string Name = "Professor E. Gadd";

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, ISampleStore samples) {
            Content = new Sprite { Texture = textures.Get("Emotes/Elvin Gadd") };
            Sample = samples.Get("Emotes/Elvin Gadd's Lab");
        }
    }
}
