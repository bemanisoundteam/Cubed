using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning.CellGlows {
    public record CellGlowSkinInfo(string Name, string Namespace) {
        public Drawable CreateCellGlow() => CubedSkinRegistry.CreateCellGlow(this);

        public CellGlowSkin CreateSkin() => new (this);

        public override string ToString() => Namespace != "Cubed"
            ? $"{Name} ({Namespace})"
            : Name ?? "Default";
    }

    public class BindableCellGlowSkinInfo(CellGlowSkinInfo value = default) : Bindable<CellGlowSkinInfo>(value) {
        public override string ToString(string format, IFormatProvider formatProvider) =>
            JsonConvert.SerializeObject(Value);

        public override void Parse(object input, IFormatProvider provider) {
            if (input is string json)
                Value = JsonConvert.DeserializeObject<CellGlowSkinInfo>(json);
            else
                base.Parse(input, provider);
        }

        protected override Bindable<CellGlowSkinInfo> CreateInstance() => new BindableCellGlowSkinInfo();
    }
}
