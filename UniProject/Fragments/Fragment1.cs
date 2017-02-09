using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Support.V4.Widget;

using Android.Gms.Location;
using Android.Locations;
using Android.Gms.Maps.Model;

namespace UniProject.Fragments
{
    public class Fragment1 : Fragment, IOnMapReadyCallback
    {
        /*  Google Maps Fragment*/
        private GoogleMap mMap;

        /* Drawer VARS*/
        DrawerLayout LeftDrawerLayout;
        List<string> mLeftView = new List<string>();
        ArrayAdapter mLeftAdapter;
        ListView mLeftDraw;
        LatLng myPosition;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetupMap();
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            /*  Button bd = FindViewById<Button>(Resource.Id.button1);
            bd.Click += (object e, EventArgs s) => 
            {
               
            };*/

            LeftDrawerLayout = View.FindViewById<DrawerLayout>(Resource.Id.myDrawer);
            mLeftDraw = View.FindViewById<ListView>(Resource.Id.LeftDrawer);

            mLeftView.Add("..1..");
            mLeftView.Add("..2..");
            mLeftView.Add("..3..");

            mLeftAdapter = new ArrayAdapter(this.Context, Android.Resource.Layout.SimpleListItem1, mLeftView);
            mLeftDraw.Adapter = mLeftAdapter;
            Button Location = View.FindViewById<Button>(Resource.Id.button1);
            Location.Click += (object e, EventArgs s) =>
            {
                //centreMap();
                Activity.SetContentView(Resource.Layout.Main);
                MainActivity.MAIN.ReturnToMain();
            };
            //Button fragment = View.FindViewById<Button>(Resource.Id.m1_f1);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        


        public void OnMapReady(GoogleMap googleMap)
        {
            //mMap = googleMap;
            centreMap();
            

            LocationManager lm = (LocationManager)MainActivity.MAIN.FindSystemService("LocationService");
            mMap.MyLocationEnabled = true;
            Criteria crit = new Criteria();
            String provider = lm.GetBestProvider(crit, true);
            Location location = lm.GetLastKnownLocation(provider);
            Console.WriteLine("Finding location of  center map");
            if (location != null)
            {               
                double latitude = location.Latitude;
                double longitude = location.Longitude;

                myPosition = new LatLng(latitude, longitude);
                CameraUpdate yourLocation = CameraUpdateFactory.NewLatLngZoom(myPosition, 19);
                mMap.AnimateCamera(yourLocation);
            }

        }

        private void centreMap()
        {
            Console.WriteLine("Called center map");
           // mMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom()
        }

        protected void SetupMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);

            }
            else
            {

            }
        }

        private void Bd_Click(object sender, EventArgs e)
        {
            MainActivity.MAIN.ReturnToMain();
        }
    }
}