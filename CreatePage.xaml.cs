using InterfaceHund.Model;
using InterfaceHund.ViewModel;
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
    public sealed partial class CreatePage : Page
    {
        private const string BASE_URL = @"http://localhost:53878/"; //Hunds/
        public CreatePage()
        {
            this.InitializeComponent();
            LoadOwner();
        }
        private async void Btn_Create(object sender, RoutedEventArgs e)
        {
            //kalla på metod som lägger till hund
            await AddHund();
            var dialog = new MessageDialog("Hunden är sparad");
            await dialog.ShowAsync();
            LoadOwner();

        }
        private async void LoadOwner()
        {
            try
            {
                string URL = BASE_URL + "Owners";
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ownerList = JsonConvert.DeserializeObject<List<Owner>>(content);

                    //Databind listan
                    ShowOwner.ItemsSource = GetPresentationsList(ownerList);
                    ShowOwner.SelectedIndex = 0;
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        private List<OwnerPresentation> GetPresentationsList (List<Owner> ownerList)
        {
            List<OwnerPresentation> returnList = new List<OwnerPresentation>();
            foreach (var owner in ownerList)
            {
                OwnerPresentation presentOwner = new OwnerPresentation { Id = owner.Id, Firstname = owner.Firstname, Lastname = owner.Lastname };
                returnList.Add(presentOwner);
            }
            return returnList;
        }
        public async System.Threading.Tasks.Task AddHund()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                OwnerPresentation selectedOwner = (OwnerPresentation)ShowOwner.SelectedItem;
                Owner hundowner = new Owner { Id = selectedOwner.Id, Firstname = selectedOwner.Firstname, Lastname = selectedOwner.Lastname };

                Hund newHund = new Hund();
                newHund.Name = Hnamn.Text;
                newHund.Ras = Hras.Text;
                newHund.Owners = new List<Owner>();
                newHund.Owners.Add(hundowner);

                string jsonString = JsonConvert.SerializeObject(newHund);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string URL = BASE_URL + "Hunds";
                var response = await httpClient.PostAsync(URL, content);
                var responseString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(responseString);
            }
            catch (Exception e)
            {
                //TODO: Ge användaren error meaage tex att det inte går att visa.
                System.Diagnostics.Debug.WriteLine(e.ToString());

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
