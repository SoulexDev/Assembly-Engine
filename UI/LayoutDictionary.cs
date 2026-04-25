namespace AssemblyEngine.UI
{
    internal class LayoutDictionary
    {
        private static DefaultLayoutProcessor defaultLayoutProcessor = new DefaultLayoutProcessor();
        private static CenterHorizontalLayoutProcessor centerHorizontalLayoutProcessor = new CenterHorizontalLayoutProcessor();

        private static FillHorizontalLayoutProcessor fillHorizontalLayoutProcessor = new FillHorizontalLayoutProcessor();

        public static Dictionary<LayoutType, ILayoutProcessor> layoutProcessors = new Dictionary<LayoutType, ILayoutProcessor>()
        {
            { LayoutType.None, defaultLayoutProcessor }, //done
            { LayoutType.CenterHorizontal, centerHorizontalLayoutProcessor }, //done, but still needs to be optimized
            { LayoutType.CenterVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.TopHorizontal, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.TopVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.BottomHorizontal, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.BottomVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.RightHorizontal, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.RightVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.LeftHorizontal, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.LeftVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.TopRightHorizontal, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.TopRightVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.TopLeftHorizontal, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.TopLeftVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.BottomRightHorizontal, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.BottomRightVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.BottomLeftHorizontal, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.BottomLeftVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.FillHorizontal, fillHorizontalLayoutProcessor }, //wip
            { LayoutType.FillVertical, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.FillHorizontalRight, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.FillVerticalRight, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.FillHorizontalLeft, centerHorizontalLayoutProcessor }, //unimplemented
            { LayoutType.FillVerticalLeft, centerHorizontalLayoutProcessor }, //unimplemented
        };
    }
}
