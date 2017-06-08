// 埋め込み画像の表示

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpItem {
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider) {
            if (Source == null)
                return null;

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source);

            return imageSource;
        }
    }

    public class MyBoxView : BoxView {

        public Color StrokeColor { get; set; }  //ボーダ色
        public Color FillColor { get; set; }    //塗りつぶし色

        public int LineWidth { get; set; }      //ボーダの幅(0px～10px)
        public float Radius { get; set; }       //角丸(0%～50%)

        public MyBoxView(Color fillColor, Color strokeColor, int lineWidth, float radius) {
            FillColor = fillColor;
            StrokeColor = strokeColor;
            LineWidth = lineWidth;
            Radius = radius;

            //デフォルト値でサイズとレイアウトを設定
            WidthRequest = 100;
            HeightRequest = 100;
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
        }
    }
}
