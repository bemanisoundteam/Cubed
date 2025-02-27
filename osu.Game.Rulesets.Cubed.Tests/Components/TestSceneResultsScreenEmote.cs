using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Framework.Testing;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Tests.Visual;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Tests.Components {
    public partial class TestSceneResultsScreenEmote : OsuTestScene {
        private CubedResultsScreenEmote screen;

        [BackgroundDependencyLoader]
        private void load(GameHost host) {
            Dependencies.Cache(new TextureStore(host.Renderer,
                host.CreateTextureLoaderStore(CubedRuleset.CreateNamespacedResourceStore("Textures")), false));
            Child = screen = new CubedResultsScreenEmote();
            AddStep("Reroll emote", () => screen.PickRandomEmote());
        }

        [Test]
        public void Larry() {
            AddStep("Larry ! I pick you", () => screen.PickEmote(typeof(LarryEmote)));
            AddAssert("Larry is on screen", () => screen.ChildrenOfType<LarryEmote>().Any());
            AddAssert("Text workey", () => screen.ChildrenOfType<OsuSpriteText>().First().Text == LarryEmote.Quote);
        }
    }
}
