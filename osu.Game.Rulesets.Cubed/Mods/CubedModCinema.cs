using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Replays;
using osu.Game.Rulesets.Mods;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Mods {
    public class CubedModCinema : ModCinema<CubedHitObject> {
        public override ModReplayData CreateReplayData(IBeatmap beatmap, IReadOnlyList<Mod> mods)
            => new(new CubedAutoGenerator(beatmap).Generate(), new ModCreatedUser { Username = "Concierge" });
    }
}
