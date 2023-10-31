
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Settings.Primitives;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Settings {
    public interface ISettings {
        TernaryBoolean Enabled { get; }
        TernaryBoolean DrawVerticalSeparator { get; }
        TernaryBoolean DrawHorizontalSeparator { get; }
        TernaryBoolean Expandable { get; }
        TernaryBoolean CustomDrawer { get; }

        TernaryBoolean CullingOptimization { get; }
        TernaryBoolean CacheOptimization { get; }
    }
}