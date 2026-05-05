namespace AssemblyEngine.GUI
{
    public class GUIWindow
    {
        public GUIWindow()
        {
            GUIManager.AddWindow(this);
        }
        public virtual void Draw()
        {

        }
        public void CloseWindow()
        {
            GUIManager.RemoveWindow(this);
        }
    }
}
