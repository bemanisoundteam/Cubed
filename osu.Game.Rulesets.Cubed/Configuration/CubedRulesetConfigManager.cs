using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class CubedRulesetConfigManager : RulesetConfigManager<CubedRulesetSetting> {
        public CubedRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null) : base(settings, ruleset, variant) {
            InitializeSkinBindables();
        }

        protected override void InitialiseDefaults() {
            base.InitialiseDefaults();

            SetDefault(CubedRulesetSetting.HighlightCells, false);
            SetDefault(CubedRulesetSetting.CellBorders, false);

            SetDefault(CubedRulesetSetting.CurrentCellGlow, new CellGlowSkinInfo(null, "Cubed"));
            SetDefault(CubedRulesetSetting.CurrentGameplaySkin, new CubedGameplaySkinInfo(null, "Cubed"));
        }
    }

    public enum CubedRulesetSetting {
        HighlightCells,
        CellBorders,

        // Skin items
        CurrentCellGlow,
        CurrentGameplaySkin,
    }
}
