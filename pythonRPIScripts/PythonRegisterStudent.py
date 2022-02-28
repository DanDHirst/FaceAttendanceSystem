
#pip install azure-storage-blob
import os, uuid
from azure.storage.blob import BlobServiceClient, BlobClient, ContainerClient, __version__

connect_str ='DefaultEndpointsProtocol=https;AccountName=faceattendimages;AccountKey=g1VnlmiQ8ChdJluUaPpoOAHtk1WD/CpcNjzTuBKS3P8PekttQvm46jhhRGyG/e3meS7fB/W4AP8+ftjcDNAq+A==;EndpointSuffix=core.windows.net' 
blob_service_client = BlobServiceClient.from_connection_string(connect_str)
container_name = 'images'
local_path = "./data47"
os.mkdir(local_path)

local_file_name = str(uuid.uuid4()) + ".txt"
upload_file_path = os.path.join(local_path, local_file_name)

file = open(upload_file_path, 'w')
file.write("Hello, World!")
file.close()

blob_client = blob_service_client.get_blob_client(container=container_name, blob=local_file_name)


print("\nUploading to Azure Storage as blob:\n\t" + local_file_name)
with open(upload_file_path, "rb") as data:
    blob_client.upload_blob(data)

import requests
#pip install requests
filename = 'newPhoto.jpg'
room = 'bgb102'
testUrl = "https://localhost:44358/api/registeredStudents?filename="+filename+"&room="+room+""
url = "https://faceattendance.azurewebsites.net/api/registeredStudents?filename="+filename+"&room="+room+""

#x = requests.post(url)
x = requests.post(testUrl , verify= False)

print(x.text)
