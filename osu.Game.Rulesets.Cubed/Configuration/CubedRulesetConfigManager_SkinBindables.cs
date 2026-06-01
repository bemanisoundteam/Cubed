using osu.Framework.Bindables;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;

namespace osu.Game.Rulesets.Cubed.Configuration {
    public partial class CubedRulesetConfigManager {
        private readonly Bindable<CubedGameplaySkin> gameplaySkin = new();
        private readonly Bindable<CellGlowSkin> cellGlowSkin = new();

        public IBindable<CubedGameplaySkin> GameplaySkin => gameplaySkin.GetBoundCopy();
        public IBindable<CellGlowSkin> CellGlowSkin => cellGlowSkin.GetBoundCopy();

        private void InitializeSkinBindables() {
            GetOriginalBindable<CubedGameplaySkinInfo>(CubedRulesetSetting.CurrentGameplaySkin).BindValueChanged(e => {
                gameplaySkin.Value?.Dispose();
                gameplaySkin.Value = e.NewValue.CreateSkin();
            }, true);
            GetOriginalBindable<CellGlowSkinInfo>(CubedRulesetSetting.CurrentCellGlow).BindValueChanged(e => {
                cellGlowSkin.Value?.Dispose();
                cellGlowSkin.Value = e.NewValue.CreateSkin();
            }, true);
        }
    }
}
