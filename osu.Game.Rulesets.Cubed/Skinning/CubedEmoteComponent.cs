using Newtonsoft.Json.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Localisation.SkinComponents;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Cubed.UI.Emotes;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public partial class CubedEmoteComponent : CompositeDrawable, ISerialisableDrawable {
        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.SpriteName), nameof(SkinnableComponentStrings.SpriteNameDescription), SettingControlType = typeof(EmoteSelector))]
        public Bindable<CubedEmoteLookup> Emote { get; } = new() {
            // Defaulting to CubedEmote#EmptyEmote avoids a crash when resetting that setting
            Default = new CubedEmoteLookup(null, null)
        };

        public CubedEmoteComponent() {
            Size = new osuTK.Vector2(200);
            Emote.ValueChanged += e => InternalChild = e.NewValue.CreateEmote();
        }

        public void CopyAdjustedSetting(IBindable target, object source) {
            // Change this shit if I make another setting
            Bindable<CubedEmoteLookup> emoteSetting = (Bindable<CubedEmoteLookup>) target;

            emoteSetting.Value = source switch {
                CubedEmoteLookup lookup => lookup,
                JObject jObject => jObject.ToObject<CubedEmoteLookup>(),
                _ => throw new System.Exception("Emote setting source is not supported !")
            };
        }

        public bool UsesFixedAnchor { get; set; }

        private partial class EmoteSelector : SettingsDropdown<CubedEmoteLookup> {
            protected override void LoadComplete() =>
                Items = CubedEmote.Emotes.Keys;
        }
    }
}
