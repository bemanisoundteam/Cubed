using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Skinning.Components;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public class CubedSkinTransformer(ISkin skin) : SkinTransformer(skin) {
        public override Drawable GetDrawableComponent(ISkinComponentLookup lookup) {
            switch (lookup) {
                case GlobalSkinnableContainerLookup containerLookup:
                    switch (containerLookup.Lookup) {
                        case GlobalSkinnableContainers.MainHUDComponents:
                            Container components = (Container) base.GetDrawableComponent(lookup);

                            if (containerLookup.Ruleset != null) {
                                components ??= new DefaultSkinComponentsContainer(_ => {});

                                // Stolen from ArgonSkin.cs
                                const float padding = 10;
                                // Hard to find this at runtime, so taken from the most expanded state during replay.
                                const float paddedSongProgressHeight = 36 + padding * 3;

                                // This does not work with apply defaults... and I can't be bothered to do reflection black magic
                                components.Add(new CubedKeyCounterDisplay {
                                    Anchor = Anchor.BottomRight,
                                    Origin = Anchor.BottomRight,
                                    Position = new Vector2(-padding, -paddedSongProgressHeight)
                                });
                            }

                            return components;
                    }

                    if (containerLookup.Ruleset != null)
                        return null;  // This is because I do not touch playfield or song select

                    break;
            }

            return base.GetDrawableComponent(lookup);
        }
    }
}
