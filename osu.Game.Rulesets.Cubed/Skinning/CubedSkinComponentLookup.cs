using osu.Game.Skinning;

namespace osu.Game.Rulesets.Cubed.Skinning {
    public class CubedSkinComponentLookup(CubedSkinComponents component) : SkinComponentLookup<CubedSkinComponents>(component);

    public enum CubedSkinComponents {
        Marker,
        Receptor,
        Indicator,
        CellGlow
    }
}
