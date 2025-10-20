using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Skinning;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public partial class CubedSkinnableDrawable(CubedSkinComponents lookup)
        : SkinnableDrawable(new CubedSkinComponentLookup(lookup), DefaultFunction(lookup), ConfineMode.ScaleToFit) {
        protected override bool ApplySizeRestrictionsToDefault => true;

        private static Func<ISkinComponentLookup, Drawable> DefaultFunction(CubedSkinComponents l) => l switch {
            CubedSkinComponents.Marker => _ => new DefaultMarker(),
            // TODO Should I still do a receptor indicator split ?
            // CubedSkinComponents.Receptor => TODO,
            // CubedSkinComponents.Indicator => TODO,
            CubedSkinComponents.CellGlow => _ => new DefaultCellGlow(),
            _ => null
        };
    }

    public partial class DefaultMarker : CompositeDrawable, IMarker {
        private readonly Drawable approach = new Approach();
        private readonly Drawable marker = new Box {
            RelativeSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre
        };

        [BackgroundDependencyLoader]
        private void load() =>
            InternalChildren = [marker, approach];

        public void AnimateApproach(double time) => approach.ScaleTo(1, time).Then().FadeOut();

        public void AnimateHit(double duration) {
            approach.FadeOut();
            marker.ScaleTo(1.5f, duration, Easing.OutQuint);
        }

        private partial class Approach : CompositeDrawable {
            [BackgroundDependencyLoader]
            private void iWishThereWasABetterWayToDoThis() {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.Both;
                Scale = new osuTK.Vector2(2.5f);
                Masking = true;
                BorderColour = Colour4.Red;
                BorderThickness = 3;

                InternalChild = new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Transparent
                };
            }
        }
    }
}

