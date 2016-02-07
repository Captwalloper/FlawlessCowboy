using CortanaExtension.Shared.Model;
using CortanaExtension.Shared.Utility.Cortana;
using FlawlessCowboy.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlawlessCowboy.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterPage : Page, IAppPage
    {
        public MasterViewModel ViewModel;

        public MasterPage()
        {
            this.InitializeComponent();
            ViewModel = new MasterViewModel(this);
            this.DataContext = ViewModel;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            ViewModel.OnGotFocus();
        }

        async Task IAppPage.RespondToVoice(CortanaCommand command)
        {
            await ViewModel.RespondToVoice(command);
        }

        private void CommandPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserCortanaCommand command = (UserCortanaCommand)CommandPicker.SelectedItem;
            ViewModel.EditTheCommand(command);
        }
    }
}
