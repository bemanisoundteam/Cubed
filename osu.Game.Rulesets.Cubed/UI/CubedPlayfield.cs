using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osuTK;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPlayfield : Playfield {
        public override bool UpdateSubTreeMasking() => false;
        private readonly CubedCell[][] Cells = new CubedCell[4][];

        [BackgroundDependencyLoader]
        private void load() {
            for (int i = 0; i < 4; i++) {
                Cells[i] = new CubedCell[4];
                for (int j = 0; j < 4; j++)
                    AddNested(Cells[i][j] = new CubedCell() {
                        Scale = new Vector2(0.9f),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,

                        Action = (CubedAction) (i * 4 + j)
                    });
            }

            AddInternal(new GridContainer {
                RelativeSizeAxes = Axes.Both,
                Content = Cells
            });
        }

        public CubedCell GetCell(Vector2 pos) {
            foreach (CubedCell cell in Cells.SelectMany(c => c))
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
