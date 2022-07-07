using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Org.Json;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Content;
using Android.Gms.Common.Apis;
using LostAndFoundPatras.Droid;

namespace LostAndFoundPatras.Droid
{
    [Activity(Label = "LostAndFoundPatras", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent serviceIntent;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            serviceIntent = new Intent(this, typeof(AndroidGetLocationService));
            SetServiceMethods();

            LoadApplication(new App());
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                GoogleManager.Instance.OnAuthCompleted(result);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void SetServiceMethods()
        {
            MessagingCenter.Subscribe<StartServiceMessage>(this, "ServiceStarted", message => {
                if (!IsServiceRunning(typeof(AndroidGetLocationService)))
                {
                    if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                    {
                        StartForegroundService(serviceIntent);
                    }
                    else
                    {
                        StartService(serviceIntent);
                    }
                }
            });

            MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message => {
                if (IsServiceRunning(typeof(AndroidGetLocationService)))
                    StopService(serviceIntent);
            });
        }

        private bool IsServiceRunning(System.Type cls)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(cls).CanonicalName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}