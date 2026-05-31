using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osu.Game.Rulesets.Cubed.Skinning.Indicators;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public static class CubedSkinRegistry {
        public static IBindableList<CubedGameplaySkinInfo> GameplaySkins => GameplaySkinsList;
        public static IBindableList<CellGlowSkinInfo> CellGlows => CellGlowsList;

        private static readonly BindableList<CubedGameplaySkinInfo> GameplaySkinsList = [new (null, "Cubed")];
        private static readonly Dictionary<CubedGameplaySkinInfo, GameplaySkinComponentFactories> GameplaySkinsDict = new() {
            [new CubedGameplaySkinInfo(null, "Cubed")] = null
        };
        private static readonly Dictionary<CellGlowSkinInfo, Func<Drawable>> CellGlowsDict = new() {
            [new CellGlowSkinInfo(null, "Cubed")] = () => new DefaultCellGlow()
        };
        private static readonly BindableList<CellGlowSkinInfo> CellGlowsList = [new(null, "Cubed")];

        public static void RegisterCellGlow(CellGlowSkinInfo skinInfo, Func<Drawable> cellGlow) {
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Namespace);

            ArgumentNullException.ThrowIfNull(cellGlow);

            CellGlowsDict.Add(skinInfo, cellGlow);
            CellGlowsList.Add(skinInfo);
        }

        public static void RegisterGameplaySkin(CubedGameplaySkinInfo skinInfo, Func<Drawable> marker, Func<Drawable> receptor, Func<Drawable> indicator) {
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Namespace);

            ArgumentNullException.ThrowIfNull(marker);
            ArgumentNullException.ThrowIfNull(receptor);
            ArgumentNullException.ThrowIfNull(indicator);

            GameplaySkinsDict.Add(skinInfo, new (marker, receptor, indicator));
            GameplaySkinsList.Add(skinInfo);
        }

        public static Drawable CreateGameplayComponent(CubedGameplaySkinInfo skinInfo, CubedSkinComponents component) {
            ArgumentNullException.ThrowIfNull(skinInfo);
            if (component == CubedSkinComponents.CellGlow)
                throw new ArgumentException($"Gameplay skins do not handle cell glows. Use {nameof(CreateCellGlow)} with {nameof(CellGlowSkinInfo)} instead.");

            GameplaySkinComponentFactories factory = GameplaySkinsDict[skinInfo];

            return component switch {
                CubedSkinComponents.Marker => factory?.Marker() ?? new DefaultMarker(),
                CubedSkinComponents.Receptor => factory?.Receptor() ?? new DefaultReceptor(),
                CubedSkinComponents.Indicator => factory?.Indicator() ?? new DefaultIndicator(),
                _ => null
            };
        }

        public static Drawable CreateCellGlow(CellGlowSkinInfo skinInfo) {
            ArgumentNullException.ThrowIfNull(skinInfo, nameof(skinInfo));

            return CellGlowsDict[skinInfo]();
        }

        private record GameplaySkinComponentFactories(Func<Drawable> Marker, Func<Drawable> Receptor, Func<Drawable> Indicator);
    }
}
