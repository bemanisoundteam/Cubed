using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedCell : Playfield {
        [BackgroundDependencyLoader]
        private void load() {
            RegisterPool<Cube, DrawableCube>(20, 100);
        }
    }
}
