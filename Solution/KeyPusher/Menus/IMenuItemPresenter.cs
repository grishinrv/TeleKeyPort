namespace KeyPusher.Menus
{
    public interface IMenuItemPresenter
    {
        void ExecuteAction();
        void StateChanged();
        void CreateView();
        string ActionName { get; }
        byte? HotKeyCode { get; }
    }
}