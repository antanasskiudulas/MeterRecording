# MeterRecording
You can run the app in the following modes:
 -Development
 -Production
 
 #Development
 This will use Sqlite as DB.
 Use IIS to launch the app (note, will have to run front-end seperately).
 Endpoint can then be accessed http://localhost:56420/meter-reading-uploads
 
 #Production
 This will use PostgreSQL as DB.
 Must have Docker desktop downloaded.
 Run docker-compose up --build --force-recreate
 You can then access the upload via client through http://localhost:3000