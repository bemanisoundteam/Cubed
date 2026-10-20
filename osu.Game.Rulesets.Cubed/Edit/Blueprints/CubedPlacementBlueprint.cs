using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Objects;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;
using osu.Game.Skinning;
using osuTK.Input;

namespace osu.Game.Rulesets.Cubed.Edit.Blueprints {
    public abstract partial class CubedPlacementBlueprint<T>(T hitObject) : HitObjectPlacementBlueprint(hitObject)
        where T : CubedHitObject
    {
        protected new T HitObject => hitObject;

        private CubedPlayfield playfield;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) {
            var dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            var playfieldDependencies = dependencies.Get<Playfield>().Dependencies;

            dependencies.CacheAs(playfieldDependencies.Get<ISkinSource>());
            dependencies.CacheAs(playfieldDependencies.Get<ICubedGameplaySkin>());
            dependencies.CacheAs(playfieldDependencies.Get<ICellGlowSkin>());

            return dependencies;
        }

        [BackgroundDependencyLoader]
        private void load(HitObjectComposer composer) =>
            playfield = (CubedPlayfield) composer.Playfield;

        protected CubedCell Cell {
            get;
            set {
                if (value == field)
                    return;

                field = value;
                if (field != null)
                    hitObject.Action = field.Action;
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e) {
            if (e.Button != MouseButton.Left || Cell == null)
                return false;

            BeginPlacement(true);
            return true;
        }

        public override bool ReplacesExistingObject(HitObject existing) =>
            base.ReplacesExistingObject(existing) && HitObject.Action == ((CubedHitObject) existing).Action;

        public override SnapResult UpdateTimeAndPosition(osuTK.Vector2 screenSpacePosition, double time) {
            base.UpdateTimeAndPosition(screenSpacePosition, time);
            var targetCell = playfield.GetCell(screenSpacePosition);

            if (PlacementActive == PlacementState.Waiting)
                Cell = targetCell;

            return new SnapResult(screenSpacePosition, time, targetCell);
        }
    }
}
