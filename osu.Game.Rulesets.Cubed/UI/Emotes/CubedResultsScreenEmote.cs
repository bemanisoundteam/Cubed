using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public partial class CubedResultsScreenEmote : CompositeDrawable {
        internal static readonly Dictionary<Type, String> Messages = new();
        private readonly FillFlowContainer container = new() {
            RelativeSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Padding = new MarginPadding { Bottom = 12 }
        };

        public CubedEmote CurrentEmote { get; private set; }

        public void PickEmote(Type emote) {
            container.Clear();
            CurrentEmote = createEmote(emote, container);
            container.Add(new OsuSpriteText {
                Text = Messages[emote],
                Font = OsuFont.GetFont(size: 12),
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre
            });
        }

        [BackgroundDependencyLoader]
        private void load() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            FillMode = FillMode.Fit;
            FillAspectRatio = 1;
            RelativeSizeAxes = Axes.X;
            Height = 200;
            AddInternal(container);

            PickRandomEmote();
        }

        public void PickRandomEmote() =>
            PickEmote(Messages.Keys.ElementAt(new Random().Next(0, Messages.Count)));

        private static CubedEmote createEmote(Type emote, FillFlowContainer container) {
            CubedEmote elmote = (CubedEmote) Activator.CreateInstance(emote)!;
            container.Add(elmote);
            elmote.Anchor = Anchor.TopCentre;
            elmote.Origin = Anchor.TopCentre;
            elmote.FillMode = FillMode.Fit;
            elmote.FillAspectRatio = 1;

            elmote.Emote.RelativeSizeAxes = Axes.Both;
            elmote.Emote.Size = Vector2.One;
            return elmote;
        }

        protected override bool OnClick(ClickEvent e) =>
            CurrentEmote?.Sample.Play() != null;
    }
}
