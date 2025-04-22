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
            AddStep("Larry ! I pick you", () => screen.PickEmote(new (LarryEmote.Name, Ruleset.Value.Name)));
            AddAssert("Current Emote is set to Larry", () => screen.CurrentEmote, Is.InstanceOf<LarryEmote>);
            AddAssert("Larry is on screen", () => screen.ChildrenOfType<LarryEmote>().Any());
            AddAssert("Text workey", () => screen.ChildrenOfType<OsuSpriteText>().First().Text.ToString(), () => Is.EqualTo(LarryEmote.Quote));
        }
    }
}
