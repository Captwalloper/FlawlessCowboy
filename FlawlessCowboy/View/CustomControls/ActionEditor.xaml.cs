using CortanaExtension.Shared.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;
using CortanaExtension.Utility;
using CortanaExtension.Shared.Utility.Cortana;
using FlawlessCowboy.ViewModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FlawlessCowboy.View.CustomControls
{
    public sealed partial class ActionEditor : UserControl
    {
        public ObservableCollection<CortanaCommand> ComponentTasks
        {
            get { return (ObservableCollection<CortanaCommand>)GetValue(ComponentTaskProperty); }
            set { SetValue(ComponentTaskProperty, value); }
        }
        public static readonly DependencyProperty ComponentTaskProperty =
            DependencyProperty.Register("ComponentTasks", typeof(ObservableCollection<CortanaCommand>),
              typeof(ActionEditor), new PropertyMetadata(""));

        public CortanaCommand Task
        {
            get { return (CortanaCommand)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(CortanaCommand),
              typeof(ActionEditor), new PropertyMetadata(""));

        public ObservableCollection<CortanaCommand> AvailableTasks
        {
            get { return (ObservableCollection<CortanaCommand>)GetValue(AvailableTaskProperty); }
            set { SetValue(AvailableTaskProperty, value); }
        }
        public static readonly DependencyProperty AvailableTaskProperty =
            DependencyProperty.Register("AvailableTasks", typeof(ObservableCollection<CortanaCommand>),
              typeof(ActionEditor), new PropertyMetadata(""));






        private DragDropHelper<CortanaCommand> DDH;

        public ActionEditor()
        {
            this.InitializeComponent();
            container.DataContext = this;

            DDH = new DragDropHelper<CortanaCommand>(ActionSequence, ActionPicker);
        }

        private void SourceListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            ListView source = sender as ListView;
            if (source.Equals(ActionSequence)) {
                DDH = new DragDropHelper<CortanaCommand>(ActionSequence, ActionPicker);
            } else {
                DDH = new DragDropHelper<CortanaCommand>(ActionPicker, ActionSequence);
            }
            DDH.SourceListView_DragItemsStarting(sender, e);
        }

        private void TargetListView_DragOver(object sender, DragEventArgs e)
        {
            DDH.TargetListView_DragOver(sender, e);
        }

        private async void TargetListView_Drop(object sender, DragEventArgs e)
        {
            DDH.TargetListView_Drop(sender, e);
        }

        private void TargetListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            DDH.TargetListView_DragItemsStarting(sender, e);
        }

        private void TargetListView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            DDH.TargetListView_DragItemsCompleted(sender, args);
        }

        private void TargetTextBlock_DragEnter(object sender, DragEventArgs e)
        {
            DDH.TargetTextBlock_DragEnter(sender, e);
        }

        private async void TargetTextBlock_Drop(object sender, DragEventArgs e)
        {
            DDH.TargetTextBlock_Drop(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainPage)App.GetPage()).ViewModel.Confirm();
        }
    }
}
