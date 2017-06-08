using UpItem.Droid;
using UpItem;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(Toast_Droid))]
[assembly: Xamarin.Forms.Dependency(typeof(CameraAndroid))]

namespace UpItem.Droid {
    class Toast_Droid : IToast {
        public void Show(string message) {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }


    public class CameraAndroid : CameraInterface {
        public CameraAndroid() {
        }

        // カメラインスタンスの起動
        public void BringUpCamera() {
            var intent = new Intent(MediaStore.ActionImageCapture);
            ((Activity)Forms.Context).StartActivityForResult(intent, 1);
        }

        // フォトストレージインスタンスの起動
        public void BringUpPhotoGallery() {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);

            ((Activity)Forms.Context).StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 1);
        }
    }
}