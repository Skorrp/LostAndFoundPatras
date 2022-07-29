using LostAndFoundPatras.Models;
using LostAndFoundPatras.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LostAndFoundPatras.Content
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoundVehiclesPage : ContentPage
    {
        public FoundVehiclesPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<PetModel>(this, "LocationData", pet =>
            { if (pet.Latitude != null)
                {
                    ToBeRemoved.Text += $"{Environment.NewLine}{pet.Latitude}, {pet.Longitude}";
                    Console.WriteLine($"Hello {pet.Latitude}, {pet.Longitude}");
                }
                //MessagingCenter.Subscribe<string, string>("this", "abcd", (sender, message) => {
                //    ToBeRemoved.Text += $"{Environment.NewLine}{message}";
                // ΣΗΜΑΝΤΙΚΟ: Αρχικά, ο κώδικας παίρνει εντολή για εκτέλεση από εκεί που στέλνεται (εδώ όταν φορτώνεται η λίστα Lost).
                //            Το πρόβλημα, όμως, είναι ότι προκειμένου να εκτελεστεί ο κώδικας, χρειάζεται η σελίδα στην οποία θα εκτελεστεί
                //            να έχει φορτωθεί έστω μία φορά ώστε να αρχικοποιηθεί!!! Στην συγκεκριμένη περίπτωση ο κώδικας εκτελείται στο
                //            άνοιγμα της εφαρμογής και μόνο, ενώ η σελίδα ΑΥΤΗ ΠΟΥ ΒΡΙΣΚΟΜΑΣΤΕ δεν έχει φορτώσει! Λύση που ΔΕΝ προτείνεται
                //            είναι το Refresh!
                // ΕΠΙΣΗΣ: Επιστρεφεται μονο η τελευταια τιμη (αν κανουμε refresh). Ενδεχομένως να πρέπει να βάλουμε κάποια for με κάποιο counter
                //         μετά το subscription ώστε να καταχωρεί κάθε φορά την τιμή (σε μια λιστα?)
                //});
            });
                }
    }
}