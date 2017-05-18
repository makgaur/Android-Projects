using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.App;


namespace IANET_Marshall
{
    [Activity(Label = "HomeMainActivity")]
    public class HomeMainActivity : AppCompatActivity
    {
        
        DrawerLayout drawerLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HomePage);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);


            // Initialize toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetTitle(Resource.String.app_name);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);

            // Attach item selected handler to navigation view
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            //Load default screen
            var ft = FragmentManager.BeginTransaction();
            ft.AddToBackStack(null);
            ft.Add(Resource.Id.HomeFrameLayout, new HomeFragment());
            ft.Commit();

        }
        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            FragmentTransaction ft = null;
            switch (e.MenuItem.ItemId)
            {
                    
                    
                case (Resource.Id.nav_home):
                    ft = FragmentManager.BeginTransaction();
                    ft.AddToBackStack(null);
                    ft.Replace(Resource.Id.HomeFrameLayout, new HomeFragment());
                    ft.Commit();
                    break;
                case (Resource.Id.nav_dashboard):
                    ft = FragmentManager.BeginTransaction();
                    ft.AddToBackStack(null);
                    ft.Replace(Resource.Id.HomeFrameLayout, new DashboardFragment());
                    ft.Commit();
                    break;

                case (Resource.Id.nav_messages):
                    ft = FragmentManager.BeginTransaction();
                    ft.AddToBackStack(null);
                    ft.Replace(Resource.Id.HomeFrameLayout, new MessageFragment());
                    ft.Commit();
                    break;

                case (Resource.Id.nav_itenary):
                    ft = FragmentManager.BeginTransaction();
                    ft.AddToBackStack(null);
                    ft.Replace(Resource.Id.HomeFrameLayout, new ItenaryFragment());
                    ft.Commit();
                    break;

               
            }
            // Close drawer
            drawerLayout.CloseDrawers();
        }
        
    }
}