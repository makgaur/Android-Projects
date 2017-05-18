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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace IANET_Marshall.Model
{
    public class AssignmentsForMapModel
    {
        public int AssignId { get; set; }
        public string AssignNo { get; set; }
        public LatLng location { get; set; }
        public bool ItemChecked { get; set; }
        public int resourceIconId { get; set; }
        public float DistanceFromSource { get; set; }

    }
}