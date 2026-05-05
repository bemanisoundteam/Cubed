using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osu.Game.Rulesets.Cubed.Skinning.Indicators;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public static class CubedSkinRegistry {
        public static IBindableList<CubedGameplaySkin> GameplaySkins => GameplaySkinsList;
        public static IBindableList<CellGlowSkin> CellGlows => CellGlowsList;

        private static readonly BindableList<CubedGameplaySkin> GameplaySkinsList = [new (null, "Cubed")];
        private static readonly Dictionary<CubedGameplaySkin, GameplaySkinComponentFactories> GameplaySkinsDict = new() {
            [new CubedGameplaySkin(null, "Cubed")] = null
        };
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

        public static void RegisterGameplaySkin(CubedGameplaySkin skinInfo, Func<Drawable> marker, Func<Drawable> receptor, Func<Drawable> indicator) {
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Namespace);

            ArgumentNullException.ThrowIfNull(marker);
            ArgumentNullException.ThrowIfNull(receptor);
            ArgumentNullException.ThrowIfNull(indicator);

            GameplaySkinsDict.Add(skinInfo, new (marker, receptor, indicator));
            GameplaySkinsList.Add(skinInfo);
        }

        public static Drawable CreateGameplayComponent(CubedGameplaySkin skin, CubedSkinComponents component) {
            ArgumentNullException.ThrowIfNull(skin);
            if (component == CubedSkinComponents.CellGlow)
                throw new ArgumentException($"Gameplay skins do not handle cell glows. Use {nameof(CreateCellGlow)} with {nameof(CellGlowSkin)} instead.");

            GameplaySkinComponentFactories factory = GameplaySkinsDict[skin];

            return component switch {
                CubedSkinComponents.Marker => factory?.Marker() ?? new DefaultMarker(),
                CubedSkinComponents.Receptor => factory?.Receptor() ?? new DefaultReceptor(),
                CubedSkinComponents.Indicator => factory?.Indicator() ?? new DefaultIndicator(),
                _ => null
            };
        }

        public static Drawable CreateCellGlow(CellGlowSkin skin) {
            ArgumentNullException.ThrowIfNull(skin, nameof(skin));

            return CellGlowsDict[skin]();
        }

        private record GameplaySkinComponentFactories(Func<Drawable> Marker, Func<Drawable> Receptor, Func<Drawable> Indicator);
    }
}
