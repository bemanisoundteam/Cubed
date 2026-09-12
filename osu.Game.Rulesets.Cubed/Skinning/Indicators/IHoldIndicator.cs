using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning.Indicators {
    public interface IHoldIndicator : IDrawable {
        HoldDirection Direction { set; }

        int Length { set; }

        Func<double> ComputeProgress { set; }
    }
}
