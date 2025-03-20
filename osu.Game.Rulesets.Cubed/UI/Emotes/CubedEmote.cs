using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Audio;
using osu.Framework.Graphics.Containers;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public abstract partial class CubedEmote : CompositeDrawable {
        private static readonly List<Type> emotes = [];
        public static IEnumerable<Type> Emotes => emotes;

        public Drawable Emote { get; protected set; }
        public DrawableSample Sample { get; protected set; }
        private SampleChannel Channel;

        // I advise users of RegisterEmote to register them in a static constructor inside their Ruleset class
        public static void RegisterEmote(Type emote, uint trigger = 0, String resultsScreenMessage = null) {
            ArgumentNullException.ThrowIfNull(emote);
            if (!(typeof(CubedEmote).IsAssignableFrom(emote)))
                throw new ArgumentException($"Type '{emote.Name}' is not a {nameof(CubedEmote)}");

            if (emotes.Contains(emote))
                throw new ArgumentException($"Type '{emote.Name}' is already registered");

            emotes.Add(emote);
            if (trigger != 0)
                CubedEmotes.Emotes.Add(trigger, emote);
            if (!String.IsNullOrEmpty(resultsScreenMessage))
                CubedResultsScreenEmote.Messages.Add(emote, resultsScreenMessage);
        }

        public void Play() => Channel = Sample.Play();

        [BackgroundDependencyLoader]
        private void load() {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void Update() {
            if (Channel is not null && Channel.HasCompleted)
                Expire();
        }
    }
}
