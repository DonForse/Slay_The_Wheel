using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Settings.Primitives;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Settings {
    public class DefaultSettingsLayer : ISettings {
        public TernaryBoolean Enabled { 
            get { return TernaryBoolean.TRUE; } 
        }

        public TernaryBoolean DrawVerticalSeparator {
            get { return TernaryBoolean.TRUE; }
        }

        public TernaryBoolean DrawHorizontalSeparator {
            get { return TernaryBoolean.TRUE; }
        }

        public TernaryBoolean Expandable {
            get { return TernaryBoolean.TRUE; }
        }

        public TernaryBoolean CustomDrawer {
            get { return TernaryBoolean.TRUE; }
        }

        public TernaryBoolean CullingOptimization {
            get { return TernaryBoolean.TRUE; }
        }

        public TernaryBoolean CacheOptimization {
            get { return TernaryBoolean.TRUE; }
        }
    }
}