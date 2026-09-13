// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework;
using osu.Framework.Platform;
using osu.Game.Tests;
using System;
using System.Diagnostics.CodeAnalysis;

namespace osu.Game.Rulesets.Cubed.Tests {
    public static class VisualTestRunner {
        [STAThread]
        [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
        public static int Main(string [] args) {
            using (DesktopGameHost host = Host.GetSuitableDesktopHost(@"osu")) {
                host.Run(new OsuTestBrowser());
                return 0;
            }
        }
    }
}
