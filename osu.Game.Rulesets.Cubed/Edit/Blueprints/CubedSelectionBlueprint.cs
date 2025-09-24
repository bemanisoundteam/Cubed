using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public abstract partial class CubedSelectionBlueprint(CubedHitObject item) : HitObjectSelectionBlueprint<CubedHitObject>(item) {
        [BackgroundDependencyLoader]
        private void load() {
            RelativeSizeAxes = Axes.None;
            Scale = new Vector2(CubedPlayfield.CellScale);
        }
    }
}
