import { authenticationService, patientService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";
import { Link } from "react-router-dom";

class PatientSearch extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      patients: null,
      errorMessage: "",
      search: null,
    };
    this.clickDelete = this.clickDelete.bind(this);
    this.handleReturn = this.handleReturn.bind(this);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let search = url.substring(url.lastIndexOf("/") + 1);
    patientService
      .patientSearch(search)
      .then((patients) => this.setState({ patients }));
  }

  handleReturn() {
    this.props.history.push(`/Patients`);
  }

  clickDelete(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      patientService.deletePatient(value).then(
        () => {
          this.props.history.push(`/Patients`);
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
          render={({ status }) => (
            <div className="form-row flexbox-row">
              <div className="form-group col margin-from-top">
                <h1>Search Results</h1>
              </div>
              {status && <div className={"alert alert-danger"}>{status}</div>}
              <div className="input-group mb-4 col p-2 bd"></div>
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
              <th>Email</th>
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
        <div className="form-row flexbox-row-list">
          <button
            className="btn btn-danger"
            type="button"
            id="button-addon2"
            onClick={this.handleReturn}
          >
            Back
          </button>
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

export { PatientSearch };
