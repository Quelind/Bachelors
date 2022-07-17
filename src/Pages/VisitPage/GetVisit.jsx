import { authenticationService, visitService } from "@/Services";
import { Field, Form, Formik } from "formik";
import React from "react";
import { Link } from "react-router-dom";

class GetVisit extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      visits: null,
      name: null,
      surname: null,
      specialization: null,
      birthdate: null,
      phone: null,
      email: null,
      image: null,
    };
    this.handleReturn = this.handleReturn.bind(this);
    this.clickDelete = this.clickDelete.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Visits`);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    visitService.getVisit(id).then((visits) => {
      this.setState(visits);
    });
  }
  clickDelete(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      visitService.deleteVisit(value).then(
        () => {
          const { from } = this.props.location.state || {
            from: { pathname: "/Visits" },
          };
          this.props.history.push(from);
        },
        (error) => {
          this.setState({ errorMessage: error });
        }
      );
    }
  }
  render() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    return (
      <div>
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              enableReinitialize
              initialValues={{
                date: this.state.date,
                time: this.state.time,
                procedure_name: this.state.procedure_name,
                procedure_information: this.state.procedure_information,

                doctor_name: this.state.doctor_name,
                doctor_surname: this.state.doctor_surname,
                specialization: this.state.specialization,
                fk_room: this.state.fk_room,

                patient_name: this.state.patient_name,
                patient_surname: this.state.patient_surname,
                patient_comment: this.state.patient_comment,
                confirmed: this.state.confirmed,
              }}
              render={() => (
                <Form>
                  <div className="get-visit-margin-top">
                    <h1>Visit ID: {this.state.id}</h1>
                  </div>
                  <div className="form-row">
                    <div className="form-group col">
                      <h4 className="card-header">
                        <strong>Visit</strong>
                      </h4>
                      <h6>
                        <strong>Date: </strong> {this.state.date}{" "}
                        {this.state.time}
                        <br />
                      </h6>
                      <h6>
                        <strong>Confirmed: </strong> {this.state.confirmed}
                        <br />
                      </h6>
                      <h6>
                        <strong>Procedure: </strong> {this.state.procedure_name}
                        <br />
                        {this.state.procedure_information}
                        <br />
                      </h6>
                    </div>
                    <div className="form-group col">
                      <h4 className="card-header">
                        <strong>Doctor</strong>
                      </h4>
                      <h6>
                        <strong>Name: </strong> {this.state.doctor_name}{" "}
                        {this.state.doctor_surname}
                        <br />
                      </h6>
                      <h6>
                        <strong>Specialization: </strong>{" "}
                        {this.state.specialization}
                        <br />
                      </h6>
                      <h6>
                        <strong>Room: </strong> {this.state.fk_room}
                        <br />
                      </h6>
                    </div>
                    <div className="form-group col">
                      <h4 className="card-header">
                        <strong>Patient</strong>
                      </h4>
                      <h6>
                        <strong>Name: </strong> {this.state.patient_name}{" "}
                        {this.state.patient_surname}
                        <br />
                      </h6>
                      <h6>
                        <strong>Comment: </strong> {this.state.patient_comment}
                        <br />
                      </h6>
                    </div>
                    <div className="flex-row">
                      <button
                        className="btn btn-danger"
                        type="button"
                        id="button-addon2"
                        onClick={this.handleReturn}
                      >
                        Back
                      </button>
                      {this.state.currentUser &&
                        this.state.currentUser.role === "Admin" && (
                          <button className="btn btn-primary change-doctor-margin-left">
                            <Link
                              style={{ color: "white" }}
                              to={`/ChangeDoctor/${id}+${this.state.fk_timetable}`}
                            >
                              Change doctor
                            </Link>
                          </button>
                        )}
                      <button className="btn btn-warning  back-button-margin-left">
                        <Link
                          style={{ color: "white" }}
                          to={`/EditVisit/${id}`}
                        >
                          Edit
                        </Link>
                      </button>
                      <button
                        value={this.state.id}
                        onClick={this.clickDelete}
                        className="btn btn-danger change-doctor-margin-left"
                      >
                        Delete
                      </button>
                    </div>
                  </div>
                </Form>
              )}
            />
          </div>
        </div>
      </div>
    );
  }
}

export { GetVisit };
