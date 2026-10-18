using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Configuration;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPlayfield(bool isEditor = false) : Playfield {
        public const float CellScale = .9f;

        // Children of the playfield never get masked away, so we can skip this
        public override bool UpdateSubTreeMasking() => false;

        private readonly CubedCell[][] Cells = new CubedCell[4][];
        private readonly CubedEmotesHandler emotes = new ();
        private readonly Container cellGlowProxyContainer = new();

        private readonly BindableBool highlightCells = new(true);
        private readonly BindableBool drawCellBorders = new(true);

        [BackgroundDependencyLoader(true)]
        private void load(CubedRulesetConfigManager config) {
            if (!isEditor) {
                config?.BindWith(CubedRulesetSetting.HighlightCells, highlightCells);
                config?.BindWith(CubedRulesetSetting.CellBorders, drawCellBorders);
            }

            // Proxied here to render below the notes
            AddInternal(emotes.CreateProxy());

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

            // Used to have all cell glows in the same container, so that batching renders them in a single drawcall
            AddInternal(cellGlowProxyContainer);

            // This is placed here to catch inputs first, but as it has a proxy
            // It won't be rendered
            AddInternal(emotes);
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => GetCell(screenSpacePos) != null;

        public CubedCell GetCell(Vector2 pos) {
            foreach (CubedCell[] c in Cells)
                foreach (CubedCell cell in c)
                    if (cell.ReceivePositionalInputAt(pos))
                        return cell;

            return null;
        }

        public override void Add(HitObject haj) {
            var blahaj = (CubedHitObject) haj;
            Cells[blahaj.Row][blahaj.Column].Add(haj);
        }

        public override bool Remove(HitObject haj) {
            var blahaj = (CubedHitObject) haj;
            return Cells[blahaj.Row][blahaj.Column].Remove(haj);
        }

        public override void Add(DrawableHitObject haj) {
            var blahaj = (DrawableCubedHitObject) haj;
            Cells[blahaj.HitObject.Row][blahaj.HitObject.Column].Add(haj);
        }

        public override bool Remove(DrawableHitObject haj) {
            var blahaj = (DrawableCubedHitObject) haj;
            return Cells[blahaj.HitObject.Row][blahaj.HitObject.Column].Remove(haj);
        }
    }
}
