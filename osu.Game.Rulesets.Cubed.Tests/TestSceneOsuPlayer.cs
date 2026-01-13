// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Rendering;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Cubed.Tests {
    [TestFixture]
    public partial class TestSceneOsuPlayer : PlayerTestScene {
        [BackgroundDependencyLoader]
        private void load(IRenderer renderer) =>
            ConciergeIcon.EnsureWhitePixel(renderer);

        protected override Ruleset CreatePlayerRuleset() => new CubedRuleset();
    }
}
