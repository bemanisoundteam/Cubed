using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class DrawableCubedRuleset(CubedRuleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null, bool editor = false)
        : DrawableRuleset<CubedHitObject>(ruleset, beatmap, mods) {
        protected override Playfield CreatePlayfield() => new CubedPlayfield { isEditor = editor };

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new CubedPlayfieldAdjustmentContainer();

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new CubedFramedReplayInputHandler(replay);

        protected override ReplayRecorder CreateReplayRecorder(Score score) => new CubedReplayRecorder(score);

        public override DrawableHitObject<CubedHitObject> CreateDrawableRepresentation(CubedHitObject h) => null;

        protected override PassThroughInputManager CreateInputManager() => new CubedInputManager(Ruleset?.RulesetInfo);
    }
}
