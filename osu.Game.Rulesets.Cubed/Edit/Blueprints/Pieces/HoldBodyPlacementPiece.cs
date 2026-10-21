using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.Skinning.Indicators;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints.Pieces {
    public partial class HoldBodyPlacementPiece(CubedHoldNote hold) : CompositeDrawable {
        private readonly CubedSkinnableDrawable indicator = new (CubedSkinComponents.Indicator);
        private IHoldIndicator Indicator => (IHoldIndicator) indicator.Drawable;

        [BackgroundDependencyLoader]
        private void load() {
            indicator.OnSkinChanged += Refresh;
            InternalChild = indicator;
        }

        public void Refresh() {
            Indicator.ComputeProgress = () => 0;

            Indicator.Direction = hold.Direction;
            Indicator.Length = hold.TailLength;
        }
    }
}
