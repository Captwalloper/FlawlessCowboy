using CortanaExtension.Shared.Model;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        public ActionEditor()
        {
            this.InitializeComponent();
            container.DataContext = this;
        }
    }
}
