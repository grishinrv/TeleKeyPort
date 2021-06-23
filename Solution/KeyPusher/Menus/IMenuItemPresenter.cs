using KeyPusher.Services;

namespace KeyPusher.Menus
{
    public interface IMenuItemPresenter
    {
        void ExecuteAction();
        void StateChanged(MenuController meniController)
    }
}