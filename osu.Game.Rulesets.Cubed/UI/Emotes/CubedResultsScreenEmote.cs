using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Cubed.Skinning;
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class CubedResultsScreenEmote : CompositeDrawable, IHasCustomTooltip<uint> {
        internal static readonly Dictionary<CubedEmoteLookup, Tuple<String, uint>> Messages = new();
        private readonly FillFlowContainer container = new() {
            RelativeSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Padding = new MarginPadding { Bottom = 12 }
        };

        public ICubedEmote CurrentEmote { get; private set; }
        private uint CurrentTrigger;

        public void PickEmote(CubedEmoteLookup emote) {
            container.Clear();
            CurrentEmote = createEmote(emote, container);
            CurrentTrigger = Messages[emote].Item2;
            container.Add(new OsuSpriteText {
                Text = Messages[emote].Item1,
                Font = OsuFont.GetFont(size: 12),
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre
            });
        }

        [BackgroundDependencyLoader]
        private void load() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Size = new Vector2(200);
            AddInternal(container);

            PickRandomEmote();
        }

        public void PickRandomEmote() =>
            PickEmote(Messages.Keys.ElementAt(new Random().Next(0, Messages.Count)));

        private static ICubedEmote createEmote(CubedEmoteLookup emote, FillFlowContainer container) {
            CubedSkinnableEmote elmote = emote.CreateEmote();
            container.Add(elmote);

            // These lines are here to center the emote and because the FillFlowContainer requires it
            // But for some funny reason if they're placed before adding the emote to it, it will not like it and throw
            elmote.Anchor = Anchor.TopCentre;
            elmote.Origin = Anchor.TopCentre;

            return elmote.Emote;
        }

        protected override bool OnClick(ClickEvent e) {
            CurrentEmote?.Fire(false);
            return CurrentEmote != null;
        }

        public ITooltip<uint> GetCustomTooltip() => new CubedPanelTooltip();

        public uint TooltipContent => CurrentTrigger;
    }
}
