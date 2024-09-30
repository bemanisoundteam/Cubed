using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Cubed {
    public partial class ConciergeIcon : Sprite {
        private readonly CubedRuleset ruleset;

        public ConciergeIcon(CubedRuleset ruleset) {
            this.ruleset = ruleset;
        }

        [BackgroundDependencyLoader]
        private void load(GameHost host) {
            Texture = new TextureStore(host.Renderer, new TextureLoaderStore(ruleset.CreateResourceStore())).Get("Cubed-logo");
        }
    }
}
