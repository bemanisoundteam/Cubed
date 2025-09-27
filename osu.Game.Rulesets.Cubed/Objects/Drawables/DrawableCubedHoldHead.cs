namespace osu.Game.Rulesets.Cubed.Objects.Drawables {
    public partial class DrawableCubedHoldHead : DrawableCube {
        public DrawableCubedHoldHead() : this(null) { }  // Required for pooling
        public DrawableCubedHoldHead(CubedHoldHead head) : base(head) { }
    }
}
