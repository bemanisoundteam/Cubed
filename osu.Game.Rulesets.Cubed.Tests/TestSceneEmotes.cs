using osu.Framework.Allocation;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Cubed.Tests {
    public partial class TestSceneEmotes : OsuTestScene {
        private CubedInputManager inputManager;

        [BackgroundDependencyLoader]
        private void load() {
            Child = inputManager = new CubedInputManager(Ruleset.Value);
            // This is necessary for CubedCell to propagate touch/mouse events
            Dependencies.Cache(inputManager);
            inputManager.Add(new CubedPlayfieldAdjustmentContainer { Child = new CubedPlayfield() });
        }

        protected override Ruleset CreateRuleset() => new CubedRuleset();
    }
}
