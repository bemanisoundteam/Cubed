using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Online.Leaderboards;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Scoring;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPanelPerKeyRank : CubedPanel {
        private readonly ScoreProcessor[] scoreProcessors = new ScoreProcessor[16];

        public CubedPanelPerKeyRank(IReadOnlyList<HitEvent> hitEvents, Func<ScoreProcessor> createScoreProcessor) {
            for (int i = 0; i < 16; i++)
                scoreProcessors[i] = createScoreProcessor();

            foreach (HitEvent e in hitEvents)
                scoreProcessors[(int) (e.HitObject as CubedHitObject)!.Action].ApplyResult(
                    new(e.HitObject, e.HitObject.CreateJudgement()) { Type = e.Result });
        }

        [BackgroundDependencyLoader]
        private void load() => UpdateRanks();

        public void UpdateRanks() {
            for (int i = 0; i < 16; i++)
                Cells[i / 4][i % 4].Add(new DrawableRank(scoreProcessors[i].Rank.Value) {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new osuTK.Vector2(.8f)
                });
        }
    }
}
