using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public record CubedEmoteLookup(string Name, string Namespace) : ISkinComponentLookup {
        public CubedSkinnableEmote CreateEmote() => new (this, CubedEmoteRegistry.Emotes[this]);

        public override string ToString() => !string.IsNullOrEmpty(Namespace)
            ? $"{Name} ({Namespace})"
            // Please don't use null/empty namespaces for actual emotes...
            // They are meant to be either for the EmptyEmote or for test scenarios
            : Name ?? "Nothing";
    }
}
