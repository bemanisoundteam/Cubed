using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning.Gameplay {
    public partial class CubedGameplaySkin(CubedGameplaySkinInfo skinInfo) : ICubedGameplaySkin {
        public readonly CubedGameplaySkinInfo SkinInfo = skinInfo;

        // Lazy because SkinRegistry might not have received GameHost yet
        private readonly Lazy<TextureStore> textures = new(
            () => CubedSkinRegistry.CreateTextureStore(CubedSkinRegistry.GetSkinResources(skinInfo)));

        public Drawable CreateComponent(CubedSkinComponents component) =>
            CubedSkinRegistry.CreateGameplayComponent(SkinInfo, component);

        public Texture GetTexture(string name, WrapMode wrapModeS, WrapMode wrapModeT) =>
            textures.Value.Get(name, wrapModeS, wrapModeT);

        public void Dispose() {
            textures.Value.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
