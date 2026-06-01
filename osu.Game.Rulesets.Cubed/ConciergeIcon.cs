using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Cubed.Skinning;
using System.Reflection;

namespace osu.Game.Rulesets.Cubed {
    public partial class ConciergeIcon : Sprite {
        /// <summary>
        /// A replacement for IRenderer.WhitePixel, that preserves base textRect size
        /// </summary>
        /// <remarks>Please inject and use Renderer.WhitePixel whenever possible instead</remarks>
        public static Texture WhitePixel { get; private set; }

        private static bool UploadedToTheStores;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, AudioManager audio, GameHost host, Storage storage) {
            if (!UploadedToTheStores) {
                textures.AddTextureSource(host.CreateTextureLoaderStore(CubedRuleset.CreateNamespacedResourceStore("Textures")));
                ResourceStore<byte[]> sampleStore = (ResourceStore<byte[]>) audio.GetSampleStore().GetType().
                    GetField("store", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(audio.GetSampleStore());
                sampleStore!.AddStore(CubedRuleset.CreateNamespacedResourceStore("Samples"));

                EnsureWhitePixel(host.Renderer);

                CubedSkinRegistry.InjectDependencies(host.Renderer, host.CreateTextureLoaderStore);

                CubedSkinLoader.DiscoverSkins(CubedRuleset.CreateNamespacedResourceStore("Skins"), host);
                CubedSkinLoader.DiscoverSkins(storage.GetStorageForDirectory("CubedSkins"), host);

                UploadedToTheStores = true;
            }

            Texture = textures.Get("Cubed-logo");
        }

        public static void EnsureWhitePixel(IRenderer renderer) =>
            WhitePixel ??= renderer.CreateTexture(1, 1, initialisationColour: Colour4.White);
    }
}
