# MeterRecording

You can run the app in the following modes:
- **Production**
- **Development**

## Production (Recommended)
App in this mode is containerised and built via docker-compose.yaml file. Here PostgreSQL as the DB.

1. Ensure that Docker Desktop is installed on your machine.
2. Run the following command to start the app:
   ```powershell
   docker-compose up --build --force-recreate```
3. You can then access the upload client at: `http://localhost:3000`

## Development
This mode uses SQLite as the database.

1. Use IIS to launch the app (note: you will need to run the front end separately).
2. Once launched, the API endpoint can be accessed at:  
   `http://localhost:56420/meter-reading-uploads`