using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Objects.Drawables;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.Cubed.UI {
    [Cached]
    public partial class CubedPlayfield : Playfield {
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
