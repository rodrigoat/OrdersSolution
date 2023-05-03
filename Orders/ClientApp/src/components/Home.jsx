import React from "react";
import { useNavigate } from "react-router-dom";
import { SpreadsheetComponent } from "@syncfusion/ej2-react-spreadsheet";
import "../App.css";

function Home() {
  const navigate = useNavigate();
  const beforeSave = (beforeSaveEventArgs) => {
    beforeSaveEventArgs.isFullPost = false;
    beforeSaveEventArgs.needBlobData = true;
  };

  const saveCompleted = () => {
    navigate("/grid");
  };

  return (
    <div>
      <SpreadsheetComponent
        allowOpen={true}
        openUrl="http://localhost:44405/order/open"
        beforeSave={beforeSave}
        allowSave={true}
        saveUrl="http://localhost:44405/order/save"
        saveComplete={saveCompleted}
      />
    </div>
  );
}

export default Home;
