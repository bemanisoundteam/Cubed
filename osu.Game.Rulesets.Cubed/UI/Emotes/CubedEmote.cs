using osu.Framework.Allocation;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Skinning;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public abstract partial class CubedEmote : CompositeDrawable {
        private static readonly Dictionary<CubedEmoteLookup, Func<CubedEmote>> emotes = new() {
            [new CubedEmoteLookup(null, null)] = Empty
        };
        public static IReadOnlyDictionary<CubedEmoteLookup, Func<CubedEmote>> Emotes => emotes;

        public static new CubedEmote Empty() => new EmptyEmote();

        public Drawable Content {
            get => InternalChild;
            protected set {
                value.Anchor = Anchor.Centre;
                value.Origin = Anchor.Centre;
                value.RelativeSizeAxes = Axes.Both;
                value.Size = osuTK.Vector2.One;
                value.Scale = osuTK.Vector2.One;
                value.FillMode = FillMode.Fit;
                InternalChild = value;
            }
        }

        public ISample Sample { get; protected set; }
        private SampleChannel Channel;

        // I advise users of RegisterEmote to register them in a static constructor inside their Ruleset class
        public static void RegisterEmote(CubedEmoteLookup emote, Func<CubedEmote> createDefault, uint trigger, String resultsScreenMessage = null) {
            // That error handling is open to discussion
            // I could replace a null createDefault with an "empty emote" one
            ArgumentNullException.ThrowIfNull(createDefault);
            // Or making null triggers accepted but with reduced functionality
            if (trigger == 0)
                throw new ArgumentException("Tried to register an emote without a valid trigger!", nameof(trigger));

            emotes.Add(emote, createDefault);
            CubedEmotesHandler.Emotes.Add(trigger, emote);
            if (!String.IsNullOrEmpty(resultsScreenMessage))
                CubedResultsScreenEmote.Messages.Add(emote, new (resultsScreenMessage, trigger));
        }

        [BackgroundDependencyLoader]
        private void load() =>
            RelativeSizeAxes = Axes.Both;

        public void Play() => Channel = Sample?.Play();

        protected override void Update() {
            if (Channel is not null && Channel.HasCompleted)
                Expire();
        }

        private partial class EmptyEmote : CubedEmote;
    }
}
