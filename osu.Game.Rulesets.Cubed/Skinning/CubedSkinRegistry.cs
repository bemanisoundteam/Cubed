using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public static class CubedSkinRegistry {
        public static IBindableList<CellGlowSkin> CellGlows => CellGlowsList;

        private static readonly Dictionary<CellGlowSkin, Func<Drawable>> CellGlowsDict = [];
        private static readonly BindableList<CellGlowSkin> CellGlowsList = [];
        private static readonly Dictionary<CellGlowSkin, Func<Drawable>> CellGlowsDict = new() {
            [new CellGlowSkin(null, "Cubed")] = () => new DefaultCellGlow()
        };
        private static readonly BindableList<CellGlowSkin> CellGlowsList = [new(null, "Cubed")];

        public static void RegisterCellGlow(CellGlowSkin skinInfo, Func<Drawable> cellGlow) {
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Namespace);

            ArgumentNullException.ThrowIfNull(cellGlow);

            CellGlowsDict.Add(skinInfo, cellGlow);
            CellGlowsList.Add(skinInfo);
        }

        public static Drawable CreateCellGlow(CellGlowSkin skin) {
            ArgumentNullException.ThrowIfNull(skin, nameof(skin));

            return CellGlowsDict[skin]();
        }
    }
}
