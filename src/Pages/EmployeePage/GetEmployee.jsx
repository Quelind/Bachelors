import { authenticationService, employeeService } from "@/Services";
import { Field, Form, Formik } from "formik";
import React from "react";
import { Link } from "react-router-dom";

class GetEmployee extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      employees: null,
      name: null,
      surname: null,
      specialization: null,
      birthdate: null,
      phone: null,
      email: null,
      image: null,
    };
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Employees`);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    employeeService.getEmployee(id).then((employees) => {
      this.setState(employees);
    });
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
                name: this.state.name,
                surname: this.state.surname,
                specialization: this.state.specialization,
                birthdate: this.state.birthdate,
                phone: this.state.phone,
                email: this.state.email,
                fk_room: this.state.fk_room,
                image: this.state.image,
              }}
              render={() => (
                <Form>
                  <h2>More information:</h2>
                  <div className="form-row">
                    <div className="form-group col">
                      <div className="form-group">
                        <label htmlFor="name">Name</label>
                        <Field
                          name="name"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="surname">Surname</label>
                        <Field
                          name="surname"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="specialization">Specialization</label>
                        <Field
                          name="specialization"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="birthdate">Birthdate</label>
                        <Field
                          name="birthdate"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <button
                        className="btn btn-danger"
                        type="button"
                        id="button-addon2"
                        onClick={this.handleReturn}
                      >
                        Back
                      </button>
                    </div>
                    <div className="form-group col">
                      <div className="form-group">
                        <label htmlFor="phone">Phone</label>
                        <Field
                          name="phone"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="email">Email</label>
                        <Field
                          name="email"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="fk_room">Room</label>
                        <Field
                          name="fk_room"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      {this.state.currentUser &&
                        this.state.currentUser.role === "Admin" && (
                          <button className="btn btn-warning get-employee-back-button-margin-left">
                            <Link
                              style={{ color: "white" }}
                              to={`/EditEmployee/${id}`}
                            >
                              Edit
                            </Link>
                          </button>
                        )}
                      {this.state.currentUser &&
                        this.state.currentUser.role === "User" && (
                          <button className="button-main register-button get-employee-back-button-margin-left">
                            <Link
                              style={{ color: "white" }}
                              to={`/AddVisit/${id}`}
                            >
                              Register
                            </Link>
                          </button>
                        )}
                    </div>
                    <div className="form-group col">
                      <img
                        src={this.state.image}
                        className="get-image-margin"
                      />
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

export { GetEmployee };
