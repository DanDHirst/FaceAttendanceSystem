
import RPi.GPIO as GPIO
import time
GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)

GPIO.setup(27,GPIO.IN,pull_up_down=GPIO.PUD_DOWN)
print("camera ready")
while True:
    
    if GPIO.input(27) == GPIO.HIGH:

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
        camera.close()

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
        auth = 1297
        testUrl = "https://localhost:44358/api/registeredStudents?filename="+filename+"&room="+room+"&auth="+auth+""
        url = "https://faceattendance.azurewebsites.net/api/registeredStudents?filename="+filename+"&room="+room+"&auth="+auth+""

        x = requests.post(url)
        #x = requests.post(testUrl , verify= False)

        print(x.text)
        import json
        try:
            response = json.loads(x.text)
            print(response["notFound"])
            if(response["notFound"]):
                GPIO.setup(4,GPIO.OUT)
                print ("red")
                GPIO.output(4,GPIO.HIGH)
                time.sleep(3)
                print("redoff")
                GPIO.output(4,GPIO.LOW)
            else:
                GPIO.setup(17,GPIO.OUT)
                print ("green")
                GPIO.output(17,GPIO.HIGH)
                time.sleep(3)
                print("green off")
                GPIO.output(17,GPIO.LOW)
        except:
                GPIO.setup(4,GPIO.OUT)
                print ("red")
                GPIO.output(4,GPIO.HIGH)
                time.sleep(3)
                print("redoff")
                GPIO.output(4,GPIO.LOW)
            
            
            
        


                
                

