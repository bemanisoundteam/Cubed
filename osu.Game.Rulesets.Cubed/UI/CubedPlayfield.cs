using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Cubed.UI {
    [Cached]
    public partial class CubedPlayfield : Playfield {
        [BackgroundDependencyLoader]
        private void load() {
            AddInternal(HitObjectContainer);
            RegisterPool<Cube, DrawableCube>(20, 100);
        }
    }
}
