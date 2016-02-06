using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FlawlessCowboy.View.CustomControls
{
    public partial class Entry : UserControl
    {

        public String Label
        {
            get { return (String)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string),
              typeof(Entry), new PropertyMetadata(""));

        public String Value
        {
            get { return (String)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string),
              typeof(Entry), new PropertyMetadata(""));

        public Entry()
        {
            Init();
        }

        protected virtual void Init()
        {
            this.InitializeComponent();
            container.DataContext = this;
        }
    }
}
