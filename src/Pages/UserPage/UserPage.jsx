import { authenticationService, patientService } from "@/Services";
import { Field, Form, Formik } from "formik";
import React from "react";
import { Link } from "react-router-dom";

class UserPage extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      name: null,
      surname: null,
      specialization: null,
      birthdate: null,
      phone: null,
      email: null,
      image: null,
    };
  }

  componentDidMount() {
    {
      this.state.currentUser &&
        this.state.currentUser.role === "User" &&
        patientService
          .getPatientByUser(this.state.currentUser.id)
          .then((patient) => this.setState({ patient }));
    }
  }
  render() {
    const { currentUser } = this.state;
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
                  <header id="header">
                    <div className="homepage">
                      <div className="overlay">
                        <div className="container">
                          <div className="row">
                            {currentUser && (
                              <div className="get-user-margin-top"></div>
                            )}
                            {currentUser && (
                              <div
                                className="form-row"
                                style={{ color: "black" }}
                              >
                                <p>
                                  <h1>{currentUser.username}</h1>
                                  <h3>
                                    <strong>Role: </strong> {currentUser.role}
                                    <br />
                                  </h3>

                                  <h3>
                                    <strong>Name: </strong> {currentUser.name}
                                    <br />
                                  </h3>
                                  <h3>
                                    <strong>Surname: </strong>{" "}
                                    {currentUser.surname}
                                    <br />
                                  </h3>
                                  <h3>
                                    <strong>Birthdate: </strong>{" "}
                                    {currentUser.birthdate}
                                    <br />
                                  </h3>
                                  <h3>
                                    <strong>Phone: </strong> {currentUser.phone}
                                    <br />
                                  </h3>
                                  <h3>
                                    <strong>Email: </strong> {currentUser.email}
                                    <br />
                                  </h3>
                                  <div>
                                    <button className="btn btn-warning">
                                      <Link
                                        style={{ color: "white" }}
                                        to={`/EditUser`}
                                      >
                                        Edit
                                      </Link>
                                    </button>
                                    <button className="btn btn-primary change-doctor-margin-left">
                                      <Link
                                        style={{ color: "white" }}
                                        to={`/ChangePassword`}
                                      >
                                        Change password
                                      </Link>
                                    </button>
                                    {this.state.patient &&
                                      currentUser.role === "User" && (
                                        <button className="btn btn-success change-doctor-margin-left">
                                          <Link
                                            style={{ color: "white" }}
                                            to={`/Histories/${this.state.patient.id}`}
                                          >
                                            Disease History
                                          </Link>
                                        </button>
                                      )}
                                  </div>
                                </p>
                              </div>
                            )}
                          </div>
                        </div>
                      </div>
                    </div>
                  </header>
                </Form>
              )}
            />
          </div>
        </div>
      </div>
    );
  }
}

export { UserPage };
