using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.Configuration;
using osu.Game.Rulesets.Cubed.Skinning;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces {
    public partial class CubePlacementPiece : CellGlowPreviewer {
        [BackgroundDependencyLoader]
        private void load() =>
            CellGlow = new CubedSkinnableDrawable(CubedSkinComponents.CellGlow);
    }
}
