using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using System.Linq;
using System.Threading.Tasks;

namespace GaiaDemo.Droid
{
    [Activity(Label = "GaiaDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const string TAG = "Main.Droid";

        private const int DemoPermissionsRequestedId = 1001;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();

            var appHasAllPermissions = CheckPermissions();
            if (!appHasAllPermissions)
            {
                Finish();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode != DemoPermissionsRequestedId )
            {
                return;
            }

            if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
            {

            }
            else
            {
                new AlertDialog.Builder(this, 5)
                    .SetTitle("Permissions required")
                    .SetMessage("Demo requires these permissions and cannot work without them.")
                    .SetCancelable(true)
                    .SetPositiveButton(Android.Resource.String.Ok, (sender, e) => { Finish(); })
                    .Show();
            }
        }

        private bool CheckPermissions()
        {
            string[] reqPerimissions =
            {
                Manifest.Permission.BluetoothAdmin,
                Manifest.Permission.AccessCoarseLocation
            };

            if ((int) Build.VERSION.SdkInt >= 23)
            {
                var perimissionsToReq = reqPerimissions
                    .Where(permission => CheckSelfPermission(permission) != Permission.Granted).ToList();

                if (perimissionsToReq.Count > 0)
                {
                    RequestPermissions(perimissionsToReq.ToArray(), DemoPermissionsRequestedId);
                    return false;
                }
            }

            return true;
        }
    }
}