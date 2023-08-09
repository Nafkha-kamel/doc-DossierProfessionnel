using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TpModule4.services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TpModule4
{
    public partial class MainPage : ContentPage
    {
        private ITwitterService twitter = new TwitterService();

        public MainPage()
        {
            InitializeComponent();
            this.form.IsVisible = true;
            this.tweets.IsVisible = false;
        }
    
        public void Connection_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("cool !");
            string id = this.id.Text;
            string motDePasse = this.motDePasse.Text;
            bool sw = this.sw.IsToggled;
            Debug.WriteLine($"id = {id}");
            Debug.WriteLine($"mot de passe = {motDePasse}");
            Debug.WriteLine($"sw = {sw}");
            
            if(string.IsNullOrEmpty(id) || id.Length < 3)
            {
                this.error.Text = "id doit > 3";
                this.error.IsVisible = true;
                return;
            }
            if (string.IsNullOrEmpty(motDePasse) || motDePasse.Length < 6)
            {
                this.error.Text = "mot de passe doit > 6";
                this.error.IsVisible = true;
                return;
            }

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                this.error.Text = "pas de connexion internet !";
                this.error.IsVisible = true;
                return;
            }

            if (twitter.authenticate(id, motDePasse))
            {
                this.error.IsVisible = false;
                this.tweets.IsVisible = true;
                this.form.IsVisible = false;
            }else
            {
                this.error.Text = "Login/Mot de passe incorrect !";
                this.error.IsVisible = true;
                return;
            }
            
           
            

        }

    }
}
