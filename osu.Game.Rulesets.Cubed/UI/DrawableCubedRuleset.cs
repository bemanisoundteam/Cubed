using osu.Framework.Allocation;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Cubed.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.UI {
    [Cached]
    public partial class DrawableCubedRuleset : DrawableRuleset<CubedHitObject> {
        public DrawableCubedRuleset(CubedRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null) : base(ruleset, beatmap, mods) {}

        protected override Playfield CreatePlayfield() => new CubedPlayfield();

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new CubedPlayfieldAdjustmentContainer();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new CubedFramedReplayInputHandler(replay);

        public override DrawableHitObject<CubedHitObject> CreateDrawableRepresentation(CubedHitObject h) => new DrawableCubedHitObject(h);

        protected override PassThroughInputManager CreateInputManager() => new CubedInputManager(Ruleset?.RulesetInfo);
    }
}
