using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Skinning;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public partial class CubedSkinnableEmote : SkinnableDrawable {
        public CubedEmote Emote => Drawable as CubedEmote;

        public CubedSkinnableEmote(CubedEmoteLookup lookup, Func<CubedEmote> createDefault)
            : base(lookup, (_ => createDefault()), ConfineMode.ScaleToFit) {}

        [BackgroundDependencyLoader]
        private void load() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public void Play() => Emote.Play();
    }
}
