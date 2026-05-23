using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace osu.Game.Rulesets.Cubed.Skinning.Indicators {
    public partial class NullIndicator : Drawable, IHoldIndicator {
        public HoldDirection Direction { get; set; }

        public int Length { get; set; }

        public CubedHoldNote Object { get; set; }

        [Resolved]
        public Dictionary<CubedHoldNote, double> PressTimes { get; private set; }

    }
}
