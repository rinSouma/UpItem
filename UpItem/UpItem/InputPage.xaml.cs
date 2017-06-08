using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using UpItem.Popup;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;

namespace UpItem {
    public partial class InputPage : ContentPage {
        private MainViewModel _vm;

        private const string PhotoMsg = "写真を撮影";
        private const string GalaryMsg = "ライブラリから選択";

        private const string DiagKey = "Diag";
        private const string DiagTitle = "進捗どうですか？";
        private const string DiagOK = "いいです";
        private const string DiagNG = "だめです";
        private const string AlertKey = "Alert";

        public InputPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this._vm = new MainViewModel();
            this.BindingContext = _vm;
            this.button1.Clicked += callDiag;
            this.button2.Clicked += callAlert;

            MessagingCenter.Subscribe<App, string>(this, "showImage", (sender, arg) => {
                ShowImage(arg);
            });

            MessagingCenter.Subscribe<MainViewModel>(this, "callImage", (arg) => {
                TakePhoto();
            });

            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (args) => {
                Device.BeginInvokeOnMainThread(() => {
                    //取得した画像データをViewImageに設定する
                    this.ViewImg1.Source = ImageSource.FromStream(() => new MemoryStream((byte[])args));
                    this.ViewImg1.WidthRequest = 200;
                    this.ViewImg1.HeightRequest = 200;
                });
            });

            MessagingCenter.Subscribe<string>(this, DiagKey, (args) => {
                var toast = DependencyService.Get<IToast>();
                toast.Show(args);
            });

            MessagingCenter.Subscribe<string>(this, AlertKey, (args) => {
                var toast = DependencyService.Get<IToast>();
                toast.Show(args + "!!");
            });
        }

        public void ShowImage(string filepath) {
            this.ViewImg1.Source = ImageSource.FromFile(filepath);
            this.ViewImg1.WidthRequest = 200;
            this.ViewImg1.HeightRequest = 200;
        }

        private void ClearSubscribe() {
            try {
                MessagingCenter.Unsubscribe<MainViewModel, string>(this, "EventDriven");
                MessagingCenter.Unsubscribe<byte[]>(this, "ImageSelected");
                MessagingCenter.Unsubscribe<string>(this, "Dialog_OKNG");
            } catch (Exception exception) {
                throw exception;
            }
        }

        async void TakePhoto() {
            Device.BeginInvokeOnMainThread(async () =>
            {
                //カメラかフォトギャラリーか選択する
                var action = await DisplayActionSheet("画像を追加", "キャンセル", null, GalaryMsg, PhotoMsg);

                if (action == GalaryMsg) {
                    DependencyService.Get<CameraInterface>().BringUpPhotoGallery();
                } else if (action == PhotoMsg) {
                    DependencyService.Get<CameraInterface>().BringUpCamera();
                }
            });
        }

        private async void callDiag(object sender, EventArgs e) {
            var page = new Dialog(DiagKey, DiagTitle, DiagOK, DiagNG);
            await Navigation.PushPopupAsync(page);
        }

        private async void callAlert(object sender, EventArgs e) {
            var page = new Alert(AlertKey, DiagTitle,  DiagNG);
            await Navigation.PushPopupAsync(page);
        }

    }
}
