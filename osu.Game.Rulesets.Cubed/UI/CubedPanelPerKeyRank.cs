using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Game.Online.Leaderboards;
using osu.Game.Overlays;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Game.Screens.SelectV2;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPanelPerKeyRank : CubedPanel {
        private readonly ScoreProcessor[] scoreProcessors = new ScoreProcessor[16];
        private readonly ScoreInfo scoreInfo;

        public CubedPanelPerKeyRank(IReadOnlyList<HitEvent> hitEvents, Func<ScoreProcessor> createScoreProcessor, ScoreInfo scoreInfo = null) {
            for (int i = 0; i < 16; i++) {
                scoreProcessors[i] = createScoreProcessor();
                if (scoreInfo != null)
                    scoreProcessors[i].Mods.Value = scoreInfo.Mods;
            }

            foreach (HitEvent e in hitEvents)
                scoreProcessors[(int) (e.HitObject as CubedHitObject)!.Action].ApplyResult(
                    new(e.HitObject, e.HitObject.CreateJudgement()) { Type = e.Result });

            this.scoreInfo = scoreInfo;
        }

        [BackgroundDependencyLoader]
        private void load() {
            UpdateRanks();

            foreach (CubedPanelCell[] cells in Cells)
                foreach (CubedPanelCell cell in cells)
                    cell.Active.Disabled = true;
        }

        public void UpdateRanks() {
            for (int i = 0; i < 16; i++)
                Cells[i / 4][i % 4].Add(new HoverableRank(scoreProcessors[i], scoreInfo) {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new osuTK.Vector2(.8f)
                });
        }

        private partial class HoverableRank : DrawableRank, IHasCustomTooltip<ScoreInfo> {
            [Resolved(CanBeNull = true)]
            private OverlayColourProvider colourProvider { get; set; }
            private readonly ScoreInfo scoreInfo = new ();

            public HoverableRank(ScoreProcessor scoreProcessor, ScoreInfo SourceScoreInfo) : base(scoreProcessor.Rank.Value) {
                scoreProcessor.PopulateScore(scoreInfo);

                // Not carried over by PopulateScore
                scoreInfo.Ruleset = scoreProcessor.Ruleset.RulesetInfo;
                scoreInfo.Date = DateTimeOffset.Now;
                if (SourceScoreInfo != null) {
                    scoreInfo.BeatmapInfo = SourceScoreInfo.BeatmapInfo;
                    scoreInfo.BeatmapHash = SourceScoreInfo.BeatmapHash;
                    scoreInfo.RealmUser = SourceScoreInfo.RealmUser;
                    scoreInfo.Mods = SourceScoreInfo.Mods;
                }
            }

            public ITooltip<ScoreInfo> GetCustomTooltip() => new BeatmapLeaderboardScore.LeaderboardScoreTooltip(colourProvider ?? new OverlayColourProvider(160));

            public ScoreInfo TooltipContent => scoreInfo;
        }
    }
}
