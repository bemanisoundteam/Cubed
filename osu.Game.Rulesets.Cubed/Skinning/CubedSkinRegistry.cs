using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osu.Game.Rulesets.Cubed.Skinning.Indicators;
using osu.Game.Rulesets.Cubed.Skinning.Markers;
using osu.Game.Rulesets.Cubed.Skinning.Receptors;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public static class CubedSkinRegistry {
        public static IBindableList<CubedGameplaySkinInfo> GameplaySkins => GameplaySkinsList;
        public static IBindableList<CellGlowSkinInfo> CellGlows => CellGlowsList;

        private static readonly BindableList<CubedGameplaySkinInfo> GameplaySkinsList = [
            #if DEBUG
            new (null, "Cubed")
            #endif
        ];
        private static readonly Dictionary<CubedGameplaySkinInfo, IResourceStore<byte[]>> GameplaySkinResources = new();
        private static readonly Dictionary<CubedGameplaySkinInfo, GameplaySkinComponentFactories> GameplaySkinsDict = new() {
            #if DEBUG
            [new CubedGameplaySkinInfo(null, "Cubed")] = null
            #endif
        };
        private static readonly Dictionary<CellGlowSkinInfo, Func<Drawable>> CellGlowsDict = new() {
            #if DEBUG
            [new CellGlowSkinInfo(null, "Cubed")] = () => new DefaultCellGlow()
            #endif
        };
        private static readonly Dictionary<CellGlowSkinInfo, IResourceStore<byte[]>> CellGlowSkinResources = new();
        private static readonly BindableList<CellGlowSkinInfo> CellGlowsList = [
            #if DEBUG
            new (null, "Cubed")
            #endif
        ];

        public static void RegisterCellGlow(CellGlowSkinInfo skinInfo, Func<Drawable> cellGlow, IResourceStore<byte[]> skinResources = null) {
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Namespace);

            ArgumentNullException.ThrowIfNull(cellGlow);

            CellGlowsDict.Add(skinInfo, cellGlow);
            CellGlowsList.Add(skinInfo);
            if (skinResources != null)
                CellGlowSkinResources.Add(skinInfo, skinResources);
        }

        public static void RegisterGameplaySkin(CubedGameplaySkinInfo skinInfo, Func<Drawable> marker, Func<Drawable> receptor, Func<Drawable> indicator, IResourceStore<byte[]> skinResources = null) {
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Name);
            ArgumentException.ThrowIfNullOrWhiteSpace(skinInfo.Namespace);

            ArgumentNullException.ThrowIfNull(marker);
            ArgumentNullException.ThrowIfNull(receptor);
            ArgumentNullException.ThrowIfNull(indicator);

            GameplaySkinsDict.Add(skinInfo, new (marker, receptor, indicator));
            GameplaySkinsList.Add(skinInfo);
            if (skinResources != null)
                GameplaySkinResources.Add(skinInfo, skinResources);
        }

        public static Drawable CreateGameplayComponent(CubedGameplaySkinInfo skinInfo, CubedSkinComponents component) {
            ArgumentNullException.ThrowIfNull(skinInfo);
            if (component == CubedSkinComponents.CellGlow)
                throw new ArgumentException($"Gameplay skins do not handle cell glows. Use {nameof(CreateCellGlow)} with {nameof(CellGlowSkinInfo)} instead.");

            GameplaySkinComponentFactories factory = GameplaySkinsDict.GetValueOrDefault(skinInfo);

            return component switch {
                CubedSkinComponents.Marker => factory?.Marker() ?? new DefaultMarker(),
                CubedSkinComponents.Receptor => factory?.Receptor() ?? new DefaultReceptor(),
                CubedSkinComponents.Indicator => factory?.Indicator() ?? new DefaultIndicator(),
                _ => null
            };
        }

        public static Drawable CreateCellGlow(CellGlowSkinInfo skinInfo) {
            ArgumentNullException.ThrowIfNull(skinInfo, nameof(skinInfo));

            return CellGlowsDict.GetValueOrDefault(skinInfo)?.Invoke() ?? new DefaultCellGlow();
        }

        public static IResourceStore<byte[]> GetSkinResources(CubedGameplaySkinInfo skinInfo) =>
            GameplaySkinResources.GetValueOrDefault(skinInfo);

        public static IResourceStore<byte[]> GetSkinResources(CellGlowSkinInfo skinInfo) =>
            CellGlowSkinResources.GetValueOrDefault(skinInfo);

        public static TextureStore CreateTextureStore(IResourceStore<byte[]> resources) =>
            new (Renderer, CreateTextureLoaderStore(resources));

        #region Dependencies

        private static IRenderer Renderer;
        private static Func<IResourceStore<byte[]>, IResourceStore<TextureUpload>> CreateTextureLoaderStore;

        internal static void InjectDependencies(IRenderer renderer, Func<IResourceStore<byte[]>, IResourceStore<TextureUpload>> createTextureLoaderStore) {
            Renderer = renderer;
            CreateTextureLoaderStore = createTextureLoaderStore;
        }

        #endregion

        private record GameplaySkinComponentFactories(Func<Drawable> Marker, Func<Drawable> Receptor, Func<Drawable> Indicator);
    }
}
