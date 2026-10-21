using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces {
    public partial class HoldLinkSelectionPiece : CubeSelectionPiece {
        [BackgroundDependencyLoader]
        private void load() =>
            Origin = Anchor.TopCentre;
    }
}
