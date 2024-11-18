using osu.Framework.Graphics;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPlayfieldAdjustmentContainer : PlayfieldAdjustmentContainer {
        public CubedPlayfieldAdjustmentContainer() {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            FillAspectRatio = 1;
            FillMode = FillMode.Fit;
        }
    }
}
