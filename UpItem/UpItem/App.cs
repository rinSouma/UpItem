using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace UpItem {
    public class App : Application {
        public static App Instance;
        public App() {
            Instance = this;

            MainPage = new NavigationPage(new InputPage());

        }
        public event Action ShouldTakePicture = () => { };

        public void EventDriven() {
            ShouldTakePicture();
        }

        public void SetPhotho(String path) {
            MessagingCenter.Send<App, string>(this, "showImage", path);
            //            (MainPage as InputPage).ShowImage(path);
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}
