import React from "react";
import './UploadFile.css'
import ChooseFile from "./ChooseFile/ChooseFile";

function UploadFile(){
    return (
    <div className="header">
        <p>
          Upload a file!
        </p>
        <ChooseFile/>
    </div>)
}

export default UploadFile;