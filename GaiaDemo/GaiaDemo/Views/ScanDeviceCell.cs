using Xamarin.Forms;

namespace GaiaDemo
{
    public class ScanDeviceCell : ViewCell
    {
        public ScanDeviceCell()
        {
            var lblName = new Label();
            lblName.FontSize = 16;
            lblName.HorizontalOptions = LayoutOptions.Start;
            lblName.VerticalOptions = LayoutOptions.Center;
            lblName.VerticalTextAlignment = TextAlignment.Center;
            lblName.SetBinding(Label.TextProperty, new Binding("Name"));

            var layout = new StackLayout();
            layout.BackgroundColor = Color.AliceBlue;
            layout.HorizontalOptions = LayoutOptions.Fill;
            layout.Orientation = StackOrientation.Horizontal;

            layout.Children.Add(lblName);

            View = layout;
        }
    }
}
