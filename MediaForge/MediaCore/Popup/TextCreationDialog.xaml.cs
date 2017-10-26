using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Graphics.Imaging;
using System.Threading.Tasks;
using System.Numerics;

// Документацию по шаблону элемента "Диалоговое окно содержимого" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaCore.Popup
{
    using Render.Text;

    public class FontSetting
    {
        public string FontFamilyName;
        public FontFamily FontFamily;
    }

    public sealed partial class TextCreationDialog : ContentDialog
    {
        public SoftwareBitmap Result { get; set; }
        TextRenderSettings settings = new TextRenderSettings();

        public TextCreationDialog()
        {
            this.InitializeComponent();
            Fonts.ItemsSource = CanvasTextFormat.GetSystemFontFamilies().Select(x => new FontSetting
            {
                FontFamilyName = x,
                FontFamily = new FontFamily(x)
            });     
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Result = await TextRender.RenderToSoftwareBitmap(canvas, settings);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var text = TextValue.Text;           

            if (string.IsNullOrEmpty(text))
                return;

            var cbi = TextRender.Render(canvas, settings);
            var session = args.DrawingSession;
            session.Clear(sender.ClearColor);
            session.DrawImage(cbi);            
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            settings.Text = TextValue.Text;
            canvas.Invalidate();
        }

        private void OnFontFamilyChange(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            settings.FontFamily = (cb.SelectedItem as FontSetting).FontFamily;
            canvas.Invalidate();
        }

        private void OnOutlineChange(object sender, RangeBaseValueChangedEventArgs e)
        {
            /*
            if (m_render_text_setting == null)
                return;

            m_render_text_setting.StrokeWidth = (float)e.NewValue;
            canvas.Invalidate();
            */
        }

        private void OnOutlineToggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var toggle = sender as ToggleSwitch;
            settings.OutlineTextSettings.Enabled = toggle?.IsOn ?? false;
            canvas.Invalidate();
        }
    }
}
