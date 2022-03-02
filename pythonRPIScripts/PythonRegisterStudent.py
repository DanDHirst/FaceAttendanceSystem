
from picamera import PiCamera
from time import sleep
import random

camera= PiCamera()
camera.start_preview()
sleep(5)
randnum = random.randrange(1,100000)
filename = 'image'+str(randnum)+ '.jpg'
fileLoc = '/home/pi/Desktop/images/'+filename
camera.capture(fileLoc)
camera.stop_preview()

#sudo pip3 install azure-storage-blob
import os, uuid
from azure.storage.blob import BlobServiceClient, BlobClient, ContainerClient, __version__

connect_str ='DefaultEndpointsProtocol=https;AccountName=faceattendimages;AccountKey=g1VnlmiQ8ChdJluUaPpoOAHtk1WD/CpcNjzTuBKS3P8PekttQvm46jhhRGyG/e3meS7fB/W4AP8+ftjcDNAq+A==;EndpointSuffix=core.windows.net' 
blob_service_client = BlobServiceClient.from_connection_string(connect_str)
container_name = 'newimage'


upload_file_path = fileLoc



blob_client = blob_service_client.get_blob_client(container=container_name, blob=filename)


print("\nUploading to Azure Storage as blob:\n\t" + filename)
with open(upload_file_path, "rb") as data:
    blob_client.upload_blob(data)

import requests
#sudo pip3 install requests
room = 'bgb102'
testUrl = "https://localhost:44358/api/registeredStudents?filename="+filename+"&room="+room+""
url = "https://faceattendance.azurewebsites.net/api/registeredStudents?filename="+filename+"&room="+room+""

x = requests.post(url)
#x = requests.post(testUrl , verify= False)

print(x.text)
