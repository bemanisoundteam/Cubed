// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Cubed.Tests {
    public partial class TestSceneOsuGame : OsuTestScene {
        [BackgroundDependencyLoader]
        private void load() {
            Children = [
                new Box {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                }
            ];
            AddGame(new FreeSupporterHellYeah());
        }
    }

    public partial class FreeSupporterHellYeah : OsuGame {
        [BackgroundDependencyLoader]
        private void load() =>
            API.LocalUser.BindValueChanged(gimmeSupporter, true);

        private void gimmeSupporter(ValueChangedEvent<APIUser> e) =>
            // You wouldn't download a supporter tag
            API.LocalUser.Value.IsSupporter = true;
    }
}
