using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Lists;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Screens.Play;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class CubedEmotesHandler : Container, IKeyBindingHandler<CubedAction> {
        public readonly BindableBool Enabled = new(true);

        internal static readonly Dictionary<uint, CubedEmoteLookup> Emotes = new();

        [BackgroundDependencyLoader(permitNulls: true)]
        private void load(Player player, CubedInputManager manager) {
            if (manager == null)
                throw new DependencyNotRegisteredException(this.GetType(), typeof(CubedInputManager));
            pressedActions = manager.KeyBindingContainer.PressedActions;
            if (player != null)
                Enabled.BindTarget = player.IsBreakTime;

            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        private SlimReadOnlyListWrapper<CubedAction> pressedActions;

        public bool OnPressed(KeyBindingPressEvent<CubedAction> e) {
            if (Enabled.Value) {
                uint pressedActionsBitfield = pressedActions.Aggregate<CubedAction, uint>(0,
                    (current, action) => current | 1u << (int) action);

                if (Emotes.TryGetValue(pressedActionsBitfield, out CubedEmoteLookup emote))
                    Fire(emote.CreateEmote());
            }

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<CubedAction> e) { }

        // FIRE IN THE HOLE
        public void Fire(CubedSkinnableEmote emote) {
            AddInternal(emote);
            emote.Size = new osuTK.Vector2(.40f);
            emote.Play();
        }
    }
}
