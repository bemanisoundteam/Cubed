using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using System.Reflection;

namespace osu.Game.Rulesets.Cubed {
    public partial class ConciergeIcon : Sprite {
        private static bool UploadedToTheStores;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, AudioManager audio) {
            if (!UploadedToTheStores) {
                textures.AddTextureSource(new TextureLoaderStore(CubedRuleset.CreateNamespacedResourceStore("Textures")));
                ResourceStore<byte[]> sampleStore = (ResourceStore<byte[]>) audio.GetSampleStore().GetType().
                    GetField("store", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(audio.GetSampleStore());
                sampleStore!.AddStore(CubedRuleset.CreateNamespacedResourceStore("Samples"));
                UploadedToTheStores = true;
            }

            Texture = textures.Get("Cubed-logo");
        }
    }
}
