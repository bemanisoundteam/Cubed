using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using System.Reflection;

namespace osu.Game.Rulesets.Cubed {
    public partial class ConciergeIcon : Sprite {
        /// <summary>
        /// A replacement for IRenderer.WhitePixel, that preserves base
        /// </summary>
        /// <remarks>Please inject and use Renderer.WhitePixel whenever possible instead</remarks>
        public static Texture WhitePixel { get; private set; }

        private static bool UploadedToTheStores;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, AudioManager audio, IRenderer renderer) {
            if (!UploadedToTheStores) {
                textures.AddTextureSource(new TextureLoaderStore(CubedRuleset.CreateNamespacedResourceStore("Textures")));
                ResourceStore<byte[]> sampleStore = (ResourceStore<byte[]>) audio.GetSampleStore().GetType().
                    GetField("store", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(audio.GetSampleStore());
                sampleStore!.AddStore(CubedRuleset.CreateNamespacedResourceStore("Samples"));
                WhitePixel = renderer.CreateTexture(1, 1, initialisationColour: Colour4.White);

                UploadedToTheStores = true;
            }

            Texture = textures.Get("Cubed-logo");
        }
    }
}
