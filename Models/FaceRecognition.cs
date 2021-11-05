using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace PersonRecog
{
    class FaceRecognition
    {
        public string result;
        public string name;
        public string url;
        public double confidence;
        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url,
            string recognition_model)
        {
            // Detect faces from image URL. Since only recognizing, use the recognition model 1.
            // We use detection model 2 because we are not retrieving attributes.
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url,
                recognitionModel: recognition_model, detectionModel: DetectionModel.Detection02);
            Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{Path.GetFileName(url)}`");
            return detectedFaces.ToList();
        }

        public   async Task  FindSimilar(IFaceClient client, string url, string newImage, string recognition_model)
        {
            Console.WriteLine("========FIND SIMILAR========");
            Console.WriteLine();

            /*List<string> targetImageFileNames = new List<string>
            {
                "newPhoto.jpg"


            };*/

            List<string> targetImageFileNames = BlobStorage.GetBlobs("newimage").ToList();
            /*List<string> ImageList = new List<string>
            {
                *//*"image4.jpeg",
                "image5.jfif",
                "image6.jfif",
                "image7.jfif",*//*
                "main.jpg",
                "Dan Hirst.jpg"

            };*/
            List<string> ImageList = BlobStorage.GetBlobs("images").ToList();

            IList<Guid?> targetFaceIds = new List<Guid?>();
            foreach (var targetImageFileName in targetImageFileNames)
            {
                // Detect faces from target image url.
                var faces = await DetectFaceRecognize(client, $"{newImage}{targetImageFileName}", recognition_model);
                // Add detected faceId to list of GUIDs.
                targetFaceIds.Add(faces[0].FaceId.Value);
            }


            // Detect faces from source image url.
            foreach (var Image in ImageList)
            {

                IList<DetectedFace> detectedFaces =
                    await DetectFaceRecognize(client, $"{url}{Image}", recognition_model);
                Console.WriteLine();

                // Find a similar face(s) in the list of IDs. Comapring only the first in list for testing purposes.

                IList<SimilarFace> similarResults =
                    await client.Face.FindSimilarAsync(detectedFaces[0].FaceId.Value, null, null, targetFaceIds);
                // </snippet_find_similar>
                // <snippet_find_similar_print>

                if (similarResults.Count != 0)
                {
                    foreach (var similarResult in similarResults)
                    {
                        Console.WriteLine(
                            $"Faces from {Image} & ID:{similarResult.FaceId} are similar with confidence: {similarResult.Confidence}.");
                        result +=
                            $"Person is {Image} with confidence: {similarResult.Confidence}.";
                        name = Image;
                        this.url = $"{url}{Image}";
                        confidence = similarResult.Confidence;

                    }
                }
                else
                {
                    Console.WriteLine("Person does not match anyone in the list");
                }

                Console.WriteLine();
                // </snippet_find_similar_print>
            }

        }

    }
}
