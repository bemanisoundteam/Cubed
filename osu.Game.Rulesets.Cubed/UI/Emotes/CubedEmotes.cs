using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Lists;
using osu.Game.Screens.Play;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class CubedEmotes : Container, IKeyBindingHandler<CubedAction> {
        public readonly BindableBool Enabled = new(true);

        internal static readonly Dictionary<uint, Type> Emotes = new();

        [BackgroundDependencyLoader(permitNulls: true)]
        private void load(Player player, CubedInputManager manager) {
            if (manager == null)
                throw new DependencyNotRegisteredException(this.GetType(), typeof(CubedInputManager));
            pressedActions = manager.KeyBindingContainer.PressedActions;
            if (player != null)
                ((IBindable<bool>) Enabled).BindTo(player.IsBreakTime);

            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        private SlimReadOnlyListWrapper<CubedAction> pressedActions;

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) {
            if (Enabled.Value) {
                uint pressedActionsBitfield = pressedActions.Aggregate<CubedAction, uint>(0,
                    (current, action) => current | 1u << (int) action);

                if (Emotes.TryGetValue(pressedActionsBitfield, out Type emote))
                    Fire((CubedEmote) Activator.CreateInstance(emote));
            }

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) { }

        // FIRE IN THE HOLE
        private void Fire(CubedEmote emote) {
            AddInternal(emote);
            emote.Size = new osuTK.Vector2(.40f);
            // Enforce this, as sprites set their size according to their texture
            emote.Emote.RelativeSizeAxes = Axes.Both;
            emote.Emote.Size = osuTK.Vector2.One;
            emote.Play();
        }
    }
}
