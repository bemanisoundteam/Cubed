using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning.Indicators {
    public partial class NullIndicator : Drawable, IHoldIndicator {
        public HoldDirection Direction { set {} }

        public int Length { set {} }

        public Func<double> ComputeProgress { set {} }
    }
}
