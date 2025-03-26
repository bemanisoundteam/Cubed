using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.IO.Stores;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Beatmaps;
using osu.Game.Rulesets.Cubed.Edit;
using osu.Game.Rulesets.Cubed.Mods;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Scoring;
using osu.Game.Rulesets.Cubed.Skinning;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Screens.Ranking.Statistics;
using osu.Game.Skinning;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed {
    public partial class CubedRuleset : Ruleset {
        public override string Description => "Cubed";
        public override string ShortName => "cubedruleset";
        public override string PlayingVerb => "Tapping cubes";

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableCubedRuleset(this, beatmap, mods);

        public override ScoreProcessor CreateScoreProcessor() => new CubedScoreProcessor(this);

        public override HealthProcessor CreateHealthProcessor(double drainStartTime) => new CubedHealthProcessor();

        public override HitObjectComposer CreateHitObjectComposer() => new CubedHitObjectComposer(this);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new CubedBeatmapConverter(beatmap, this);

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new CubedDifficultyCalculator(RulesetInfo, beatmap);

        public override ISkin CreateSkinTransformer(ISkin skin, IBeatmap beatmap) => new CubedSkinTransformer(skin);

        public override IEnumerable<Mod> GetModsFor(ModType type) => type switch {
            ModType.DifficultyReduction => [
                new CubedModNoFail(),
                new MultiMod(new CubedModHalfTime(), new CubedModDaycore())
            ],

            ModType.DifficultyIncrease => [
                new MultiMod(new CubedModSuddenDeath(), new CubedModPerfect()),
                new MultiMod(new CubedModDoubleTime(), new CubedModNightcore()),
                new ModAccuracyChallenge()
            ],

            ModType.Automation => [
                new MultiMod(new CubedModAutoplay(), new CubedModCinema())
            ],

            ModType.Fun => [
                new MultiMod(new ModWindUp(), new ModWindDown()),
                new CubedModMuted(),
                new ModAdaptiveSpeed()
            ],

            _ => Array.Empty<Mod>()
        };

        public override StatisticItem[] CreateStatisticsForScore(ScoreInfo score, IBeatmap playableBeatmap) {
            IReadOnlyList<HitEvent> relevantHitEvents = score.HitEvents.Where(e => e.HitObject is Cube).ToList();

            return [
                new("Live reaction", () => new CubedResultsScreenEmote()),
                new ("Notes Distribution", () => new CubedMusicBar(relevantHitEvents, playableBeatmap) {
                    RelativeSizeAxes = Axes.X,
                }, true),
                new("Timing Distribution", () => new HitEventTimingDistributionGraph(relevantHitEvents) {
                    RelativeSizeAxes = Axes.X,
                    Height = 250
                }, true),
                new ("Statistics", () => new SimpleStatisticTable(2,[
                    new AverageHitError(relevantHitEvents),
                    new UnstableRate(relevantHitEvents)
                ]), true),
                new ("Per key results", () => new HackCubedPanelPerKeyRank(relevantHitEvents, CreateScoreProcessor) {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                }, true),
            ];
        }

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new [] {
            new KeyBinding(InputKey.Number4, CubedAction.X0Y0),
            new KeyBinding(InputKey.Number5, CubedAction.X1Y0),
            new KeyBinding(InputKey.Number6, CubedAction.X2Y0),
            new KeyBinding(InputKey.Number7, CubedAction.X3Y0),

            new KeyBinding(InputKey.R, CubedAction.X0Y1),
            new KeyBinding(InputKey.T, CubedAction.X1Y1),
            new KeyBinding(InputKey.Y, CubedAction.X2Y1),
            new KeyBinding(InputKey.U, CubedAction.X3Y1),

            new KeyBinding(InputKey.F, CubedAction.X0Y2),
            new KeyBinding(InputKey.G, CubedAction.X1Y2),
            new KeyBinding(InputKey.H, CubedAction.X2Y2),
            new KeyBinding(InputKey.J, CubedAction.X3Y2),

            new KeyBinding(InputKey.V, CubedAction.X0Y3),
            new KeyBinding(InputKey.B, CubedAction.X1Y3),
            new KeyBinding(InputKey.N, CubedAction.X2Y3),
            new KeyBinding(InputKey.M, CubedAction.X3Y3)
        };

        protected override IEnumerable<HitResult> GetValidHitResults() => [
            HitResult.Perfect,
            HitResult.Great,
            HitResult.Good,
            HitResult.Meh,
            HitResult.Miss
        ];

        public static ResourceStore<byte[]> CreateNamespacedResourceStore(string ns) =>
            new NamespacedResourceStore<byte[]>(new DllResourceStore(typeof(CubedRuleset).Assembly), "Resources/" + ns);

        public override Drawable CreateIcon() => new ConciergeIcon();

        static CubedRuleset() {
            CubedEmote.RegisterEmote(typeof(LarryEmote), LarryEmote.Trigger, LarryEmote.Quote);
        }

        // Leave this line intact. It will bake the correct version into the ruleset on each build/release.
        public override string RulesetAPIVersionSupported => CURRENT_RULESET_API_VERSION;

        // I'm VERY mad that I had to do this, but at least this shit works, and I'm never going to think about it again
        private partial class HackCubedPanelPerKeyRank(IReadOnlyList<HitEvent> hitEvents, Func<ScoreProcessor> createScoreProcessor) : CubedPanelPerKeyRank(hitEvents, createScoreProcessor) {
            protected override void Update() => Height = DrawWidth;
        }
    }
}
