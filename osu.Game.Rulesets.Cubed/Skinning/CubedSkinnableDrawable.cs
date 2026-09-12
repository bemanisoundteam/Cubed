using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Indicators;
using osu.Game.Rulesets.Cubed.Skinning.Markers;
using osu.Game.Rulesets.Cubed.Skinning.Receptors;
using osu.Game.Skinning;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public partial class CubedSkinnableDrawable(CubedSkinComponents lookup)
        : SkinnableDrawable(new CubedSkinComponentLookup(lookup), DefaultFunction(lookup), ConfineMode.ScaleToFit) {
        protected override bool ApplySizeRestrictionsToDefault => true;

        // This accessor allows me to refresh skin before applying HitObjects
        public void FlushPendingSkinChange() => FlushPendingSkinChanges();

        [BackgroundDependencyLoader]
        private void load() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        private static Func<ISkinComponentLookup, Drawable> DefaultFunction(CubedSkinComponents l) => l switch {
            CubedSkinComponents.Marker => _ => new DefaultMarker(),
            CubedSkinComponents.Receptor => _ => new DefaultReceptor(),
            CubedSkinComponents.Indicator => _ => new DefaultIndicator(),
            CubedSkinComponents.CellGlow => _ => new DefaultCellGlow(),
            _ => null
        };
    }
}

