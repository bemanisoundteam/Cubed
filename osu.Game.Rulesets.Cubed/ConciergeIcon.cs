using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Cubed.Skinning;
using System.Reflection;
using System.Threading;

namespace osu.Game.Rulesets.Cubed {
    public partial class ConciergeIcon : Sprite {
        private static int _isFirstLoad;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, AudioManager audio, GameHost host, Storage storage) {
            if (Interlocked.CompareExchange(ref _isFirstLoad, 727, 0) == 0) {
                textures.AddTextureSource(host.CreateTextureLoaderStore(CubedRuleset.CreateNamespacedResourceStore("Textures")));
                ResourceStore<byte[]> sampleStore = (ResourceStore<byte[]>) audio.GetSampleStore().GetType().
                    GetField("store", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(audio.GetSampleStore());
                sampleStore!.AddStore(CubedRuleset.CreateNamespacedResourceStore("Samples"));

                CubedSkinRegistry.InjectDependencies(host.Renderer, host.CreateTextureLoaderStore);

                CubedSkinLoader.DiscoverSkins(CubedRuleset.CreateNamespacedResourceStore("Skins"));
                CubedSkinLoader.DiscoverSkins(storage.GetStorageForDirectory("CubedSkins"));
            }

            Texture = textures.Get("ruleset-icon");
        }
    }
}
