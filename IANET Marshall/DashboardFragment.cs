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
using OxyPlot.Xamarin.Android;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

namespace IANET_Marshall
{
    [Activity(Label = "DashboardFragment")]
    public class DashboardFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.DashboardFragment, container, false);
            PlotView viewPlotAssignments = view.FindViewById<PlotView>(Resource.Id.plot_view);
            viewPlotAssignments.Model = CreatePlotModelAssignments();
            PlotView viewPlotCycle = view.FindViewById<PlotView>(Resource.Id.plot_view_cycle);
            viewPlotCycle.Model = CreatePlotModelCycle();
            return view;//base.OnCreateView (inflater.Inflate(Resource.Layout.homeLayout, container, savedInstanceState);
        }
        //private PlotModel CreatePlotModel()
        //{
        //    var plotModel = new PlotModel { Title = "OxyPlot Demo" };

        //    plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
        //    plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 10, Minimum = 0 });

        //    var series1 = new LineSeries
        //    {
        //        MarkerType = MarkerType.Circle,
        //        MarkerSize = 4,
        //        MarkerStroke = OxyColors.White
        //    };

        //    series1.Points.Add(new DataPoint(0.0, 6.0));
        //    series1.Points.Add(new DataPoint(1.4, 2.1));
        //    series1.Points.Add(new DataPoint(2.0, 4.2));
        //    series1.Points.Add(new DataPoint(3.3, 2.3));
        //    series1.Points.Add(new DataPoint(4.7, 7.4));
        //    series1.Points.Add(new DataPoint(6.0, 6.2));
        //    series1.Points.Add(new DataPoint(8.9, 8.9));

        //    plotModel.Series.Add(series1);

        //    return plotModel;
        //}
        private PlotModel CreatePlotModelAssignments()
        {
            var model = new PlotModel { Title = "Assignments Completed Monthly" };

            //generate a random percentage distribution between the 5
            //cake-types (see axis below)
            var rand = new Random();
            double[] cakePopularity = new double[5];
            //for (int i = 0; i < 5; ++i)
            //{
            //    cakePopularity[i] = rand.NextDouble();
            //}
            for (int i = 0; i < 5; ++i)
                cakePopularity[i] = rand.Next(1, 100);
            var sum = cakePopularity.Sum();

            var columnSeries = new ColumnSeries
            {
                ItemsSource = new List<ColumnItem>(new[]
                {
                        new ColumnItem{ Value =cakePopularity[0] },
                        new ColumnItem{ Value =cakePopularity[1] },
                        new ColumnItem{ Value =cakePopularity[2] },
                        new ColumnItem{ Value =cakePopularity[3] },
                        new ColumnItem{ Value =cakePopularity[4] }
                }),
                LabelPlacement = LabelPlacement.Inside,
                //LabelFormatString = "{0:.00}%",
                LabelFormatString = "{0}"
                
                
            };
            model.Series.Add(columnSeries);
            model.PlotAreaBorderColor = OxyColors.Transparent;
            
            //model.LegendBorder = OxyColors.Black;
            model.Axes.Add(new CategoryAxis
            {
                IsZoomEnabled=false,
                IsPanEnabled=false,
                MinorTickSize=0,
                MajorTickSize=0,

                AxislineStyle=LineStyle.Solid,
                Position = AxisPosition.Bottom,
                Key = "CakeAxis",
                ItemsSource = new[]
                {
                        "JAN",
                        "FEB",
                        "MAR",
                        "APR",
                        "JUN"
                },
                
                
            });
            model.Axes.Add(new LinearAxis { 
                Position = AxisPosition.Left, 
                TextColor = OxyColors.Transparent,
                MajorTickSize=0,
                MinorTickSize=0});
            return model;
        }
        public static PlotModel CreatePlotModelCycle()
        {
            var modelP1 = new PlotModel { Title = "Assignments Status" };
            modelP1.IsLegendVisible = true;
            modelP1.LegendPlacement = LegendPlacement.Outside;
            modelP1.LegendPosition = LegendPosition.BottomCenter;
            dynamic seriesP1 = new PieSeries { 
                
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.5,
                AngleSpan = 180,
                StartAngle = 180,
                InnerDiameter = 0.7,
                Diameter=1
            };

            seriesP1.Slices.Add(new PieSlice("15",15) { IsExploded = true, Fill = OxyColor.FromRgb(238,171,31) });
            seriesP1.Slices.Add(new PieSlice("6", 6) { IsExploded = false, Fill = OxyColor.FromRgb(255, 0, 0) });

            seriesP1.OutsideLabelFormat = "";
            seriesP1.TickHorizontalLength = 0.00;
            seriesP1.TickRadialLength = 0.00;
            
            modelP1.Series.Add(seriesP1);

            return modelP1;

        }
    }
}