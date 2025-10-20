namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCubedHoldHead(CubedHoldHead head) : DrawableCube(head) {
        public DrawableCubedHoldHead() : this(null) { }  // Required for pooling
    }
}
