using osu.Framework.Graphics.Textures;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning.Gameplay {
    public interface ICubedGameplaySkin : IDisposable {
        public Texture GetTexture(string name) => GetTexture(name, default, default);

        public Texture GetTexture(string name, WrapMode wrapModeS, WrapMode wrapModeT);
    }
}
