using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;

namespace osu.Game.Rulesets.Cubed.Tests {
    public class CubedShaderManager(IRenderer renderer, ShaderManager parent = null)
        : ShaderManager(renderer, CubedRuleset.CreateNamespacedResourceStore("Shaders")) {
        public override IShader GetCachedShader(string vertex, string fragment) =>
            base.GetCachedShader(vertex, fragment) ?? parent?.GetCachedShader(vertex, fragment);

        public override IShaderPart GetCachedShaderPart(string name) =>
            base.GetCachedShaderPart(name) ?? parent?.GetCachedShaderPart(name);

        public override byte[] GetRawData(string fileName) =>
            base.GetRawData(fileName) ?? parent?.GetRawData(fileName);
    }
}
