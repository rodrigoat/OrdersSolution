import React, { useEffect, useState } from "react";
import "../App.css";
import {
  GridComponent,
  Inject,
  Edit,
  Toolbar,
} from "@syncfusion/ej2-react-grids";
import { MessageComponent } from "@syncfusion/ej2-react-notifications";
import { ButtonComponent } from "@syncfusion/ej2-react-buttons";
import {
  DataManager,
  Query,
  Predicate,
  ODataV4Adaptor,
} from "@syncfusion/ej2-data";

function Grid() {
  const [showMessage, setShowMessage] = useState(false);
  const [responseMessage, setResponseMessage] = useState("Order does not exist anymore.");
  const [serviceURI, setServiceURI] = useState("http://localhost:5238/odata/order");
  const [countryList, setCountryList] = useState(null);
  const [searchOrder, setSearchOrder] = useState(null);
  const [searchCountry, setSearchCountry] = useState(null);
  const [operatorFilter, setOperatorFilter] = useState(false);

  const COUNTRY_URI = "http://localhost:5238/odata/country";
  const toolbarOptions = ["Add", "Edit", "Delete", "Update", "Cancel"];
  const editOptions = {
    allowEditing: true,
    allowAdding: true,
    allowDeleting: true,
  };

  const dataManager = new DataManager({
    url: serviceURI,
    adaptor: new ODataV4Adaptor(),
  });
  const query = new Query().expand("Country");
  const columns = [
    {
      field: "OrderId",
      headerText: "Order ID",
      width: 50,
      textAlign: "Center",
      isPrimaryKey: true,
    },
    { field: "CustoumerId", headerText: "Customer ID", width: 120 },
    {
      field: "Freight",
      headerText: "Freight",
      width: 60,
      textAlign: "Center",
      format: "C2",
    },
    {
      field: 'Country.CountryName',
      headerText: "Ship Country",
      width: 100,
      textAlign: "Center",
      editType: 'dropdownedit'
    }
  ];

  const reset = () => {
    setSearchCountry("");
    setSearchOrder("");
    setShowMessage(false);
    setServiceURI(`http://localhost:5238/odata/order`);
  }
  const search = () => {
    setShowMessage(false);
    if (searchOrder || searchCountry) {
      if (searchOrder && searchCountry) {
        if (operatorFilter) {
          setServiceURI(`http://localhost:5238/odata/order?$filter=OrderId%20eq%20${searchOrder}%20OR%20ShipCountryID%20eq%20${searchCountry}`);
        }
        else {
          setServiceURI(`http://localhost:5238/odata/order?$filter=OrderId%20eq%20${searchOrder}%20and%20ShipCountryID%20eq%20${searchCountry}`);
        }
      }
      else {

        if (searchOrder) {
          setServiceURI(`http://localhost:5238/odata/order?$filter=OrderId%20eq%20${searchOrder}`);
        }

        if (searchCountry) {
          setServiceURI(`http://localhost:5238/odata/order?$filter=ShipCountryID%20eq%20${searchCountry}`);
        }
      }
    }
    else {
      alert("Please select any search filter");
    }
    // setServiceURI("http://localhost:5238/odata/order?$filter=OrderId%20eq%2010255");
  };

  const getCountriesList = async () => {

    let _countries = await (await fetch(COUNTRY_URI)).json();
    console.log(_countries);
    setCountryList(_countries.value);
  }

  useEffect(() => {

  }, [serviceURI]);

  useEffect(() => {
    getCountriesList();
  }, []);

  const onActionFailure = (args) => {
    setShowMessage(true);
  };


  const checkHandler = () => {
    setOperatorFilter(!operatorFilter);
  }

  return (
    <div>
      <div className="filterContainer">
        <div class="row mb-3">
          <label for="orderId" class="col-sm-2 col-form-label">Order Id</label>
          <div class="col-sm-10">
            <input
              id="orderId"
              class="form-control"
              name="input"
              type="search"
              placeholder="12345"
              onChange={(e) => setSearchOrder(e.target.value)}
              value={searchOrder}
            />
          </div>
        </div>
        <div class="row mb-3">
          <label for="shipCountry" class="col-sm-2 col-form-label">Ship Country</label>
          <div class="col-sm-10">
            <select class="form-select" onChange={(e) => setSearchCountry(e.target.value)} value={searchCountry}>
              <option selected value="">Select</option>
              {
                countryList && countryList.map((item, index) => (
                  <option key={index} value={item.CountryId}>{item.CountryName} </option>
                ))
              }
            </select>
          </div>
        </div>
        <div class="row mb-3">
          <label for="operator" class="col-sm-2 col-form-label">AND / OR</label>
          <div class="col-sm-10">
            <label class="toggle-btn">
              <input id="operator" type="checkbox" onChange={checkHandler} disabled={!(searchCountry && searchOrder)} />
              <span class="slider"></span>
            </label>
          </div>
        </div>
        <div class="row mb-3">
          <div class="col-sm-2">

          </div>
          <div class="col-md-6 ps-2">
            <ButtonComponent onClick={search}>Search</ButtonComponent>
            <ButtonComponent onClick={reset}>Reset</ButtonComponent>
          </div>
        </div>

      </div>

      {showMessage && (
        <div className="gridMessage">
          <MessageComponent
            id="msg_error"
            content={responseMessage}
            severity="Error"
          ></MessageComponent>
        </div>
      )}
      <GridComponent
        dataSource={dataManager}
        editSettings={editOptions}
        toolbar={toolbarOptions}
        actionFailure={onActionFailure}
        columns={columns}
        query={query}
      >
        <Inject services={[Edit, Toolbar]} />
      </GridComponent>
    </div>
  );
}

export default Grid;
