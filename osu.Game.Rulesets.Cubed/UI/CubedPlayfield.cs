using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Cubed.Configuration;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osuTK;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPlayfield : Playfield {
        public const float CellScale = .9f;

        public bool isEditor { get; init; }

        public override bool UpdateSubTreeMasking() => false;
        private readonly CubedCell[][] Cells = new CubedCell[4][];
        private readonly CubedEmotesHandler emotes = new ();
        private readonly Container cellGlowProxyContainer = new();

        [Cached]
        private readonly Dictionary<CubedHoldNote, double> PressTimes = new();

        private readonly BindableBool highlightCells = new(true);
        private readonly BindableBool drawCellBorders = new(true);

        [BackgroundDependencyLoader(true)]
        private void load(CubedRulesetConfigManager config, IBeatmap beatmap) {
            if (!isEditor) {
                config?.BindWith(CubedRulesetSetting.HighlightCells, highlightCells);
                config?.BindWith(CubedRulesetSetting.CellBorders, drawCellBorders);
            }

            if (beatmap != null)
                PressTimes.EnsureCapacity(beatmap.HitObjects.OfType<CubedHoldNote>().Count());

            // Proxied here to render below the notes
            AddInternal(emotes.CreateProxy());

            // Used to have all cell glows in the same container, so that batching renders them in a single drawcall
            AddInternal(cellGlowProxyContainer);

            for (int i = 0; i < 4; i++) {
                Cells[i] = new CubedCell[4];
                for (int j = 0; j < 4; j++)
                    AddNested(Cells[i][j] = new CubedCell {
                        Scale = new Vector2(CellScale),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,

                        Action = (CubedAction) (i * 4 + j),
                        HighlightCells = highlightCells,
                        CellBorders = drawCellBorders,

                        GlowProxyContainer = cellGlowProxyContainer,
                    });
            }

            AddInternal(new GridContainer {
                RelativeSizeAxes = Axes.Both,
                Content = Cells
            });
            // This is placed here to catch inputs first, but as it has a proxy
            // It won't be rendered
            AddInternal(emotes);
        }

        public CubedCell GetCell(Vector2 pos) {
            foreach (CubedCell[] c in Cells)
                foreach (CubedCell cell in c)
                    if (cell.ReceivePositionalInputAt(pos))
                        return cell;
            return null;
        }

        public override void Add(HitObject haj) {
            CubedHitObject blahaj = (haj as CubedHitObject)!;
            Cells[blahaj.Row][blahaj.Column].Add(haj);
        }

        public override bool Remove(HitObject haj) {
            CubedHitObject blahaj = (haj as CubedHitObject)!;
            return Cells[blahaj.Row][blahaj.Column].Remove(haj);
        }

        public override void Add(DrawableHitObject haj) {
            DrawableCubedHitObject blahaj = (haj as DrawableCubedHitObject)!;
            Cells[blahaj.HitObject.Row][blahaj.HitObject.Column].Add(haj);
        }

        public override bool Remove(DrawableHitObject haj) {
            DrawableCubedHitObject blahaj = (haj as DrawableCubedHitObject)!;
            return Cells[blahaj.HitObject.Row][blahaj.HitObject.Column].Remove(haj);
        }
    }
}
