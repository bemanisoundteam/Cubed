using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class CubedEmotesController(CubedEmotesContainer emotesContainer) : Component, IKeyBindingHandler<CubedAction> {
        public readonly BindableBool Enabled = new(true);

        [BackgroundDependencyLoader(permitNulls: true)]
        private void load(Player player) {
            if (player != null)
                ((IBindable<bool>) Enabled).BindTo(player.IsBreakTime);
        }

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) {
            if (Enabled.Value)
                // TODO IMPLEMENT THIS BECAUSE THAT FUCKING BIRD RUINED MY LIFE
                emotesContainer.Fire(new LarryEmote());
            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) { }
    }
}
