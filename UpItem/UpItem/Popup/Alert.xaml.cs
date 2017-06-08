using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;


namespace UpItem.Popup {
    public partial class Alert : PopupPage {
        private string returnKey;

        public Alert(string key, string title, string ok = "OK") {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            this.returnKey = key;
            this.Title.Text = title;
            this.OKButton.Text = ok;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed() {
            //base.OnBackButtonPressed();
            return true;
        }

        protected override bool OnBackgroundClicked() {
            // Return default value - CloseWhenBackgroundIsClicked
            return base.OnBackgroundClicked();
        }

        private void OnClose(object sender, EventArgs e) {
            var test = (Button)sender;
            MessagingCenter.Send<string>(test.Text, this.returnKey);
            PopupNavigation.PopAsync();
        }
    }
}
