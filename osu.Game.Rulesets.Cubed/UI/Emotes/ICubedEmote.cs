using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Cubed.UI.Emotes {
    public interface ICubedEmote : IDrawable {
        /// <summary>
        /// Fire and forget call on an emote,
        /// Emote may do things like play a sample, or start animating.
        /// </summary>
        /// <param name="expire">Should Emote expire after being fired ?</param>
        public void Fire(bool expire);
    }
}
