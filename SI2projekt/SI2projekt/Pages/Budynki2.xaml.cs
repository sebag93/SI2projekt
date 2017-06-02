using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SI2projekt.Pages
{
    public partial class Budynki2 : ContentPage
    {
        public class ViewModel
        {
            public string Key { get; set; }//pokazuje klucz wartosc 
            public string Value { get; set; }


            public ViewModel(string key, string value)
            {
                Key = key;
                Value = value;
            }
        }
        private static ObservableCollection<ViewModel>
                buldings = new ObservableCollection<ViewModel>();
        public Budynki2()
        {
            buldings.Clear();
            buldings.Add(new ViewModel("Budynek IIT", "50.012190, 22.673114")); //vievModel obsługa wszystkich akcji połaczonych z widokiem
            buldings.Add(new ViewModel("Budynek J1", "50.012720, 22.673061"));
            buldings.Add(new ViewModel("Budynek J2", "50.012551, 22.672189"));
            buldings.Add(new ViewModel("Budynek J3", "50.012379, 22.671331"));
            buldings.Add(new ViewModel("Budynek J4", "50.012203, 22.670478"));
            buldings.Add(new ViewModel("Hala sportowa", "50.013283, 22.673329"));
            buldings.Add(new ViewModel("Budynek P", "50.012032, 22.671494"));
            buldings.Add(new ViewModel("Budynek IEiZ", "50.012270, 22.672562"));
            buldings.Add(new ViewModel("Budynek A", "50.010273, 22.673660"));
            buldings.Add(new ViewModel("Budynek B", "50.009959, 22.673478"));
            buldings.Add(new ViewModel("Budynek C", "50.010065, 22.672634"));
            buldings.Add(new ViewModel("Budynek D", "50.010459, 22.672850"));
            InitializeComponent();
            if (App.GetApp().AppSettings.ContainsKey("colorschema") == false || (App.GetApp().AppSettings.ContainsKey("colorschema") == true && App.GetApp().AppSettings.Data["colorschema"] == "standard"))
            {
                schemastandard();
            }
            if (App.GetApp().AppSettings.ContainsKey("colorschema") == true && App.GetApp().AppSettings.Data["colorschema"] == "contrast")
            {
                schemacontrast();
            }
            lstBuldings.ItemsSource = buldings;
            BindingContext = this;
        }
        public void schemastandard()
        {
            this.BackgroundColor = Color.Black;
            lstBuldings.BackgroundColor = Color.Black;
            txtNiemaza.TextColor = Color.White;
        }
        public void schemacontrast()
        {
            this.BackgroundColor = Color.Yellow;
            lstBuldings.BackgroundColor = Color.Yellow;
            txtNiemaza.TextColor = Color.Black;

        }
        private void LstBuldings_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new Navigation(e.Item as ViewModel));
        }
    }
}
