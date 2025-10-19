using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Skinning;
using System;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public partial class CubedSkinnableEmote(CubedEmoteLookup lookup, Func<ICubedEmote> createDefault)
        : SkinnableDrawable(lookup, _ => (Drawable) createDefault(), ConfineMode.ScaleToFit) {
        public ICubedEmote Emote => Drawable as ICubedEmote;

        protected override bool ApplySizeRestrictionsToDefault => true;

        [BackgroundDependencyLoader]
        private void load() {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        public void Play() => Emote.Fire(true);
    }
}
