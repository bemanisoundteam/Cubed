using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Testing;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Tests.Visual;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Tests {
    public partial class TestSceneEmotes : OsuTestScene {
        private CubedInputManager inputManager;
        private CubedPanel panel;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures) {
            Child = inputManager = new CubedInputManager(Ruleset.Value);
            // This is necessary for CubedCell to propagate touch/mouse events
            Dependencies.Cache(inputManager);
            inputManager.Add(new CubedPlayfieldAdjustmentContainer {
                Origin = Anchor.CentreRight,
                Anchor = Anchor.CentreRight,
                Child = new CubedPlayfield()
            });
            inputManager.Add(panel = new CubedPanel {
                Origin = Anchor.BottomLeft,
                Anchor = Anchor.BottomLeft,
                Size = new Vector2(180),
                SmallTriangles = new BindableBool(true)
            });
            bindPanelToInputManager();
            AddToggleStep("Toggle manual input", e => inputManager.UseParentInput = e);

            // Add custom TextureStore for use in debug emotes
            textures.AddTextureSource(new TextureLoaderStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(GetType().Assembly), "Resources/Textures/Emotes")));
        }

        private void bindPanelToInputManager() {
            for (int x = 0; x != 16; x++) {
                int x1 = x;
                panel.Cells[x / 4][x % 4].Active.BindValueChanged(e => {
                    if (e.NewValue)
                        inputManager.KeyBindingContainer.TriggerPressed((CubedAction) x1);
                    else
                        inputManager.KeyBindingContainer.TriggerReleased((CubedAction) x1);
                });
            }
        }

        [Test]
        // Don't dare try him tho...
        public void TestLarry() {
            AddStep("Bird fucking screams", () => panel.PressKeys(LarryEmote.Trigger));
            AddAssert("Bird is actually fucking screaming", () => inputManager.ChildrenOfType<LarryEmote>().Any());
            AddAssert("Bird is present", () => inputManager.ChildrenOfType<LarryEmote>().First().IsPresent);
            AddAssert("Bird is visible", () => (inputManager.ChildrenOfType<LarryEmote>().First().Content as Sprite)?.Texture != null);
        }

        [Test]
        public void TestWideEmote() =>
            AddStep("Fire wide emote", () => inputManager.ChildrenOfType<CubedEmotesHandler>().First().Fire(NatGeoEmote.Skinnable()));

        [SetUpSteps]
        public void Reset() => AddStep("Reset panel", ResetPanel);

        // Workaround to make headless tests work
        // Caused by a race condition where it calls SetUpSteps before load()
        // Which causes panel to be null at that moment
        private void ResetPanel() => panel.Reset();

        protected override Ruleset CreateRuleset() => new CubedRuleset();

        // Used for testing with a wide emote
        private partial class NatGeoEmote : CubedEmote {
            private static readonly CubedEmoteLookup Lookup = new("NATIONAL GEOGRAPHIC GOD DAMNIT", "Cubed Tests");
            public static CubedSkinnableEmote Skinnable() => new (Lookup, () => new NatGeoEmote());

            [BackgroundDependencyLoader]
            private void load(TextureStore textures) =>
                Content = new Sprite { Texture = textures.Get("Natgeologo.svg") };
        }
    }
}
