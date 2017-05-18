using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Locations;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;

using Xamarin.Android;
using Newtonsoft.Json;
using IANET_Marshall.Model;
using Java.Text;
using Newtonsoft.Json.Linq;

namespace IANET_Marshall
{
    [Activity(Label = "ItenaryFragment")]
    public class ItenaryFragment : Fragment, IOnMapReadyCallback
    {

        static List<AssignmentsForMapModel> AssignmentDetails = new List<AssignmentsForMapModel>{
            new AssignmentsForMapModel{AssignId=1,AssignNo="120121",location=new LatLng(Convert.ToDouble(28.47865047158699), Convert.ToDouble(77.085120677948)),ItemChecked=true,resourceIconId=Resource.Drawable.A,DistanceFromSource=0},
            new AssignmentsForMapModel{AssignId=2,AssignNo="120122",location=new LatLng(Convert.ToDouble(28.468070),Convert.ToDouble(77.082436)),ItemChecked=true,resourceIconId=Resource.Drawable.B,DistanceFromSource=0},
            new AssignmentsForMapModel{AssignId=3,AssignNo="120123",location=new LatLng(Convert.ToDouble(28.471088),Convert.ToDouble(77.072523)),ItemChecked=true,resourceIconId=Resource.Drawable.C,DistanceFromSource=0},
            new AssignmentsForMapModel{AssignId=4,AssignNo="120124",location=new LatLng(Convert.ToDouble(28.490100),Convert.ToDouble(77.081707)),ItemChecked=true,resourceIconId=Resource.Drawable.D,DistanceFromSource=0},
            new AssignmentsForMapModel{AssignId=5,AssignNo="120125",location=new LatLng(Convert.ToDouble(28.492388), Convert.ToDouble(77.096362)),ItemChecked=true,resourceIconId=Resource.Drawable.E,DistanceFromSource=0}

        };
        static View view;
        private int layoutId;
        private GoogleMap GMap;
        WebClient webclient;
        
        
        public override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (view != null)
            {
                ViewGroup parent = (ViewGroup)view.Parent;
                if (parent != null)
                {
                    parent.RemoveView(view);
                }
            }

            try
            {
                if (layoutId == 0)
                    layoutId = Resource.Layout.ItenaryFragment;
                view = inflater.Inflate(layoutId, container, false);
            }
            catch (InflateException e)
            {

            }
            Context context = Activity;
            SetUpListOnDistance();
            SetUpMap();
            SetUpScrollView(context,AssignmentDetails);
            return view;//base.OnCreateView (inflater.Inflate(Resource.Layout.homeLayout, container, savedInstanceState);
        }

        private void SetUpListOnDistance()
        {   
            CalculateDistance();
            RearrangeList();
             
        }

        private void RearrangeList()
        {
            AssignmentDetails = AssignmentDetails.OrderBy(o => o.DistanceFromSource).ToList();
        }
        private async void CalculateDistance()
        {
            for(int i=1;i<AssignmentDetails.Count();i++)
            {
                string requestUrl = BuildDistanceURL(AssignmentDetails[0], AssignmentDetails[i]);
                string JSONDirectionResponse = await HttpRequest(requestUrl);
                JObject o = JObject.Parse(JSONDirectionResponse);
                float distance = new float();
                try
                {
                    distance = (int)o.SelectToken("routes[0].legs[0].distance.value");
                    AssignmentDetails[i].DistanceFromSource = distance;
                }
                catch (Exception ex)
                {

                }
            }
            
        }

        private string BuildDistanceURL(AssignmentsForMapModel origin, AssignmentsForMapModel destination)
        {
            var buildURL = "https://maps.googleapis.com/maps/api/directions/json?origin=" + origin.location.Latitude.ToString() + ',' + origin.location.Longitude.ToString() + "&destination=" + destination.location.Latitude.ToString() + ',' + destination.location.Longitude.ToString() + "&key=AIzaSyCH-woLzuavLaAgm6SvHzLdw3ZXkWVZpHY";
            return buildURL;
        }

        

        private LinearLayout SetUpScrollView(Context context,List<AssignmentsForMapModel> model)
        {
            LinearLayout linearLayoutView = view.FindViewById<LinearLayout>(Resource.Id.LinearLayoutCheckBox);
            
            foreach(var item in model)
            {
                
                try
                {
                    LinearLayout ll = new LinearLayout(context);
                    
                    ll.Orientation = Orientation.Horizontal;
                    //ll.SetPadding(10, 10, 10, 10);
                    LinearLayout.LayoutParams linearLayoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent, 50);
                    linearLayoutParams.SetMargins(10, 10, 10, 10);
                    ll.LayoutParameters = linearLayoutParams;
                    CheckBox checkbox = new CheckBox(context);
                    checkbox.Checked = item.ItemChecked;
                    checkbox.Id = item.AssignId;
                    checkbox.CheckedChange += checkbox_CheckedChange;
                    ImageView imageView = new ImageView(context);
                    imageView.SetImageResource(item.resourceIconId);

                    TextView textView = new TextView(context);
                    textView.Text = item.AssignNo;
                    ll.AddView(checkbox);
                    ll.AddView(imageView);
                    ll.AddView(textView);
                    linearLayoutView.AddView(ll);

                }
                catch (Exception ex)
                {

                }
            }

            
            return linearLayoutView;
            
        }

        private void checkbox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            GMap.Clear();
            var checkbox =sender as CheckBox;
            if (e.IsChecked == true)
            {
                GMap.Clear();
                foreach (var item in AssignmentDetails)
                {
                    if (checkbox.Id == item.AssignId)
                    {
                        item.ItemChecked = true;
                    }
                }
                SetUpMarkers();
                BuildStringandRequestForWaypoint();

            }
            else
            {
                GMap.Clear();
                foreach(var item in AssignmentDetails)
                {
                    if(checkbox.Id==item.AssignId)
                    {
                        item.ItemChecked = false;
                    }
                }
                SetUpMarkers();
                BuildStringandRequestForWaypoint();
            }  
        }

        private void SetUpMarkers()
        {
            int count = AssignmentDetails.Count;
            if (GMap != null)
            {
                
                GMap.Clear();
                foreach (var item in AssignmentDetails)
                {
                    if(item.ItemChecked==true)
                    {
                        MarkOnMap(item.AssignNo, item.location, item.resourceIconId);
                        
                    }
                    
                }
            }
            FnUpdateCameraPosition(AssignmentDetails[0]);
        }
        
        private void SetUpMap()
        {
            if (GMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.googlemap).GetMapAsync(this);
            }
        }

        private async void BuildstringandRequest()
        {
            string requestUrl = BuildDirectionURL(AssignmentDetails[0], AssignmentDetails[(AssignmentDetails.Count()-1)]);
            string JSONDirectionResponse = await HttpRequest(requestUrl);
            FnSetDirectionQuery(JSONDirectionResponse);
        }
        private async void BuildStringandRequestForWaypoint()
        {
            try
            {
                string requestUrl = BuildDirectionWaypointURL(AssignmentDetails);
                string JSONDirectionResponse = await HttpRequest(requestUrl);
                FnSetDirectionQuery(JSONDirectionResponse);
            }
            catch(Exception ex)
            {

            }
           
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;
            //this.GMap.MapClick += (sender, e) =>
            //{
            //    var wayPoint = e.Point;
            //    if (GMap != null)
            //    {
            //        GMap.Clear();
            //        MarkOnMap("Source", origLatLong, Resource.Drawable.MarkerSource);
            //        MarkOnMap("Destination", destLatLong, Resource.Drawable.MarkerDest);
            //        MarkOnMap("Waypoint", wayPoint, Resource.Drawable.MarkerDest);

            //    }
            //    BuildStringandRequestForWaypoint(wayPoint);
            //};
            SetUpMarkers();
            BuildStringandRequestForWaypoint();
        }
        public string BuildDirectionURL(AssignmentsForMapModel origin,AssignmentsForMapModel destination)
        {
            var buildUrl = "https://maps.googleapis.com/maps/api/directions/json?origin=" + origin.location.Latitude.ToString() + ',' + origin.location.Longitude.ToString() + "&destination=" + destination.location.Latitude.ToString() + ',' + destination.location.Longitude.ToString() + "&key=AIzaSyCH-woLzuavLaAgm6SvHzLdw3ZXkWVZpHY";
            return buildUrl;
        }
        public string BuildDirectionWaypointURL(List<AssignmentsForMapModel> model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("https://maps.googleapis.com/maps/api/directions/json?origin=" + model[0].location.Latitude.ToString() + ',' + model[0].location.Longitude.ToString() + "&destination=" + model[(model.Count - 1)].location.Latitude.ToString() + ',' + model[(model.Count - 1)].location.Longitude.ToString() + "&waypoints=");
            for (int i = 1; i <= model.Count - 2; i++)
            {
                if (model[i].ItemChecked == true)
                {
                    if (i < model.Count - 2)
                        sb.Append(model[i].location.Latitude.ToString() + ',' + model[i].location.Longitude.ToString() + '|');
                    else
                        sb.Append(model[i].location.Latitude.ToString() + ',' + model[i].location.Longitude.ToString());
                }
            }
            //for(int i=0;i<=wayPoints.Count-1;i++)
            //{
            //    if(i < wayPoints.Count-1)
            //        sb.Append(wayPoints[i].Latitude.ToString()+','+ wayPoints[i].Longitude.ToString()+'|');
            //    else
            //        sb.Append(wayPoints[i].Latitude.ToString() + ',' + wayPoints[i].Longitude.ToString());

            //}
            sb.Append("&key=AIzaSyCH-woLzuavLaAgm6SvHzLdw3ZXkWVZpHY");
            var rVal=sb.ToString();
            return rVal;
        }
        void MarkOnMap(string title, LatLng pos, int resourceId)
        {
            
                try
                {
                    MarkerOptions options = new MarkerOptions().SetPosition(pos).SetTitle(title).SetIcon(BitmapDescriptorFactory.FromResource(resourceId));

                    options.Draggable(true);
                    GMap.AddMarker(options);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
        }
        void FnUpdateCameraPosition(AssignmentsForMapModel pos)
        {
            try
            {
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(pos.location);
                builder.Zoom(12);
                builder.Bearing(45);
                builder.Tilt(10);
                CameraPosition cameraPosition = builder.Build();
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                GMap.AnimateCamera(cameraUpdate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
        }
        async Task<string> HttpRequest(string strUri)
        {
            webclient = new WebClient();
            string strResultData=string.Empty;
            try
            {
                strResultData = await webclient.DownloadStringTaskAsync(new Uri(strUri));
                Console.WriteLine(strResultData);
            }
            catch(Exception e)
            {
               
            }
            finally
            {
                if (webclient != null)
                {
                    webclient.Dispose();
                    webclient = null;
                }
            }

            return strResultData;
        }
        void FnSetDirectionQuery(string strJSONDirectionResponse)
        {
            var objRoutes = JsonConvert.DeserializeObject<GoogleDirectionClass>(strJSONDirectionResponse);
            //objRoutes.routes.Count  --may be more then one 
            if (objRoutes.routes.Count > 0)
            {
                string encodedPoints = objRoutes.routes[0].overview_polyline.points;

                var lstDecodedPoints = FnDecodePolylinePoints(encodedPoints);
                //convert list of location point to array of latlng type
                var latLngPoints = new LatLng[lstDecodedPoints.Count];
                int index = 0;
                foreach (Location loc in lstDecodedPoints)
                {
                    latLngPoints[index++] = new LatLng(loc.Latitude, loc.Longitude);
                }

                var polylineoption = new PolylineOptions();
                polylineoption.InvokeColor( Android.Graphics.Color.ParseColor("#40c4ff"));
                polylineoption.Geodesic(true);
                polylineoption.Add(latLngPoints);                
                //new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                //{
                //    InvokeOnMainThread(() =>
                //    {
                        
                //    });
                //})).Start();
                GMap.AddPolyline(polylineoption);
            }
        }
        List<Location> FnDecodePolylinePoints(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                return null;
            var poly = new List<Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    Location p = new Location(string.Empty);
                    p.Latitude = Convert.ToDouble(currentLat) / 100000.0;
                    p.Longitude = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return poly;
        }
    }
}