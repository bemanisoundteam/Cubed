using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning.CellGlows {
    public record CellGlowSkin(string Name, string Namespace) {
        public Drawable CreateCellGlow() => CubedSkinRegistry.CreateCellGlow(this);

        public override string ToString() => Namespace != "Cubed"
            ? $"{Name} ({Namespace})"
            : Name ?? "Default";
    }

    public class BindableCellGlowSkin(CellGlowSkin value = default) : Bindable<CellGlowSkin>(value) {
        public override string ToString(string format, IFormatProvider formatProvider) =>
            JsonConvert.SerializeObject(Value);

        public override void Parse(object input, IFormatProvider provider) {
            if (input is string json)
                Value = JsonConvert.DeserializeObject<CellGlowSkin>(json);
            else
                base.Parse(input, provider);
        }

        protected override Bindable<CellGlowSkin> CreateInstance() => new BindableCellGlowSkin();
    }
}
