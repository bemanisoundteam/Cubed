using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Cubed.UI {
    [Cached]
    public partial class CubedPlayfield : Playfield {
        [BackgroundDependencyLoader]
        private void load() {
            AddRangeInternal(new Drawable [] {
                HitObjectContainer,
            });
        }
    }
}
