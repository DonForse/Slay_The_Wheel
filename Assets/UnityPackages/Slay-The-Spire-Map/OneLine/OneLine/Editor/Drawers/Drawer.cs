using UnityEditor;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor {
    internal abstract class Drawer {

        protected SpaceDrawer space = new SpaceDrawer();
        protected SeparatorDrawer separator = new SeparatorDrawer();
        protected HeaderDrawer header = new HeaderDrawer();
        protected HighlightDrawer highlight = new HighlightDrawer();
        protected TooltipDrawer tooltip = new TooltipDrawer();

        public abstract void AddSlices(SerializedProperty property, Slices.Slices slices);

    }
}
