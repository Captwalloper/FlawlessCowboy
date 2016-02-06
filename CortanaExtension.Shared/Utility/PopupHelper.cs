using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace CortanaExtension.Shared.Utility
{
    public static class PopupHelper
    {
        public async static Task ShowPopup(string message)
        {
            var dialog = new MessageDialog(message);

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 0;

            await dialog.ShowAsync();
        }
    }
}
