import { authenticationService, patientService } from "@/Services";
import { Field, Form, Formik } from "formik";
import React from "react";
import { Link } from "react-router-dom";

class GetPatient extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      patients: null,
      name: null,
      surname: null,
      information: null,
      birthdate: null,
      phone: null,
      email: null,
      debt: null,
    };
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Patients`);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    patientService.getPatient(id).then((patients) => {
      this.setState(patients);
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
                information: this.state.information,
                birthdate: this.state.birthdate,
                phone: this.state.phone,
                email: this.state.email,
                debt: this.state.debt,
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
                        <label htmlFor="information">Information</label>
                        <Field
                          name="information"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="debt">Debt</label>
                        <Field
                          name="debt"
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
                        <label htmlFor="birthdate">Birthdate</label>
                        <Field
                          name="birthdate"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <button className="btn btn-warning get-employee-back-button-margin-left">
                        <Link
                          style={{ color: "white" }}
                          to={`/EditPatient/${id}`}
                        >
                          Edit
                        </Link>
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

export { GetPatient };
