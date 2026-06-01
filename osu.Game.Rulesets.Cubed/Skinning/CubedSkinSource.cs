using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Game.Audio;
using osu.Game.Rulesets.Cubed.Configuration;
using osu.Game.Rulesets.Cubed.Skinning.CellGlows;
using osu.Game.Rulesets.Cubed.Skinning.Gameplay;
using osu.Game.Skinning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public sealed class CubedSkinSource : ISkinSource, ICubedGameplaySkin, ICellGlowSkin {
        #pragma warning disable CS8632  // Needed to mark Action nullable whilst not opening the NRT can-worm
        public event Action? SourceChanged;

        private readonly ISkinSource parent;
        private readonly IBindable<CubedGameplaySkin> currentGameplaySkin;
        private readonly IBindable<CellGlowSkin> currentCellGlow;

        public CubedSkinSource(ISkinSource parent, CubedRulesetConfigManager config) {
            Debug.Assert(parent != null);
            this.parent = parent;

            currentGameplaySkin = config.GameplaySkin;
            currentCellGlow = config.CellGlowSkin;

            currentGameplaySkin.ValueChanged += _ => SourceChanged?.Invoke();
            currentCellGlow.ValueChanged += _ => SourceChanged?.Invoke();

            parent.SourceChanged += TriggerSourceChange;
        }

        private void TriggerSourceChange() => SourceChanged?.Invoke();

        public Drawable GetDrawableComponent(ISkinComponentLookup lookup) {
            if (lookup is CubedSkinComponentLookup lkp)
                return lkp.Component switch {
                    CubedSkinComponents.CellGlow => currentCellGlow.Value.CreateCellGlow(),
                    _ => currentGameplaySkin.Value.CreateComponent(lkp.Component)
                };

            return parent.GetDrawableComponent(lookup);
        }

        public ISkin FindProvider(Func<ISkin, bool> lookupFunction) => AllSources.FirstOrDefault(lookupFunction);

        public IEnumerable<ISkin> AllSources => parent.AllSources.Prepend(this);

        Texture ICubedGameplaySkin.GetTexture(string name, WrapMode wrapModeS, WrapMode wrapModeT) => currentGameplaySkin.Value.GetTexture(name, wrapModeS, wrapModeT);

        Texture ICellGlowSkin.GetTexture(string name, WrapMode wrapModeS, WrapMode wrapModeT) => currentCellGlow.Value.GetTexture(name, wrapModeS, wrapModeT);

        #region Out of scope, stubbed to parent ISkinSource

        public IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
            where TLookup : notnull where TValue : notnull => parent.GetConfig<TLookup, TValue>(lookup);

        public Texture GetTexture(string componentName, WrapMode wrapModeS, WrapMode wrapModeT) => parent.GetTexture(componentName, wrapModeS, wrapModeT);

        public ISample GetSample(ISampleInfo sampleInfo) => parent.GetSample(sampleInfo);

        #endregion

        public void Dispose() {
            SourceChanged = null!;

            parent.SourceChanged -= TriggerSourceChange;
        }
    }
}
