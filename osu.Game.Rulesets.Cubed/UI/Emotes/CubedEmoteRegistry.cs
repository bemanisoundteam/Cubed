using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Skinning;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public static partial class CubedEmoteRegistry {
        private static readonly Dictionary<CubedEmoteLookup, Func<ICubedEmote>> emotes = new() {
            [new CubedEmoteLookup(null, null)] = Empty
        };
        public static IReadOnlyDictionary<CubedEmoteLookup, Func<ICubedEmote>> Emotes => emotes;

        #region EMPTY EMOTE
        private static EmptyEmote Empty() => new ();

        private partial class EmptyEmote : Drawable, ICubedEmote {
            public void Fire(bool expire) => Expire();
        }
        #endregion

        public static void RegisterEmote(CubedEmoteLookup emote, Func<ICubedEmote> createDefault, uint trigger, String resultsScreenMessage = null) {
            // That error handling is open to discussion :
            // I could replace a null createDefault with a "missing emote warning" one
            ArgumentNullException.ThrowIfNull(createDefault);
            // Or making null triggers accepted but unable to be summoned in gameplay and without the tooltip
            if (trigger == 0)
                throw new ArgumentException("Tried to register an emote without a valid trigger!", nameof(trigger));

            emotes.Add(emote, createDefault);

            // I could move that part to an event or something if I expect external code to consume these
            CubedEmotesHandler.Emotes.Add(trigger, emote);
            if (!string.IsNullOrEmpty(resultsScreenMessage))
                CubedResultsScreenEmote.Messages.Add(emote, new (resultsScreenMessage, trigger));
        }
    }
}
