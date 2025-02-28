using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Scoring;
using osu.Game.Tests.Visual;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Tests.Components {
    public partial class TestSceneResultsScreenPerKeyScoring : OsuTestScene {

        [Test]
        public void PerfectPlay() => AddStep("Full perfect hits", () => createNewPanel(new List<HitEvent>()));

        [Test]
        public void RandomEvents() => AddStep("Make Random hits", () => {
            Random random = new();
            HitResult[] validResults = [HitResult.Perfect, HitResult.Miss];
            HitEvent[] hitEvents = new HitEvent[573];
            for (int i = 0; i < hitEvents.Length; i++)
                hitEvents[i] = new HitEvent(default, null,
                    validResults[(int) (random.NextSingle() * 1.2f)],
                    new Cube() { Action = (CubedAction) random.Next(16) },
                    null, null);
            createNewPanel(hitEvents);
        });

        private void createNewPanel(IReadOnlyList<HitEvent> events) =>
            Child = new CubedPanelPerKeyRank(events, new CubedRuleset().CreateScoreProcessor) { RelativeSizeAxes = Axes.Both };
    }
}
