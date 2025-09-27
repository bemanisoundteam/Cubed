using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Cubed.UI;
using osuTK;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Objects.Drawables.Pieces {
    public partial class CubedHoldIndicator : CompositeDrawable {
        private const float LineThickness = .01f;

        #region Needed for custom Update logic

        // This contains multiple press times as a drawable can get recycled, but we still need correct values on rewind
        // This is built on the assumption that the same DHO is always used for the same HitObject,
        // if it turns out to be incorrect, making it static should be a quick fix
        private readonly Dictionary<CubedHoldNote, double> pressTimes = new ();

        // This is correct on rewind, it's set in SetObject(CubedHoldNote), which is called in OnApply() in DrawableHold
        private CubedHoldNote heldNote;
        // Same here, OnApply()'s logic updates the Direction and Distance Bindables
        private Vector2 PointerPosition;

        #endregion

        private readonly IBindable<HoldDirection> Direction;
        private readonly IBindableNumber<int> Distance;

        private readonly Drawable Pointer;
        private readonly Drawable Line;

        public CubedHoldIndicator(IBindable<HoldDirection> direction, IBindableNumber<int> distance) {
            RelativeSizeAxes = Axes.Both;

            direction.ValueChanged += DirectionChanged;
            distance.ValueChanged += DistanceChanged;

            Direction = direction;
            Distance = distance;

            // Direction's Bindable might not trigger update on first object if it's facing up
            // So these are default initialized to looking upwards values
            AddInternal(Line = new CubedHoldLine {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.TopCentre,
                Y = .5f,
                Rotation = 180,
                Height = 0,
                Width = LineThickness
            });
            AddInternal(Pointer = new Triangle {
                RelativeSizeAxes = Axes.Both,
                RelativePositionAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Rotation = 180
            });
        }

        private void DirectionChanged(ValueChangedEvent<HoldDirection> e) {
            Pointer.Rotation = (int) e.NewValue * 90 - 180;
            Line.Rotation = (int) e.NewValue * 90 - 180;

            if ((int) e.NewValue % 2 == 1)
                Line.Position = new Vector2(e.NewValue is HoldDirection.Left ? .5f : -.5f, 0);
            else
                Line.Position = new Vector2(0, e.NewValue is HoldDirection.Up ? .5f : -.5f);

            // Uncomment to make so instead of filling the hold it stops at it
            // Line.Position *= -1;

            // And if you instead want it to be centered on the hold:
            // Line.Position = Vector2.Zero

            UpdatePointerPosition();
        }

        private void DistanceChanged(ValueChangedEvent<int> e) {
            // The .5f is to make it render under the arrow
            Line.Height = (e.NewValue + .5f) / CubedPlayfield.CellScale;

            // If there shouldn't be a line, at least make it so it doesn't draw one
            if (e.NewValue == 0)
                Line.Height = 0;

            UpdatePointerPosition();
        }

        private void UpdatePointerPosition() {
            if ((int) Direction.Value % 2 == 0)
                PointerPosition = new Vector2(0, Distance.Value * (Direction.Value == HoldDirection.Up ? -1 : 1));
            else
                PointerPosition = new Vector2(Distance.Value * (Direction.Value == HoldDirection.Left ? -1 : 1), 0);

            // Scale up so that it aligns with cells
            PointerPosition /= CubedPlayfield.CellScale;
        }

        public void SetObject(CubedHoldNote hold) =>
            heldNote = hold;

        public void OnHit() =>
            // Dictionaries don't accept null keys, no need to worry about that
            pressTimes[heldNote] = Time.Current;


        // Transforms being a mess forced my hand
        protected override void Update() {
            double progress = 0;

            // Last check is to ensure not setting progress to less than 0,
            // Else the logic under would scale up instead of scale down.
            // It also kills the alignment of the line and pointer
            if (heldNote != null && pressTimes.TryGetValue(heldNote, out double pressedAt) && Time.Current >= pressedAt)
                progress = pressedAt >= heldNote.EndTime ? 1
                    : double.Min(1, (Time.Current - pressedAt) / (heldNote.EndTime - pressedAt));

            Line.Scale = new Vector2(1, 1f - (float) progress);
            Pointer.Position = PointerPosition * (1f - (float) progress);
        }
    }
}
