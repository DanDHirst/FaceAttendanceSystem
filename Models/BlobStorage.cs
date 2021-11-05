using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace PersonRecog
{
    class BlobStorage
    {
        static string _connectionString =
            "DefaultEndpointsProtocol=https;AccountName=facerecogimages;AccountKey=oXU5YZZ8XmRK6nUsawxuUKHVFVat6HAOo+YbW80HDWyeWlzMIZG0MIfA78LuZVIC0p6TC2aoPPxnPibWHoutaA==;EndpointSuffix=core.windows.net";

        
        

        public static IEnumerable<string> GetBlobs(string name)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(name);
            var items = new List<string>();
            //https://facerog123.blob.core.windows.net/images
            foreach (var blobItem in containerClient.GetBlobs())
            {
                items.Add(blobItem.Name);
            }

            return items;
        }
        
        public static void UploadAFile(string aContainer, string filename)
        {
            BlobContainerClient container = new BlobContainerClient(_connectionString, (aContainer));
            
                container.CreateIfNotExists();
                container.SetAccessPolicy(PublicAccessType.Blob);

                var file = File.OpenRead("./wwwroot/image/" + filename);

                // Upload a couple of blobs so we have something to list
            container.UploadBlob(filename, file);

                 // List all the blobs
                List<string> names = new List<string>();
                foreach (BlobItem blob in container.GetBlobs())
                {
                    names.Add(blob.Name);
                }
                file.Close();




        }

        public static void DeleteAFile(string aContainer, string filename)
        {
            BlobContainerClient container = new BlobContainerClient(_connectionString, (aContainer));
            container.DeleteBlob(filename);
        }

    }
}
