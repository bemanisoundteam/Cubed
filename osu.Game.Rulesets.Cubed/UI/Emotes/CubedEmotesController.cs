using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Lists;
using osu.Game.Screens.Play;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class CubedEmotesController(CubedEmotesContainer emotesContainer) : Component, IKeyBindingHandler<CubedAction> {
        public readonly BindableBool Enabled = new(true);

        internal static readonly Dictionary<uint, Type> Emotes = new();

        [BackgroundDependencyLoader(permitNulls: true)]
        private void load(Player player, CubedInputManager manager) {
            if (manager == null)
                throw new DependencyNotRegisteredException(this.GetType(), typeof(CubedInputManager));
            pressedActions = manager.KeyBindingContainer.PressedActions;
            if (player != null)
                ((IBindable<bool>) Enabled).BindTo(player.IsBreakTime);
        }

        private SlimReadOnlyListWrapper<CubedAction> pressedActions;

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) {
            if (Enabled.Value) {
                uint pressedActionsBitfield = pressedActions.Aggregate<CubedAction, uint>(0,
                    (current, action) => current | 1u << (int) action);

                if (Emotes.TryGetValue(pressedActionsBitfield, out Type emote))
                    emotesContainer.Fire((CubedEmote) Activator.CreateInstance(emote));
            }

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) { }
    }
}
