using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using App1.MenuItems;
using App1.Views;
using App1.Classes;

namespace App1
{
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        public NavigationPage page;
        public MainPage()
        {
            InitializeComponent();
            App.MasterDetail = this;
            buildNavigation();
            // Initial navigation, this can be used for our home page
            NavigationPage page = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)));
            page.BarBackgroundColor = functions.navbarColour();
            navigationDrawerList.BackgroundColor = functions.sidebarBodyColour();
            backgroundColour.BackgroundColor = functions.sidebarHeaderColour();
            Detail = page;
            
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            NavigationPage page = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
            page.BarBackgroundColor = functions.navbarColour();
            navigationDrawerList.BackgroundColor = functions.sidebarBodyColour();
            backgroundColour.BackgroundColor = functions.sidebarHeaderColour();
            Detail = page;
            IsPresented = false;
            buildNavigation();
        }

        private void buildNavigation()
        {
            menuList = new List<MasterPageItem>();


            // Adding menu items to menuList and you can define title ,page and icon
            menuList.Add(new MasterPageItem() { Title = "Home", Icon = "home" + functions.getLogo() +".png", TargetType = typeof(HomePage) , colour = functions.textColour() , gapColour = functions.gapColour()});
            menuList.Add(new MasterPageItem() { Title = "Favorites", Icon = "favorite" + functions.getLogo() + ".png", TargetType = typeof(FavoritesPage), colour = functions.textColour(), gapColour = functions.gapColour() });
            menuList.Add(new MasterPageItem() { Title = "FAQ", Icon = "faq" + functions.getLogo() + ".png", TargetType = typeof(FAQPage), colour = functions.textColour(), gapColour = functions.gapColour() });
            menuList.Add(new MasterPageItem() { Title = "About us", Icon = "about" + functions.getLogo() + ".png", TargetType = typeof(AboutPage), colour = functions.textColour(), gapColour = functions.gapColour() });
            menuList.Add(new MasterPageItem() { Title = "Settings", Icon = "settings" + functions.getLogo() + ".png", TargetType = typeof(OptionsPage), colour = functions.textColour(), gapColour = functions.gapColour() });
            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;
        }
    }
}