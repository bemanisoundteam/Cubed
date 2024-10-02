namespace osu.Game.Rulesets.Cubed.Objects {
    public class Cube : CubedHitObject {
        public CubedAction Action { get => (CubedAction) (Column + Row * 4); }
    }
}
