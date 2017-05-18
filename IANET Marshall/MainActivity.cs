using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;

namespace IANET_Marshall
{
    [Activity(Theme = "@style/MyTheme.Base", Label = "IANET_Marshall", MainLauncher = true, Icon = "@drawable/ianet")]
    public class MainActivity : Activity
    {
        ProgressBar progressBar;
        TextView txtUserName; 
        TextView txtPassword;
        
        WS_FileUpload.WS_FileUpload wsClientUpload = new WS_FileUpload.WS_FileUpload();
        protected override void OnCreate(Bundle bundle)
        {
            Button btnLogin;
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            btnLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            btnLogin.Click += (sender, e) =>
            {

                progressBar.Visibility = ViewStates.Visible;
                login();
            };
            
        }

        public void login()
        {
            txtUserName = FindViewById<TextView>(Resource.Id.txtUserName); 
            txtPassword = FindViewById<TextView>(Resource.Id.txtPassword);
            bool valid = wsClientUpload.VerifyUserAppraiser("dave.ptasienski@ianetwork.net", "graph1cexternal3");
            
            if(valid)
            {
                var activityFunction = new Intent(this, typeof(HomeMainActivity));
                activityFunction.PutExtra("Username", txtUserName.Text);
                activityFunction.PutExtra("Password", txtPassword.Text);
                StartActivity(activityFunction);
            }
        }
    }
}

