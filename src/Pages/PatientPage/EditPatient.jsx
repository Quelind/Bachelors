import { authenticationService, patientService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class EditPatient extends React.Component {
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
    return (
      <div>
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              enableReinitialize
              initialValues={{
                id: this.state.id,
                name: this.state.name,
                surname: this.state.surname,
                information: this.state.information,
                birthdate: this.state.birthdate,
                phone: this.state.phone,
                email: this.state.email,
                fk_user: this.state.fk_user,
              }}
              validationSchema={Yup.object().shape({
                name: Yup.string()
                  .min(3, "Name must be at least 3 characters")
                  .required("Name is required"),
                surname: Yup.string()
                  .min(3, "Surname must be at least 3 characters")
                  .required("Surname is required"),
                information: Yup.string().required("Information is required"),
                birthdate: Yup.string()
                  .required("Birthdate is required")
                  .matches(
                    /^\d{4}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$/,
                    "The date format is invalid."
                  ),
                phone: Yup.string().required("Phone is required"),
                email: Yup.string()
                  .email("Email format invalid")
                  .required("Email is required"),
              })}
              onSubmit={(
                {
                  name,
                  surname,
                  information,
                  birthdate,
                  phone,
                  email,
                  id,
                  fk_user,
                },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const Patient = {
                  name: name,
                  surname: surname,
                  information: information,
                  birthdate: birthdate,
                  phone: phone,
                  email: email,
                  id: id,
                  fk_user: fk_user,
                };
                patientService.editPatient(Patient, id).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/Patients" },
                    };
                    this.props.history.push(from);
                  },
                  (error) => {
                    setSubmitting(false);
                    setStatus(error);
                  }
                );
              }}
              render={({ errors, status, touched, isSubmitting }) => (
                <Form>
                  {status && (
                    <div className={"alert alert-danger"}>{status}</div>
                  )}
                  <h2>Edit Patient</h2>

                  <div className="form-group">
                    <label htmlFor="name">Name</label>
                    <Field
                      name="name"
                      type="text"
                      className={
                        "form-control" +
                        (errors.name && touched.name ? " is-invalid" : "")
                      }
                    />
                    <ErrorMessage
                      name="name"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="surname">Surname</label>
                    <Field
                      name="surname"
                      type="text"
                      className={
                        "form-control" +
                        (errors.surname && touched.surname ? " is-invalid" : "")
                      }
                    />
                    <ErrorMessage
                      name="surname"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="information">Information</label>
                    <Field
                      name="information"
                      type="text"
                      className={
                        "form-control" +
                        (errors.information && touched.information
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="information"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="birthdate">Birthdate</label>
                    <Field
                      name="birthdate"
                      type="text"
                      placeholder="yyyy-mm-dd"
                      className={
                        "form-control" +
                        (errors.birthdate && touched.birthdate
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="birthdate"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="phone">Phone</label>
                    <Field
                      name="phone"
                      type="text"
                      className={
                        "form-control" +
                        (errors.phone && touched.phone ? " is-invalid" : "")
                      }
                    />
                    <ErrorMessage
                      name="phone"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="email">Email</label>
                    <Field
                      name="email"
                      type="text"
                      className={
                        "form-control" +
                        (errors.email && touched.email ? " is-invalid" : "")
                      }
                    />
                    <ErrorMessage
                      name="email"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-row flexbox-row-form">
                    <button
                      className="btn btn-danger"
                      type="button"
                      id="button-addon2"
                      onClick={this.handleReturn}
                    >
                      Back
                    </button>
                    <button
                      type="submit"
                      className="btn btn-primary flexbox-back-form"
                      disabled={isSubmitting}
                    >
                      Submit
                    </button>
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

export { EditPatient };
