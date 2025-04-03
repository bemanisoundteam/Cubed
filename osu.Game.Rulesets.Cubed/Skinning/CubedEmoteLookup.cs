using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public record CubedEmoteLookup(string Name, string Namespace) : ISkinComponentLookup {
        public CubedSkinnableEmote CreateEmote() => new (this, CubedEmote.Emotes[this]);

        public override string ToString() => $"{Name} ({Namespace})";
    }
}
