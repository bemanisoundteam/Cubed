using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Components;
using osu.Game.Rulesets.Scoring;
using osu.Game.Screens.Play.HUD;
using osu.Game.Skinning;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public class CubedSkinTransformer(ISkin skin) : SkinTransformer(skin) {
        public override Drawable GetDrawableComponent(ISkinComponentLookup lookup) {
            switch (lookup) {
                case SkinComponentLookup<HitResult> hitResult:
                    // This should be a setting, or done at the skin level instead of me having to do this...
                    if (Skin is ArgonProSkin && hitResult.Component is HitResult.Great or HitResult.Perfect)
                        return Drawable.Empty();

                    break;

                case CubedSkinComponentLookup component:
                    if (component.Component is CubedSkinComponents.CellGlow)
                        return new PointerPositionAwareCellGlow();

                    break;


                case GlobalSkinnableContainerLookup containerLookup:
                    switch (containerLookup.Lookup) {
                        // TODO This is wonky (aka will not hide keycounter for edited skins) and should implement apply defaults
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
                            else
                                // This will not hide it for edited skins sadly, will have to do with it for the moment...
                                components?.OfType<KeyCounterDisplay>().FirstOrDefault()?.Hide();

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
