using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;


namespace PersonRecog
{
    class Program
    {
        /*//setx AZURE_STORAGE_CONNECTION_STRING "DefaultEndpointsProtocol=https;AccountName=facerog123;AccountKey=ZMcDHj+rANtxRJBhAPkDy1UFDsB+PlS5kjxfWpj9ZDfD7vt+bIh/05oSRd+vfqgfCBtoOEKwmyLF+F5UQKC7Rw==;EndpointSuffix=core.windows.net"
        const string IMAGE_BASE_URL = "https://facerog123.blob.core.windows.net/images/";

        private const string IMAGE_NEW_URL = "https://facerog123.blob.core.windows.net/newimage/";
  
        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        const string SUBSCRIPTION_KEY = "b457acc304a143b090bd07412b2f3a28";
        const string ENDPOINT = "https://uksouth.api.cognitive.microsoft.com/";

        static async Task Main(string[] args)
        {
            const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;
            BlobStorage.UploadAFile("newimage","newPhoto.jpg");

            // Authenticate.
            IFaceClient client = FaceRecognition.Authenticate(ENDPOINT, SUBSCRIPTION_KEY);

            // Find Similar - find a similar face from a list of faces.
            FaceRecognition.FindSimilar(client, IMAGE_BASE_URL, IMAGE_NEW_URL, RECOGNITION_MODEL3).Wait();
            BlobStorage.DeleteAContainer("newimage");



        }*/



    }
}
