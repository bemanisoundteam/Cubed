using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Testing;
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
        private void load() {
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
        }

        [SetUpSteps]
        public void Reset() => AddStep("Reset panel", panel.Reset);

        protected override Ruleset CreateRuleset() => new CubedRuleset();
    }
}
