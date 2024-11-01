using NUnit.Framework;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Cubed.Tests.Editor {
    [TestFixture]
    public partial class TestSceneCubedEditor : EditorTestScene {
        protected override Ruleset CreateEditorRuleset() => new CubedRuleset();
    }
}
