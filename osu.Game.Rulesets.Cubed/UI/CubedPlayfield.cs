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
    public partial class CubedPlayfield : Playfield {
        public const float CellScale = .9f;

        public bool isEditor { get; init; }

        public override bool UpdateSubTreeMasking() => false;
        private readonly CubedCell[][] Cells = new CubedCell[4][];
        private readonly CubedEmotesHandler emotes = new ();

        private readonly BindableBool highlightCells = new(true);

        [BackgroundDependencyLoader(true)]
        private void load(CubedRulesetConfigManager config) {
            if (!isEditor)
                config?.BindWith(CubedRulesetSetting.HighlightCells, highlightCells);

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
