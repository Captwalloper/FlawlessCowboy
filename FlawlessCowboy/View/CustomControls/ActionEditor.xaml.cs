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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FlawlessCowboy.View.CustomControls
{
    public sealed partial class ActionEditor : UserControl
    {
        public ObservableCollection<UserTask> ComponentTasks
        {
            get { return (ObservableCollection<UserTask>)GetValue(ComponentTaskProperty); }
            set { SetValue(ComponentTaskProperty, value); }
        }
        public static readonly DependencyProperty ComponentTaskProperty =
            DependencyProperty.Register("ComponentTasks", typeof(ObservableCollection<UserTask>),
              typeof(ActionEditor), new PropertyMetadata(""));

        public UserTask Task
        {
            get { return (UserTask)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(UserTask),
              typeof(ActionEditor), new PropertyMetadata(""));

        public ObservableCollection<UserTask> AvailableTasks
        {
            get { return (ObservableCollection<UserTask>)GetValue(AvailableTaskProperty); }
            set { SetValue(AvailableTaskProperty, value); }
        }
        public static readonly DependencyProperty AvailableTaskProperty =
            DependencyProperty.Register("AvailableTasks", typeof(ObservableCollection<UserTask>),
              typeof(ActionEditor), new PropertyMetadata(""));






        private DragDropHelper<UserTask> DDH;

        public ActionEditor()
        {
            this.InitializeComponent();
            container.DataContext = this;

            DDH = new DragDropHelper<UserTask>(ActionSequence, ActionPicker);
        }

        private void SourceListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            ListView source = sender as ListView;
            if (source.Equals(ActionSequence)) {
                DDH = new DragDropHelper<UserTask>(ActionSequence, ActionPicker);
            } else {
                DDH = new DragDropHelper<UserTask>(ActionPicker, ActionSequence);
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
    }
}
