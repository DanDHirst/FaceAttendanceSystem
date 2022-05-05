# Face Attendance system

Whilst studying at the University of Plymouth the author found many problems with attendance system used by the University. The intended purpose of this application is to reduce the amount of work a student must complete to register. There have been many applications used each with their disadvantages and advantages. The goal is to investigate and develop an application that reduces some of the problems whilst still making the application usable for lectures and students. The application must have a similar or improved time frame and complexity to the current system.


## Technologies used
- Azure face AI
- ASP.net Core 3.1
- Azure Blob Storgae
- Azure web services
- Azure DB SQL
- Python for RPI
- Cypress
- Nunit

## Requirements
To run this project all you need is 2019 version of visual Studio

For all the Azure services they will have to be manually created, below is a guide that demos this.

## User Guide
### You can view the live application here: 
Live application: https://faceattendance.azurewebsites.net/ 
#### Admin: 
Username: admin@123.com \
Password: Password123! 
#### Lecturer:  
Username: admin@1234.com \
Password: Password123! 

### Steps to create your own build
1.	Download the source code from GitHub - https://github.com/DanDHirst/FaceAttendanceSystem
2.	Open the visual studio solution file  
3.	Run the application to start the migrations 
4.	Once reached the login page enter email: Admin@123.com password: Password123!

#### Create face AI key
1.	Log in to azure portal (https://portal.azure.com/#home)
2.	Create new resource
3.	Go to the face service and create 
4.	 
5.	Create it in UK south for simplicity and choose the free tier
6.	Find the keys for the Face AI 
7.	 
8.	Go to the constants file in models and change the subscription key to the face ai key
9.	 
#### Create Blob storage
1.	Log in to azure portal (https://portal.azure.com/#home)
2.	Create a new storage account
3.	 
4.	Create a free resource
5.	Go to the access key for the storage account just created 
6.	Update both image urls in the constants file to the new keys and update the connection string to the blob storage connection

