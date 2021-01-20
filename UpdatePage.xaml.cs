using InterfaceHund.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InterfaceHund
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdatePage : Page
    {
        private const string BASE_URL = @"http://localhost:53878/"; //Hunds/

        public UpdatePage()
        {
            this.InitializeComponent();
            LoadHund();
        }

        private async void Btn_Update(object sender, RoutedEventArgs e)
        {
            //kalla på metod som updaterar hund
            await UpdateeHund();
            var dialog = new MessageDialog("Hunden är sparad");
            await dialog.ShowAsync();
            LoadHund();
        }
        public async System.Threading.Tasks.Task UpdateeHund()
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                Hund UpdatedHund = (Hund)SelectHund.SelectedItem;
                UpdatedHund.Name = txtNamn.Text;
                UpdatedHund.Ras = txtRas.Text;

                string jsonString = JsonConvert.SerializeObject(UpdatedHund);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string URL = BASE_URL + "Hunds/" + UpdatedHund.Id; //WebApi behöver ha ID för den hund som ska updateras.
                var response = await httpClient.PutAsync(URL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(responseString);
            }
            catch (Exception e)
            {
                //TODO: Ge användaren error meaage tex att det inte går att visa.
                System.Diagnostics.Debug.WriteLine(e.ToString());

            }
        }

        private async void LoadHund()
        {
            try
            {
                string URL = BASE_URL + "Hunds";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var hundList = JsonConvert.DeserializeObject<List<Hund>>(content);

                    //Databind listan
                    SelectHund.ItemsSource = hundList;
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        private void SelectHund_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectHund.SelectedItem != null)
            {
                Hund selectedHund = (Hund)SelectHund.SelectedItem;
                txtNamn.Text = selectedHund.Name;
                txtRas.Text = selectedHund.Ras;

                string owners = "";
                foreach (var owner in selectedHund.Owners)
                {
                    owners += owner.Firstname + " " + owner.Lastname + " ";
                }
                txtOwner.Text = owners;
            }
        }

        #region Navigation
        //hem
        private void mnuMain_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        //Skapa
        private void mnuCreate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreatePage));
        }
        //Update
        private void mnuUpdate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdatePage));
        }
        //Delete
        private void mnuDelete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DeletePage));
        }
        //Tillbaka
        private void nvSample_BackRequested(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs args)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
        #endregion
    }
}
