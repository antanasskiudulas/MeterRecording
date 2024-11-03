import { UploadResponse } from '../../models/UploadResponse';
import './ChooseFile.css'
import React, { useRef, useState } from "react";

function ChooseFile(){
    const fileInputRef = useRef<HTMLInputElement>(null);
    const [selectedFile, setSelected] = useState<File | null>(null);


    const handleChooseFileClick = () => {
        fileInputRef.current!.click();
    };

    const handleFileSelected = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];

        if (file) {
            setSelected(file);
        }
    };

    const handleFileUpload = async () => {
        if (!selectedFile){
            alert("You need to select file before uploading");
            return;
        }

        const formData = new FormData();
        formData.append("file", selectedFile);

        try
        {
            const response = await fetch(
                `${process.env.REACT_APP_API_URL}/meter-reading-uploads`,
                {
                    method: "POST",
                    body: formData
                }
            );

            if (!response.ok){
                alert(`File upload failed: ${response.statusText}`);
                return;
            }

            const data: UploadResponse = await response.json();

            alert(`Uploaded ${data.successReadings} meter readings. Skipped ${data.failedReadings} due to validation failures`);
        }
        catch (error)
        {
            console.log(error);
            alert("Failed to upload the file");
        }
    } 

    return (
    <div className="ChooseFileSection">
        <input className="ChooseFileInput"
            type="file"
            ref={fileInputRef}
            onChange={handleFileSelected}
        />
        <button className="ChooseFileButton" onClick={handleChooseFileClick}>
            {selectedFile ? `${selectedFile.name} File Selected` : "Select File"}
        </button>

        <button className="ChooseFileButton" onClick={handleFileUpload} disabled={!selectedFile}>
            {"Upload File"}
        </button>
    </div>)
}

export default ChooseFile;