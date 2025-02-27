using NUnit.Framework;
using osu.Framework.Testing;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Tests.Visual;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Tests.Components {
    public partial class TestSceneResultsScreenEmote : OsuTestScene {
        private readonly CubedResultsScreenEmote screen;

        public TestSceneResultsScreenEmote() {
            Child = screen = new CubedResultsScreenEmote();
            AddStep("Reroll emote", () => screen.PickRandomEmote());
        }

        protected override Ruleset CreateRuleset() => new CubedRuleset();

        [Test]
        public void Larry() {
            AddStep("Larry ! I pick you", () => screen.PickEmote(typeof(LarryEmote)));
            AddAssert("Larry is on screen", () => screen.ChildrenOfType<LarryEmote>().Any());
            AddAssert("Text workey", () => screen.ChildrenOfType<OsuSpriteText>().First().Text == LarryEmote.Quote);
        }
    }
}
