using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Media;
using System.IO;
using Android.Database;
using Android.Provider;


namespace IANET_Marshall
{
    [Activity(Label = "HomeFragment")]
    public class HomeFragment : Fragment
    {
        WS_FileUpload.WS_FileUpload wsClientUpload = new WS_FileUpload.WS_FileUpload();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.HomeFragment, container, false);
            Button uploadButton = view.FindViewById<Button>(Resource.Id.buttonUploadPictures);
            uploadButton.Click += delegate
            {
                var imageIntent = new Intent();
                imageIntent.SetType("image/*");
                imageIntent.PutExtra(Intent.ExtraAllowMultiple, true);
                imageIntent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(
                    Intent.CreateChooser(imageIntent, "Select photo"), 0);
            };
            return view;//base.OnCreateView (inflater.Inflate(Resource.Layout.homeLayout, container, savedInstanceState);
        }
        public override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            string path = "";
            var paths = new List<string>();
            try
                {
                    if (resultCode == Result.Ok)
                    {
                        
                        if (intent != null)
                        {
                            ClipData clipData = intent.ClipData;
                            if (clipData != null)
                            {

                                for (int i = 0; i < (clipData.ItemCount-1); i++)
                                {
                                    ClipData.Item item = clipData.GetItemAt(i);
                                    global::Android.Net.Uri uri = item.Uri;

                                    //In case you need image's absolute path
                                    path = GetPathFromURI(uri);
                                    paths.Add(path);
                                }

                            }
                            else
                            {
                                global::Android.Net.Uri uri = intent.Data;
                                path = GetPathFromURI(uri);
                                paths.Add(path);
                            }

                          }
                    }
                    //Send the paths to forms
                }
                catch (Exception ex)
                {

                    //Toast.MakeText (Xamarin.Forms.Forms.Context, "Unable to open, error:" + ex.ToString(), ToastLength.Long).Show ();
                }
                Intent uploadIntent = new Intent(this.Activity, typeof(FileUploaderService));
                uploadIntent.PutStringArrayListExtra("paths", paths);
                this.Activity.StartService(uploadIntent);
                

                // In Progress Loop

               


                


            
        }

        //private string GetImageName(string path)
        //{
        //    string name = null;
        //    char[] splitchar = { '/'};
        //    string[] splitString=path.Split(splitchar);
        //    for(int i=0 ; i<splitString.Length; i++)
        //    {
        //        if (i == (splitString.Length - 1))
        //        {
        //            name=splitString[i];
        //        }
                
        //    }
        //    return name;
            
        //}

        //private bool UploadSmallFiles(string path, System.IO.FileStream stream,string filename)
        //{
        //    try
        //    {
        //        byte[] array = new byte[((int)stream.Length) + 1];
        //        stream.Read(array, 0, (int)stream.Length);
        //        StringBuilder builder = new StringBuilder(2 * array.Length);
        //        int num2 = array.Length - 1;
        //        for (int i = 0; i <= num2; i++)
        //        {
        //            builder.AppendFormat("{0:X2} ", array[i]);
        //        }
        //        string picString = builder.ToString().Replace(" ", string.Empty);
        //        var flag = wsClientUpload.ReceivePictureToAssignmentID_New(picString, "dave.ptasienski@ianetwork.net", "graph1cexternal3", filename,"650121",650121,"",1,3);
               
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        private string GetPathFromURI(Android.Net.Uri contentURI)
        {
            ICursor cursor = Application.Context.ContentResolver.Query(contentURI, null, null, null, null);
            cursor.MoveToFirst();
            string documentId = cursor.GetString(0);
            documentId = documentId.Split(':')[1];
            cursor.Close();

            cursor = Application.Context.ContentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new[] { documentId }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }
    }
}