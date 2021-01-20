using InterfaceHund.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InterfaceHund
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string BASE_URL = @"http://localhost:53878/"; //Hunds/

        public MainPage()
        {
            this.InitializeComponent();
        }
        //Laddar varje gång användaren kommer till sidan.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadHundar();
        }

        #region Read
        public async void LoadHundar()
        {
            try
            {           
                 prgMain.IsActive = true; //ProgressRing visas tills webservices visas.
                 string URL = BASE_URL + "Hunds";
                 HttpClient httpClient = new HttpClient();
                 HttpResponseMessage response = await httpClient.GetAsync(new Uri(URL));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var hundList = JsonConvert.DeserializeObject<List<Hund>>(content);

                    //Databind listan
                    listhund.ItemsSource = hundList;
                    prgMain.IsActive = false;
                }
            }
            catch(Exception e)
            {
                //TODO: Ge användaren error meaage tex att det inte går att visa.
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        #endregion

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
