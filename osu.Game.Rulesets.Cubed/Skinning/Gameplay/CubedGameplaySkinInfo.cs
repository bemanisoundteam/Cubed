using Newtonsoft.Json;
using osu.Framework.Bindables;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning.Gameplay {
    public record CubedGameplaySkinInfo(string Name, string Namespace) {
        public CubedGameplaySkin CreateSkin() => new(this);

        public override string ToString() => Namespace != "Cubed"
            ? $"{Name} ({Namespace})"
            : Name ?? "Default";
    }

    public class BindableCubedGameplaySkinInfo(CubedGameplaySkinInfo value = default) : Bindable<CubedGameplaySkinInfo>(value) {
        public override string ToString(string format, IFormatProvider formatProvider) =>
            JsonConvert.SerializeObject(Value);

        public override void Parse(object input, IFormatProvider provider) {
            if (input is string json)
                Value = JsonConvert.DeserializeObject<CubedGameplaySkinInfo>(json);
            else
                base.Parse(input, provider);
        }

        protected override Bindable<CubedGameplaySkinInfo> CreateInstance() => new BindableCubedGameplaySkinInfo();
    }
}
