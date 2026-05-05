using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Cubed.Dependencies;
using osuTK;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class CellGlowPreviewer : Container {
        public Drawable CellGlow {
            get => InternalChild;
            set {
                InternalChild = value;
                InternalChild.RelativeSizeAxes = Axes.Both;
                InternalChild.Size = Vector2.One;
            }
        }

        protected CubedShaderManager Shaders;

        [Cached]
        private Bindable<Vector2> ptrPos = new ();

        private bool handlePtrPos = true;

        protected override bool OnMouseMove(MouseMoveEvent e) {
            if (handlePtrPos)
                ptrPos.Value = e.ScreenSpaceMousePosition;

            return handlePtrPos;
        }

        protected override void OnHoverLost(HoverLostEvent e) {
            if (handlePtrPos)
                ptrPos.SetDefault();
        }

        [BackgroundDependencyLoader(true)]
        private void load(Bindable<Vector2> parentPtrPos) {
            if (parentPtrPos != null) {
                ptrPos.BindTo(parentPtrPos);
                handlePtrPos = false;
            }
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) {
            var deps = new DependencyContainer(base.CreateChildDependencies(parent));
            deps.CacheAs<ShaderManager>(Shaders = new CubedShaderManager(deps.Get<IRenderer>(), deps.Get<ShaderManager>()));
            return deps;
        }
    }
}
