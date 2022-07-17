import {
  authenticationService,
  employeeService,
  patientService,
  visitService,
} from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";
import { Link } from "react-router-dom";

class VisitSearch extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      users: null,
      visits: null,
      errorMessage: "",
      search: null,
      patients: null,
      employees: null,
      old: 0,
    };
    this.clickDelete = this.clickDelete.bind(this);
    this.handleReturn = this.handleReturn.bind(this);
    this.clickConfirm = this.clickConfirm.bind(this);
    this.oldVisits = this.oldVisits.bind(this);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let search = url.substring(url.lastIndexOf("/") + 1);
    visitService
      .visitSearchActive(search)
      .then((visits) => this.setState({ visits }));
    {
      this.state.currentUser &&
        (this.state.currentUser.role === "Admin" ||
          this.state.currentUser.role === "Employee");
      patientService.getAllPatients().then((result) => {
        this.setState({
          patients: result.map((o) => ({
            id: o.id,
            fk_user: o.fk_user,
          })),
        });
      });
    }
    {
      this.state.currentUser &&
        this.state.currentUser.role === "Admin" &&
        employeeService.getAllEmployees().then((result) => {
          this.setState({
            employees: result.map((o) => ({
              id: o.id,
              fk_user: o.fk_user,
            })),
          });
        });
    }
  }

  handleReturn() {
    this.props.history.push(`/Visits`);
  }

  oldVisits() {
    let url = window.location.pathname;
    let search = url.substring(url.lastIndexOf("/") + 1);
    visitService
      .visitSearch(search)
      .then((visits) => this.setState({ visits, old: 1 }));
  }

  clickDelete(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      visitService.deleteVisit(value).then(
        () => {
          location.reload(true);
        },
        (error) => {
          this.setState({ errorMessage: error });
        }
      );
    }
  }
  handleReturn() {
    this.props.history.push(`/Visits`);
  }

  clickConfirm(event) {
    let { value } = event.target;
    visitService.confirmVisit(value).then(
      () => {
        location.reload(true);
      },
      (error) => {
        this.setState({ errorMessage: error });
      }
    );
  }

  render() {
    const { currentUser, currentPatient, visits, currentEmployee } = this.state;

    var today = new Date();
    var date =
      today.getFullYear() +
      "-" +
      "0" +
      (today.getMonth() + 1) +
      "-" +
      "0" +
      today.getDate();
    var dateTomorrow =
      today.getFullYear() +
      "-" +
      "0" +
      (today.getMonth() + 1) +
      "-" +
      "0" +
      (today.getDate() + 1);
    return (
      <div>
        <Formik
          render={({ status }) => (
            <div className="form-row flexbox-row">
              <div className="form-group col margin-from-top">
                <h1>Search Results</h1>
              </div>
              {status && <div className={"alert alert-danger"}>{status}</div>}
              <div className="form-group col input-group flexbox-search-list margin-from-top">
                <button
                  className="btn btn-success button-search-list"
                  type="button"
                  id="button-addon2"
                  onClick={this.oldVisits}
                >
                  Show old visits
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
              <th>Date</th>
              <th>Time</th>
              {currentUser.role != "User" && <th>Patient</th>}
              {currentUser.role != "Employee" && <th>Doctor</th>}
              <th>Procedure</th>
              <th>Room</th>
              {this.state.old != 1 && <th>Confirmed</th>}
              <th style={{ width: "10%" }}></th>
            </tr>
          </thead>
          <tbody>
            {visits && currentPatient && currentUser.role === "User"
              ? visits
                  .filter((visit) => visit.fk_patient == currentPatient.id)
                  .map((visit) => (
                    <tr key={visit.id}>
                      <td>{visit.date}</td>
                      <td>{visit.time}</td>
                      <td>
                        {visit.doctor_name} {visit.doctor_surname} (
                        {visit.specialization})
                      </td>
                      <td>{visit.procedure_name}</td>
                      <td>{visit.fk_room}</td>
                      <td>{visit.confirmed}</td>
                      <td style={{ whiteSpace: "nowrap" }}>
                        {this.state.old != 1 &&
                          date != visit.date &&
                          dateTomorrow != visit.date && (
                            <button className="btn btn-sm btn-success btn-get-visit">
                              <Link
                                style={{ color: "white" }}
                                to={`/GetVisit/${visit.id}`}
                              >
                                More Information
                              </Link>
                            </button>
                          )}

                        {this.state.old != 1 &&
                          date != visit.date &&
                          dateTomorrow != visit.date && (
                            <button className="btn btn-sm btn-warning btn-edit-visit">
                              <Link
                                style={{ color: "white" }}
                                to={`/EditVisit/${visit.id}`}
                              >
                                Edit
                              </Link>
                            </button>
                          )}
                        {this.state.old != 1 &&
                          date != visit.date &&
                          dateTomorrow != visit.date && (
                            <button
                              value={visit.id}
                              onClick={this.clickDelete}
                              className="btn btn-sm btn-danger btn-delete-visit"
                            >
                              Delete
                            </button>
                          )}
                      </td>
                    </tr>
                  ))
              : visits && currentEmployee && currentUser.role === "Employee"
              ? visits
                  .filter((visit) => visit.fk_doctor == currentEmployee.id)
                  .map((visit) => (
                    <tr key={visit.id}>
                      <td>{visit.date}</td>
                      <td>{visit.time}</td>
                      <td>
                        {visit.patient_name} {visit.patient_surname}
                      </td>
                      <td>{visit.procedure_name}</td>
                      <td>{visit.fk_room}</td>
                      {visit.confirmed === "no" && this.state.old != 1 && (
                        <button
                          value={visit.id}
                          onClick={this.clickConfirm}
                          className="btn btn-sm btn-primary"
                        >
                          Confirm
                        </button>
                      )}
                      {visit.confirmed != "no" && this.state.old != 1 && (
                        <td>{visit.confirmed}</td>
                      )}
                      <td style={{ whiteSpace: "nowrap" }}>
                        {this.state.old != 1 &&
                          date != visit.date &&
                          dateTomorrow != visit.date && (
                            <button className="btn btn-sm btn-success btn-get-visit">
                              <Link
                                style={{ color: "white" }}
                                to={`/GetVisit/${visit.id}`}
                              >
                                More Information
                              </Link>
                            </button>
                          )}
                        {this.state.old != 1 &&
                          date != visit.date &&
                          dateTomorrow != visit.date && (
                            <button className="btn btn-sm btn-warning btn-edit-visit">
                              <Link
                                style={{ color: "white" }}
                                to={`/EditVisit/${visit.id}`}
                              >
                                Edit
                              </Link>
                            </button>
                          )}
                        {this.state.old != 1 &&
                          date != visit.date &&
                          dateTomorrow != visit.date && (
                            <button
                              value={visit.id}
                              onClick={this.clickDelete}
                              className="btn btn-sm btn-danger btn-delete-visit"
                            >
                              Delete
                            </button>
                          )}
                      </td>
                    </tr>
                  ))
              : visits &&
                visits.map((visit) => (
                  <tr key={visit.id}>
                    <td>{visit.date}</td>
                    <td>{visit.time}</td>
                    <td>
                      {visit.patient_name} {visit.patient_surname}
                    </td>
                    <td>
                      {visit.doctor_name} {visit.doctor_surname} (
                      {visit.specialization})
                    </td>
                    <td>{visit.procedure_name}</td>
                    <td>{visit.fk_room}</td>
                    {visit.confirmed === "no" && this.state.old != 1 && (
                      <button
                        value={visit.id}
                        onClick={this.clickConfirm}
                        className="btn btn-sm btn-primary btn-delete-visit"
                      >
                        Confirm
                      </button>
                    )}
                    {visit.confirmed === "yes" && this.state.old != 1 && (
                      <td>{visit.confirmed}</td>
                    )}
                    <td style={{ whiteSpace: "nowrap" }}>
                      {this.state.old != 1 &&
                        date != visit.date &&
                        dateTomorrow != visit.date && (
                          <button className="btn btn-sm btn-success btn-get-visit">
                            <Link
                              style={{ color: "white" }}
                              to={`/GetVisit/${visit.id}`}
                            >
                              More Information
                            </Link>
                          </button>
                        )}
                      {this.state.old != 1 &&
                        date != visit.date &&
                        dateTomorrow != visit.date && (
                          <button className="btn btn-sm btn-warning btn-edit-visit">
                            <Link
                              style={{ color: "white" }}
                              to={`/EditVisit/${visit.id}`}
                            >
                              Edit
                            </Link>
                          </button>
                        )}
                      {this.state.old != 1 &&
                        date != visit.date &&
                        dateTomorrow != visit.date && (
                          <button
                            value={visit.id}
                            onClick={this.clickDelete}
                            className="btn btn-sm btn-danger btn-delete-visit"
                          >
                            Delete
                          </button>
                        )}
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
          {currentUser.role === "Employee" && currentEmployee && (
            <button className="button-main add-button add-button-style">
              <Link
                style={{ color: "white" }}
                to={`/AddVisit/${currentEmployee.id}`}
              >
                + Add
              </Link>
            </button>
          )}
        </div>
      </div>
    );
  }
}

export { VisitSearch };
