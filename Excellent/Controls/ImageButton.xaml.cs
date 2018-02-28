namespace Excellent.Controls
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ImageButton.xaml
    /// </summary>
    public partial class ImageButton : ImageButtonUserControl
    {
        public ImageButton()
        {
            this.DataContext = this;

            InitializeComponent();            
        }
    }
}
