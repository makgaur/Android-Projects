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
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace IANET_Marshall
{
    [Activity(Label = "MessageFragment")]
    public class MessageFragment : Fragment
    {
        //HttpClient client = new HttpClient();
        //static HttpClient client = new HttpClient();

        public class Employee
        {
            public int EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public DateTime JoiningDate { get; set; }
            public int Age { get; set; }
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.MessageFragment, container, false);

            Button buttonJson = view.FindViewById<Button>(Resource.Id.buttonJson);
            buttonJson.Click += async delegate
            {
                //try
                //{
                //    using (var client = new HttpClient())
                //    {
                //        // send a GET request  
                //        var uri = "http://localhost:13805/api/employees";
                //        var result = await client.GetStringAsync(uri);


                //        var posts = JsonConvert.DeserializeObject<List<Employee>>(result);


                //        var post = posts.First();

                //    }
                //}
                //catch (Exception ex)
                //{

                //}

                // Task.Run(()=>GetData(1) ).Wait();
                var result = GetData(1);


            };
            return view;//base.OnCreateView (inflater.Inflate(Resource.Layout.homeLayout, container, savedInstanceState);
        }
        //public string DownloadData() {
        //    var request = HttpWebRequest.Create("http://localhost:13805/api/employees");
        //    request.ContentType = "application/json";
        //    request.Method = "GET";
        //    string result = string.Empty;
        //    using(HttpWebResponse response=request.GetResponse() as HttpWebResponse)
        //    {
        //        if(response.StatusCode != HttpStatusCode.OK)
        //        {

        //        }
        //        using(StreamReader reader=new StreamReader(response.GetResponseStream()))
        //        {
        //            var content = reader.ReadToEnd();
        //            if(string.IsNullOrWhiteSpace(content))
        //            {

        //            }
        //            else
        //            {
        //                result = content;
        //            }
        //        }
        //    }
        //    return result;

        // }

        static HttpClient client = new HttpClient();
        //public static async Task<Employee> GetData(int id)
        //{
            
        //    //using (HttpClient client = new HttpClient())
        //    //{
        //    client.BaseAddress = new Uri("http://localhost:13805/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    Console.WriteLine("Start");
        //    Console.ReadLine();
        //    HttpResponseMessage result = await client.GetAsync(string.Format("/api/employees/GetEmployee/{0}", id));
            
        //    Console.WriteLine("Success");
        //    var result1 = JsonConvert.DeserializeObject<Employee>(result.Content.ReadAsStringAsync().Result);
        //    Console.ReadLine();
        //    return result1;
        //    //}
        //}

        public Employee GetData(int id)
        {

            //using (HttpClient client = new HttpClient())
            //{
            string uri = "http://google.com";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Console.WriteLine("Start");
            Console.ReadLine();
            Task<string> result = client.GetStringAsync(uri);
            Console.WriteLine("Success");
            while (result.Status != TaskStatus.RanToCompletion)
            {
                Console.WriteLine("Thread ID: {0}, Status: {1}", Thread.CurrentThread.ManagedThreadId, result.Status);
                Task.Delay(100).Wait();
            }
            var result1 = JsonConvert.DeserializeObject<Employee>(result.Result);
            Console.ReadLine();
            return result1;
            //}
        }

        //public async Task<List<Employee>> RefreshDataAsync ()
        //{
        //    var Items= new List<Employee>();
        //      var uri = new Uri(string.Concat("http://localhost:13805/", "api/employees/"));
        //      var response = await client.GetAsync (uri);
        //      if (response.IsSuccessStatusCode) {
        //            var content = await response.Content.ReadAsStringAsync ();
        //            Items = JsonConvert.DeserializeObject <List<Employee>> (content);

        //        }
        //      return Items;
        //}

    }

}
