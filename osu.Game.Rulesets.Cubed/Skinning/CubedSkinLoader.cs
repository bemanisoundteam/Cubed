using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Game.Rulesets.Cubed.Skinning.Indicators;
using osu.Game.Rulesets.Cubed.Skinning.Markers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public static class CubedSkinLoader {
        private const string SkinManifestFile = "skin.manifest";

        public static void DiscoverSkins(ResourceStore<byte[]> store, GameHost host) {
            // Enumerate manifest files
            IEnumerable<string> skinManifests = store.GetAvailableResources().Where(s => s.EndsWith(SkinManifestFile, StringComparison.Ordinal));

            foreach (string manifest in skinManifests)
                try {
                    string[] splitName = manifest.Split('/');
                    if (splitName.Length < 3)
                        throw new Exception($"Cubed: {manifest} appears to be a skin but isn't located in a valid directory structure");

                    string skinName = splitName[^2];
                    string skinNamespace = splitName[^3];

                    ResourceStore<byte[]> skinResources =
                        // Take into account separator, hence the - 1
                        new NamespacedResourceStore<byte[]>(store, manifest[..(manifest.Length - SkinManifestFile.Length - 1)]);

                    loadSkin(store.GetStream(manifest), skinName, skinNamespace, skinResources, host);
                }
                catch (Exception e) {
                    Logger.Error(e, e.Message.StartsWith("Cubed", StringComparison.Ordinal)
                        ? e.Message
                        : $"Cubed: Error whilst parsing {manifest}: {e.Message}");
                }
        }

        public static void DiscoverSkins(Storage storage, GameHost host) {
            foreach (string @namespace in storage.GetDirectories(""))
            foreach (string directory in storage.GetDirectories(@namespace))
                loadSkin(
                    storage.GetStream($"{directory}/{SkinManifestFile}"),
                    directory.Split(Path.DirectorySeparatorChar)[^1],
                    @namespace,
                    new StorageBackedResourceStore(storage.GetStorageForDirectory(directory)),
                    host
                );
        }

        private static void loadSkin(Stream manifest, string skinName, string skinNamespace, IResourceStore<byte[]> skinResources, GameHost host) {
            ArgumentNullException.ThrowIfNull(manifest, nameof(manifest));
            ArgumentException.ThrowIfNullOrWhiteSpace(skinName, nameof(skinName));
            ArgumentException.ThrowIfNullOrWhiteSpace(skinNamespace, nameof(skinNamespace));
            ArgumentNullException.ThrowIfNull(skinResources, nameof(skinResources));
            ArgumentNullException.ThrowIfNull(host, nameof(host));

            Dictionary<CubedSkinComponents, SkinConfig> componentConfigs = [];

            using (StreamReader reader = new(manifest)) {
                bool componentValid = false;
                // Initialised to a bogus value to make the compiler not error out
                CubedSkinComponents component = (CubedSkinComponents) 67;

                while (reader.ReadLine() is { } str) {
                    var line = str.AsSpan().Trim();

                    // Ignore whitespace or commented out lines
                    if (line.IsEmpty || line[0] == '#')
                        continue;

                    // Parse component
                    if (line[0] == '[' && line[^1] == ']') {
                        if (!Enum.TryParse(line.Slice(1, line.Length - 2), out component))
                            Logger.Log($"Cubed: Error whilst parsing {manifest}: Unknown skin component: {line.Slice(1, line.Length - 2)}");
                        else {
                            componentConfigs.Add(component, new SkinConfig());
                            componentValid = true;
                        }

                        continue;
                    }

                    // Ignore lines that don't belong to a component
                    if (!componentValid) {
                        Logger.Log($"Cubed: Ignored line \"{line}\" whilst parsing {manifest} as it doesn't belong to a valid skin component");
                        continue;
                    }

                    // Parse the config line
                    int equalsIndex = line.IndexOf('=');
                    if (equalsIndex == -1)
                        throw new Exception($"Cubed: Malformed line \"{line}\" in {manifest}");

                    var key = line.Slice(0, equalsIndex).Trim();
                    var val = line.Slice(equalsIndex + 1).Trim();

                    switch (key) {
                        case "Type": {
                            if (!Enum.TryParse(val, out componentConfigs[component].Type))
                                throw new Exception($"Cubed: Error whilst parsing {manifest}: Unknown Type : {val}");
                            break;
                        }
                        case "Framerate":
                            componentConfigs[component].Framerate = double.Parse(val, CultureInfo.InvariantCulture);
                            break;
                        case "FrameCount" or "ApproachFrameCount":
                            componentConfigs[component].FrameCount = int.Parse(val, CultureInfo.InvariantCulture);
                            break;
                        case "TapAtFrame":
                            componentConfigs[component].TapAtFrame = int.Parse(val, CultureInfo.InvariantCulture);
                            break;
                        case "JudgementFrameCount":
                            componentConfigs[component].JudgementFrameCount = int.Parse(val, CultureInfo.InvariantCulture);
                            break;
                        default:
                            Logger.Log($"Cubed: Encountered an unknown config key whilst parsing {manifest}: {key}");
                            break;
                    }
                }
            }

            TextureStore skinTextures = new(host.Renderer, host.CreateTextureLoaderStore(skinResources));

            if (componentConfigs.TryGetValue(CubedSkinComponents.CellGlow, out SkinConfig cellGlowConfig) && cellGlowConfig.Type != ImplementationType.Null)
                CubedSkinRegistry.RegisterCellGlow(new(skinName, skinNamespace), CellGlow(cellGlowConfig), skinResources);

            if (componentConfigs.TryGetValue(CubedSkinComponents.Marker, out SkinConfig markerConfig))
                CubedSkinRegistry.RegisterGameplaySkin(
                    new(skinName, skinNamespace),
                    Marker(markerConfig),
                    Receptor(),
                    Indicator(),
                    skinResources
                );

            return;

            Func<Drawable> CellGlow(SkinConfig config) {
                return config.Type switch {
                    ImplementationType.Texture => () => new Sprite { Texture = skinTextures.Get("CellGlow") },
                    ImplementationType.Animation => () => {
                        TextureAnimation animation = new(false);
                        for (int i = 0; i < config.FrameCount; i++)
                            animation.AddFrame(skinTextures.Get($"CellGlow{i}"), 1000 / config.Framerate);
                        animation.Loop = true;
                        return animation;
                    },
                    _ => throw new Exception($"Cubed: Error with skin {skinName} ({skinNamespace}): Invalid CellGlow Type")
                };
            }

            Func<Drawable> Marker(SkinConfig config) {
                return config.Type switch {
                    ImplementationType.Null => () => new NullMarker(),
                    ImplementationType.Animation => () => new AnimatedMarker(config.FrameCount, config.TapAtFrame, config.JudgementFrameCount),
                    _ => throw new Exception($"Cubed: Error with skin {skinName} ({skinNamespace}): Invalid Marker Type")
                };
            }

            Func<Drawable> Receptor() {
                if (!componentConfigs.TryGetValue(CubedSkinComponents.Receptor, out SkinConfig config))
                    return () => null;

                return config.Type switch {
                    ImplementationType.Null => Drawable.Empty,
                    ImplementationType.Texture => () => new Sprite { Texture = skinTextures.Get("Receptor") },
                    // ImplementationType.Animation => TODO,
                    _ => throw new Exception($"Cubed: Error with skin {skinName} ({skinNamespace}): Invalid Receptor Type")
                };
            }

            Func<Drawable> Indicator() {
                if (!componentConfigs.TryGetValue(CubedSkinComponents.Indicator, out SkinConfig config))
                    return () => null;

                return config.Type switch {
                    ImplementationType.Null => () => new NullIndicator(),
                    ImplementationType.Texture => () => new SpriteIndicator(skinTextures),
                    // ImplementationType.Animation => TODO,
                    _ => throw new Exception($"Cubed: Error with skin {skinName} ({skinNamespace}): Invalid Indicator Type")
                };
            }
        }

        private class SkinConfig {
            public ImplementationType Type;
            public double Framerate;
            public int FrameCount;
            public int TapAtFrame;
            public int JudgementFrameCount;
        }

        private enum ImplementationType {
            Null,
            Texture,
            Animation,
        }
    }
}
