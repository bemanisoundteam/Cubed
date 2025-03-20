using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osuTK;

namespace osu.Game.Rulesets.Cubed.UI {
    public partial class CubedPanelTooltip : VisibilityContainer, ITooltip<uint> {
        private readonly CubedPanel panel = new() {
            Size = new Vector2(180),
            SmallTriangles = new BindableBool(true)
        };

        public CubedPanelTooltip() {
            InternalChild = panel;
        }

        protected override void PopIn() => this.FadeIn(200, Easing.OutQuint);
        protected override void PopOut() => this.FadeOut(200, Easing.OutQuint);

        public void SetContent(uint content) => panel.PressKeys(content);

        public void Move(Vector2 pos) => Position = pos;
    }
}
