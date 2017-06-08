using Xamarin.Forms;

namespace UpItem {
    public interface IToast {
        void Show(string message);
    }

    public interface CameraInterface {
        void BringUpCamera();
        void BringUpPhotoGallery();
    }
}
