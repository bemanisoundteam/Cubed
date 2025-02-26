using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Framework.Testing;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Cubed.Tests.Components {
    public partial class TestSceneResultsScreenEmote : OsuTestScene {
        private CubedResultsScreenEmote screen;

        [BackgroundDependencyLoader]
        private void load(GameHost host) {
            Dependencies.Cache(new TextureStore(host.Renderer,
                host.CreateTextureLoaderStore(CubedRuleset.CreateNamespacedResourceStore("Textures")), false));
            Child = screen = new CubedResultsScreenEmote();
        }

        [SetUpSteps]
        public void PickRandomEmote() => AddStep("Reroll emote", () => screen.PickRandomEmote());
    }
}
