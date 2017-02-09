using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Java.Security;
using System;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;
using Java.Lang;
using Android.Content;
using Android.Runtime;
using Xamarin.Facebook.Login;
using System.Collections.Generic;
using Android.Support.V4.Widget;
using Android.Gms.Maps;
using Android.Locations;

namespace UniProject
{
    [Activity(Label = "UniProject", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity,IFacebookCallback
    {
        private ICallbackManager mCallBackManager;
        private TextView mText;
        private myProfileTracker mProfileTracker;

       

        private Profile currentProfile;

        public static MainActivity MAIN;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MAIN = this;
            LocationManager lm = (LocationManager)GetSystemService(LocationService);
            /* Used to get App manifest hash
             * 
                PackageInfo info = this.PackageManager.GetPackageInfo("UniProject.UniProject", PackageInfoFlags.Signatures);

                foreach(Android.Content.PM.Signature signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());

                    string keyHash = Convert.ToBase64String(md.Digest());
                    Console.WriteLine("KH: ", keyHash);
                }
                // Set our view from the "main" layout resource
                // SetContentView (Resource.Layout.Main);
            }*/


            // Init the facebook SDK
            FacebookSdk.SdkInitialize(this.ApplicationContext);
            mProfileTracker = new myProfileTracker();
            mProfileTracker.mOnProfileChanged += MProfileTracker_mOnProfileChanged;
            mProfileTracker.StartTracking();

            if (currentProfile == null)
                SetContentView(Resource.Layout.Main);
            else
                SetContentView(Resource.Layout.Fragment1);
            





            LoginButton button = FindViewById<LoginButton>(Resource.Id.login_button);
            button.SetReadPermissions("user_friends");
            mCallBackManager = CallbackManagerFactory.Create();
            button.RegisterCallback(mCallBackManager, this);
            mText = FindViewById<TextView>(Resource.Id.textView1);
            Button MainToFrag1 = FindViewById<Button>(Resource.Id.m1_f1);
            //Button Frag1ToMain = FindViewById<Button>(Resource.Id.button1);

            // Click Handler delegates //
            MainToFrag1.Click += (object e, EventArgs s) =>
            {
                SetContentView(Resource.Layout.Fragment1);
            };
          /*  Frag1ToMain.Click += (object e, EventArgs s) =>
            {
                SetContentView(Resource.Layout.Main);
            };a*/
        }

        public void ReturnToMain()
        {
            SetContentView(Resource.Layout.Main);
        }

        private void MProfileTracker_mOnProfileChanged(object sender, onProfileEventChangedArgs e)
        {
            if (e.mProfile != null) //login
            {
                mText.Text = "" + e.mProfile.FirstName + "|" + e.mProfile.LastName + "|" + e.mProfile.Id + "";
                currentProfile = e.mProfile;
                SetContentView(Resource.Layout.Fragment1);
            }
            else // log out
            {
                SetContentView(Resource.Layout.Main);
                currentProfile = null;
                // mText.Text = "" + e.mProfile.FirstName + "|" + e.mProfile.LastName + "|" + e.mProfile.Id + "";
            }
           // mText.SetText()
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            // mCallBackManager.OnActivityResult(requestCode, (int)requestCode, data);
            mCallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    

        public void OnCancel()
        {
            throw new NotImplementedException();
        }

        public void OnError(FacebookException p0)
        {
            FacebookException loginResult = p0 as FacebookException;
            Console.WriteLine(loginResult.ToString());
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            LoginResult loginResult = result as LoginResult;
            Console.WriteLine(loginResult.AccessToken.UserId);
        }

        protected override void OnDestroy()
        {
            mProfileTracker.StopTracking();
            base.OnDestroy();
        }

        public Java.Lang.Object FindSystemService(string type)
        {
           return GetSystemService(type);
        }
    }

    public class myProfileTracker : ProfileTracker
    {
        public event EventHandler<onProfileEventChangedArgs> mOnProfileChanged;
        protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
        {
            if (mOnProfileChanged != null)
            {
                mOnProfileChanged.Invoke(this, new onProfileEventChangedArgs(newProfile));
            }
        }
    }
    public class onProfileEventChangedArgs : EventArgs
    {
        public Profile mProfile;

        public  onProfileEventChangedArgs(Profile profile) { mProfile = profile; }
    }
}

