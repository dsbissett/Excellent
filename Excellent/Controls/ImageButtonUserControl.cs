namespace Excellent.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ImageButtonUserControl : Button
    {
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(ImageButtonUserControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ButtonImageProperty =
            DependencyProperty.Register("ButtonImage", typeof(ImageSource), typeof(ImageButtonUserControl));

        public static readonly DependencyProperty AudioCommandProperty =
            DependencyProperty.Register("AudioCommand", typeof(ICommand), typeof(ImageButtonUserControl));

        public static readonly DependencyProperty AudioCommandParameterProperty =
            DependencyProperty.Register("AudioCommandParameter", typeof(object), typeof(ImageButtonUserControl));

        [Category("Common Properties")]
        public string ButtonText
        {
            get => (string) this.GetValue(ButtonTextProperty);
            set => this.SetValue(ButtonTextProperty, value);
        }

        [Category("Common Properties")]
        public ImageSource ButtonImage
        {
            get => (ImageSource)this.GetValue(ButtonImageProperty);
            set => this.SetValue(ButtonImageProperty, value);
        }

        [Bindable(true)]
        [Category("Action")]
        public ICommand AudioCommand
        {
            get => (ICommand) this.GetValue(AudioCommandProperty);
            set => this.SetValue(AudioCommandProperty, value);
        }

        [Bindable(true)]
        [Category("Action")]
        public object AudioCommandParameter
        {
            get => (object) this.GetValue(AudioCommandParameterProperty);
            set => this.SetValue(AudioCommandParameterProperty, value);
        }
    }
}