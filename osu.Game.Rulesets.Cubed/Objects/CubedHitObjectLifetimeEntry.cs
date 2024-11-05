using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Cubed.Objects {
    public class CubedHitObjectLifetimeEntry(CubedHitObject ho) : HitObjectLifetimeEntry(ho) {
        protected override double InitialLifetimeOffset => ((IHasTimePreempt) HitObject).TimePreempt;
    }
}
