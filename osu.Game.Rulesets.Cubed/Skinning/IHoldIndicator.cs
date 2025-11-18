using osu.Framework.Graphics;
using osu.Game.Rulesets.Cubed.Objects;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public interface IHoldIndicator : IDrawable {
        CubedHoldNote Object { get; set; }

        HoldDirection Direction { set; }

        int Length { set; }

        // This feeds the dict that is used for progress calculations
        void OnHit() => PressTimes[Object] = Time.Current;

        void Apply(CubedHoldNote ho) {
            Object = ho;
            Direction = ho.Direction;
            Length = ho.TailLength;
        }

        /// <summary>
        /// Used to keep track of hold press times for ComputeProgress
        /// Must be injected from depenencies to keep correctness on skin change
        /// </summary>
        protected Dictionary<CubedHoldNote, double> PressTimes { get; }

        /// <summary>
        /// Computes hold progress, ranging from 0 to 1
        /// </summary>
        /// <remarks>Starts at hold press</remarks>
        /// <returns>Hold progression, 0 if not started</returns>
        double ComputeProgress() {
            // Last check is to ensure not setting progress to less than 0,
            // Else the logic under would scale up instead of scale down.
            // It also kills the alignment of the line and pointer
            if (Object != null && PressTimes.TryGetValue(Object, out double pressedAt) && Time.Current >= pressedAt)
                return pressedAt >= Object.EndTime ? 1
                    : double.Min(1, (Time.Current - pressedAt) / (Object.EndTime - pressedAt));

            return 0;
        }
    }
}
