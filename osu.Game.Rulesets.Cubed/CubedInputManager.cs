using osu.Framework.Allocation;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using System.ComponentModel;

namespace osu.Game.Rulesets.Cubed {
    [Cached]  // Used for touch/mouse input
    public partial class CubedInputManager : RulesetInputManager<CubedAction> {
        public CubedInputManager(RulesetInfo ruleset) : base(ruleset, 0, SimultaneousBindingMode.Unique) {}
    }

    public enum CubedAction {
        [Description("01 - Row 1, Column 1")] X0Y0,
        [Description("02 - Row 1, Column 2")] X1Y0,
        [Description("03 - Row 1, Column 3")] X2Y0,
        [Description("04 - Row 1, Column 4")] X3Y0,

        [Description("05 - Row 2, Column 1")] X0Y1,
        [Description("06 - Row 2, Column 2")] X1Y1,
        [Description("07 - Row 2, Column 3")] X2Y1,
        [Description("08 - Row 2, Column 4")] X3Y1,

        [Description("09 - Row 3, Column 1")] X0Y2,
        [Description("10 - Row 3, Column 2")] X1Y2,
        [Description("11 - Row 3, Column 3")] X2Y2,
        [Description("12 - Row 3, Column 4")] X3Y2,

        [Description("13 - Row 4, Column 1")] X0Y3,
        [Description("14 - Row 4, Column 2")] X1Y3,
        [Description("15 - Row 4, Column 3")] X2Y3,
        [Description("16 - Row 4, Column 4")] X3Y3
    }
}
