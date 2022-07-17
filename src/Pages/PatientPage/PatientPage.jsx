import { authenticationService, patientService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";
import { Link } from "react-router-dom";

class PatientPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      patients: null,
      errorMessage: "",
      search: null,
    };
    this.clickDelete = this.clickDelete.bind(this);
    this.handleChangeSearch = this.handleChangeSearch.bind(this);
    this.handleSubmitSearch = this.handleSubmitSearch.bind(this);
  }

  componentDidMount() {
    patientService
      .getAllPatients()
      .then((patients) => this.setState({ patients }));
  }

  handleChangeSearch(event) {
    this.setState({ value: event.target.value });
  }

  handleSubmitSearch() {
    this.props.history.push(`/PatientSearch/${this.state.value}`);
  }

  clickDelete(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      patientService.deletePatient(value).then(
        () => {
          location.reload(true);
        },
        (error) => {
          this.setState({ errorMessage: error });
        }
      );
    }
  }

  render() {
    const { currentUser, patients } = this.state;
    return (
      <div>
        <Formik
          render={({ errors, status, touched }) => (
            <div className="form-row flexbox-row">
              <div className="form-group col margin-from-top">
                <h1>Patients</h1>
              </div>
              {status && <div className={"alert alert-danger"}>{status}</div>}
              <div className="form-group col input-group flexbox-search-list margin-from-top">
                <input
                  type="text"
                  className={
                    "form-control search-style-list" +
                    (errors.name && touched.name ? " is-invalid" : "")
                  }
                  placeholder="Search"
                  onChange={this.handleChangeSearch}
                />
                <button
                  className="btn btn-primary button-search-list"
                  type="button"
                  id="button-addon2"
                  onClick={this.handleSubmitSearch}
                >
                  Search
                </button>
              </div>
            </div>
          )}
        />
        {this.state.errorMessage && (
          <div className={"alert alert-danger"}>{this.state.errorMessage}</div>
        )}
        <table className="table table-striped">
          <thead>
            <tr>
              <th>Name</th>
              <th>Surname</th>
              <th>Birthdate</th>
              <th>Phone</th>
              <th style={{ width: "10%" }}></th>
            </tr>
          </thead>
          <tbody>
            {patients &&
              patients.map((patient) => (
                <tr key={patient.id}>
                  <td>{patient.name}</td>
                  <td>{patient.surname}</td>
                  <td>{patient.birthdate}</td>
                  <td>{patient.phone}</td>
                  <td style={{ whiteSpace: "nowrap" }}>
                    <button className="btn btn-sm btn-success btn-get-patient">
                      <Link
                        style={{ color: "white" }}
                        to={`/GetPatient/${patient.id}`}
                      >
                        More Information
                      </Link>
                    </button>
                    {currentUser.role != "User" && (
                      <button className="btn btn-sm btn-primary btn-get-history">
                        <Link
                          style={{ color: "white" }}
                          to={`/Histories/${patient.id}`}
                        >
                          Disease History
                        </Link>
                      </button>
                    )}
                    <button className="btn btn-sm btn-warning btn-edit-patient">
                      <Link
                        style={{ color: "white" }}
                        to={`/EditPatient/${patient.id}`}
                      >
                        Edit
                      </Link>
                    </button>
                    <button
                      value={patient.id}
                      onClick={this.clickDelete}
                      className="btn btn-sm btn-danger btn-delete-patient"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
        <div className="form-row flexbox-row-list2">
          <button className="button-main add-button add-button-style">
            <Link style={{ color: "white" }} to={`/AddPatient`}>
              + Add
            </Link>
          </button>
        </div>
      </div>
    );
  }
}

export { PatientPage };
