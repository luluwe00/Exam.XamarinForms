using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Droid
{
    [Activity(Label = "Exam", Icon = "@mipmap/icon", Theme = "@style/SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]

    public class SplashScreen : Activity
    {
        private static int PERMISSION_ALL = 0;
        private int SPLASH_DISPLAY_LENGTH = 3000;

        string[] PERMISSIONS = {
                Manifest.Permission.Internet,
                Manifest.Permission.WriteExternalStorage,
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.ReadPhoneState
        };
        private Handler h;
        private Runnable r;


        public static bool hasPermissions(Context context, string[] allPermissionNeeded)
        {
            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.M && context != null && allPermissionNeeded != null)
                foreach (string permission in allPermissionNeeded)
                    if (ActivityCompat.CheckSelfPermission(context, permission) != Permission.Granted)
                        return false;
            return true;
        }



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.SplashScreen);
            h = new Handler();


            if (!hasPermissions(this, PERMISSIONS))
            {
                ActivityCompat.RequestPermissions(this, PERMISSIONS, PERMISSION_ALL);
            }
            else
            {

                //new System.Threading.Thread(new ThreadStart(() =>
                //{
                // RunOnUiThread(() =>
                initialize();
                //        );
                //  })).Start();

                h.PostDelayed(r, SPLASH_DISPLAY_LENGTH);

            }
            // calling function for initializing db and other stuffs
            initialize();
        }

        void initialize()
        {
            r = new Runnable(() =>
            {
                var i = new Intent(this, typeof(MainActivity));
                StartActivity(i);
                Finish();
            });

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            int index = 0;
            Dictionary<string, int> PermissionsMap = new Dictionary<string, int>();
            foreach (string permission in permissions)
            {
                PermissionsMap.Add(permission, (int)grantResults[index]);
                index++;
            }

            if ((PermissionsMap[Manifest.Permission.WriteExternalStorage] != 0) || PermissionsMap[Manifest.Permission.ReadExternalStorage] != 0)
            {
                Toast.MakeText(this, "WRITE_EXTERNAL_STORAGE and READ_EXTERNAL_STORAGE permissions are a must", ToastLength.Long).Show();
                Finish();
            }
            else
            {
                initialize();
                h.PostDelayed(r, SPLASH_DISPLAY_LENGTH);
            }
        }

    }
}