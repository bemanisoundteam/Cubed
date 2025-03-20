using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Localisation.SkinComponents;
using osu.Game.Rulesets.Cubed.UI;
using osu.Game.Screens.Play;
using osu.Game.Screens.Play.HUD;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public partial class CubedKeyCounterDisplay : KeyCounterDisplay {
        private readonly CubedPanel panel = new() { RelativeSizeAxes = Axes.Both };
        // Users shouldn't be able to toggle the panel's cells on the display, as it's supposed to match the input state
        public override bool PropagatePositionalInputSubTree => false;

        #region Settings

        [SettingSource("Background Opacity")]
        public BindableFloat PanelOpacity { get; } = new () {
            Default = 0,
            MinValue = 0,
            MaxValue = 1,
            Precision = .01f
        };

        [SettingSource("Break Time Background Opacity")]
        public BindableFloat BreakOpacity { get; } = new (1) {
            MinValue = 0,
            MaxValue = 1,
            Precision = .01f
        };

        [SettingSource("Fade delay")]
        public BindableInt FadeDelay { get; } = new(500) {
            MinValue = 0,
            MaxValue = 500
        };

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.CornerRadius), nameof(SkinnableComponentStrings.CornerRadiusDescription))]
        public BindableFloat InnerRadius => panel.CellCornerRadius;

        [SettingSource("Cell border thickness")]
        public BindableFloat InnerThiccness => panel.CellBorderThickness;

        [SettingSource("Use smaller triangles")]
        public BindableBool UseSmallTriangles => panel.SmallTriangles;

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.Colour), nameof(SkinnableComponentStrings.ColourDescription))]
        public BindableColour4 Color => panel.AccentColor;

        #endregion

        protected override FillFlowContainer<KeyCounter> KeyFlow { get; } = new ();

        [BackgroundDependencyLoader(true)]
        private void load(Player player) {
            Content.AutoSizeAxes = AutoSizeAxes = Axes.None;
            Content.RelativeSizeAxes = Axes.Both;
            Size = new Vector2(200);

            Child = panel;
            // This is done here, as the value of IsBreakTime is unpredictable on map startup
            panel.Triangles.Alpha = panel.Background.Alpha = PanelOpacity.Value;

            PanelOpacity.ValueChanged += e => {
                if (player?.IsBreakTime.Value ?? false)
                    return;

                panel.Triangles.Alpha = panel.Background.Alpha = e.NewValue;
            };
            BreakOpacity.ValueChanged += e => {
                if (player?.IsBreakTime.Value ?? false)
                    panel.Triangles.Alpha = panel.Background.Alpha = e.NewValue;
            };

            // WARNING: This has to be scheduled to the next update
            // The obvious reason is thread safety (transforms must be made from the update thread)
            // But another important reason is that if it is run too early the transform will never happen
            // Don't ask me why I have no idea, just that it took me way too long to figure out
            player?.IsBreakTime.BindValueChanged(e => Scheduler.Add(FadePanel, e.NewValue));
        }

        private void FadePanel(bool isBreak) {
            float alpha = isBreak ? BreakOpacity.Value : PanelOpacity.Value;
            panel.Background.FadeTo(alpha, FadeDelay.Value, Easing.InOutQuad);
            panel.Triangles.FadeTo(alpha, FadeDelay.Value, Easing.InOutQuad);
        }

        protected override KeyCounter CreateCounter(InputTrigger trigger) {
            CubedKeyCounter keyCounter = new (trigger);
            CubedPanel.CubedPanelCell panelCell = panel.Cells[KeyFlow.Count / 4][KeyFlow.Count % 4];
            OsuSpriteText text = new() {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Font = OsuFont.Torus.With(size: 14 * 1.5f, weight: FontWeight.Bold),
                Y = -1
            };
            panelCell.Add(text);
            keyCounter.IsActive.BindTo(panelCell.Active);  // This is not useless, as this will update the activation count
            keyCounter.IsActive.ValueChanged += (e => text.FadeColour(e.NewValue ? Color4.Black : Color4.White, 50));
            trigger.ActivationCount.BindValueChanged(e => text.Text = e.NewValue.ToString(), true);
            return keyCounter;
        }

        // This never gets displayed, only used as a way to propagate events to my custom panel
        // This bypass is needed as I'm going against the principle of having an arbitrary amount of inputs
        // I do it this way as I do not except anyone else to use it, and Cubed always is a 4*4 panel
        // Should this assumption change this wouldn't be the only thing breaking in there
        private partial class CubedKeyCounter(InputTrigger trigger) : KeyCounter(trigger);
    }
}
