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
using System.Threading;
using Android.Database;
using Android.Provider;
using System.IO;
using Android.Support.V7.App;
using Android.Media;

namespace IANET_Marshall
{
    [Service]
    public class FileUploaderService : IntentService
    {
        WS_FileUpload.WS_FileUpload wsClientUpload = new WS_FileUpload.WS_FileUpload();
        public FileUploaderService () : base("FileUploaderService")
        {

        }
        protected override void OnHandleIntent (Android.Content.Intent intent)
        {
            //Start Notification
            var builder = new NotificationCompat.Builder(this);
            builder.SetContentTitle("Uploading Files");
            builder.SetContentText("Upload in progress...");
            builder.SetSmallIcon(Resource.Drawable.ianet);
            NotificationManager notificationManager = this.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(1337, builder.Build());
            Intent intentNoti = new Intent(this, typeof(HomeFragment));
            const int pendingIntentId = 0;
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, pendingIntentId, intentNoti, PendingIntentFlags.OneShot);
            //Perform Task
            var paths=intent.GetStringArrayListExtra("paths").ToList();
            
            
            //For Returning to Activity On Clicking Notification

            for (int i = 0; i < (paths.Count() - 1) ; i++ )
            {
                string filename = GetImageName(paths[i]);
                using (FileStream stream = new FileStream(paths[i], FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        bool flag = false;
                        builder.SetProgress(100, 100 * i / paths.Count(), false);
                        string output = String.Format("Uploading Image {0}/{1}", i + 1, (paths.Count() - 1));
                        builder.SetContentText(output);
                        builder.SetContentIntent(pendingIntent);
                        notificationManager.Notify(1337, builder.Build());

                        flag = UploadSmallFiles(paths[i], stream, filename);
                        
                        

                    }
                    catch (Exception ex)
                    {

                    }
                }


            }
            if (true)
            {
                builder.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));
                builder.SetContentText("File Upload Completed");
                builder.SetProgress(0, 0, false);
                builder.SetContentIntent(pendingIntent);
                notificationManager.Notify(1337, builder.Build());
            }
        }
        private string GetImageName(string path)
        {
            string name = null;
            char[] splitchar = { '/' };
            string[] splitString = path.Split(splitchar);
            for (int i = 0; i < splitString.Length; i++)
            {
                if (i == (splitString.Length - 1))
                {
                    name = splitString[i];
                }

            }
            return name;

        }

        private bool UploadSmallFiles(string path, System.IO.FileStream stream, string filename)
        {
            try
            {
                byte[] array = new byte[((int)stream.Length) + 1];
                stream.Read(array, 0, (int)stream.Length);
                StringBuilder builder = new StringBuilder(2 * array.Length);
                int num2 = array.Length - 1;
                for (int i = 0; i <= num2; i++)
                {
                    builder.AppendFormat("{0:X2} ", array[i]);
                }
                string picString = builder.ToString().Replace(" ", string.Empty);
                var flag = wsClientUpload.ReceivePictureToAssignmentID_New(picString, "dave.ptasienski@ianetwork.net", "graph1cexternal3", filename, "650121", 650121, "", 1, 3);
                if(flag)
                {
                    return true;
                    
                 }
                else
                {
                    return false;
                }
                
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
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