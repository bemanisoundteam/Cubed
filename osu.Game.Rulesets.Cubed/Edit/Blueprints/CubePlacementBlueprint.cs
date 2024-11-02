using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Edit;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public partial class CubePlacementBlueprint() : CubedPlacementBlueprint(new Cube()) {
        [BackgroundDependencyLoader]
        private void load(TextureStore textures) =>
            InternalChild = new Sprite { Texture = textures.Get("Cubed-logo") };

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (base.OnMouseDown(e) == false)
                return false;

            EndPlacement(true);
            return true;
        }

        public override void UpdateTimeAndPosition(SnapResult result) {
            base.UpdateTimeAndPosition(result);

            InternalChild.Alpha = 0;
            if (result.Playfield != null) {
                InternalChild.Alpha = 1;
                InternalChild.Size = result.Playfield.DrawSize;
                InternalChild.Scale = result.Playfield.Scale;
                InternalChild.Position = result.Playfield.ToSpaceOfOtherDrawable(result.Playfield.Position, this);
            }
        }
    }
}
