using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning.Gameplay {
    public record CubedGameplaySkin(string Name, string Namespace) {
        public Drawable CreateComponent(CubedSkinComponents component) =>
            CubedSkinRegistry.CreateGameplayComponent(this, component);

        public override string ToString() => Namespace != "Cubed"
            ? $"{Name} ({Namespace})"
            : Name ?? "Default";
    }

    public class BindableCubedGameplaySkin(CubedGameplaySkin value = default) : Bindable<CubedGameplaySkin>(value) {
        public override string ToString(string format, IFormatProvider formatProvider) =>
            JsonConvert.SerializeObject(Value);

        public override void Parse(object input, IFormatProvider provider) {
            if (input is string json)
                Value = JsonConvert.DeserializeObject<CubedGameplaySkin>(json);
            else
                base.Parse(input, provider);
        }

        protected override Bindable<CubedGameplaySkin> CreateInstance() => new BindableCubedGameplaySkin();
    }
}
