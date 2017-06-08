using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCLStorage;


namespace UpItem {
    /// <summary>
    /// ViewModelのBase
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value
            , [CallerMemberName] String propertyName = null) {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null) {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// メイン画面
    /// </summary>
    public class MainViewModel : BindableBase {
        #region "Definition"
        // view用
        #endregion

        #region "Property"

        #endregion

        public MainViewModel() {
            try {
                KeyPressCommandImg = new Command<string>(key => OnKeyPressCommandImg(key));
            } catch (Exception exception) {
                throw exception;
            }
        }

        public Command<string> KeyPressCommandImg { get; private set; }

        void OnKeyPressCommandImg(string key) {
            MessagingCenter.Send<MainViewModel>(this, "callImage");
        }
    }
}
