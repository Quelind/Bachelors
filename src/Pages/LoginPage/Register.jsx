import { authenticationService, userService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class Register extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      name: null,
      surname: null,
      username: null,
      password: null,
      birthdate: null,
      phone: null,
      email: null,
    };
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/login`);
  }
  render() {
    return (
      <div className="flexbox-container-3">
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              initialValues={{
                name: "",
                surname: "",
                username: "",
                password: "",
                passwordConfirmation: "",
                birthdate: "",
                phone: "",
                email: "",
              }}
              validationSchema={Yup.object().shape({
                name: Yup.string().required("Name is required"),
                surname: Yup.string().required("Surname is required"),
                username: Yup.string().required("Username is required"),
                password: Yup.string()
                  .min(8, "Password must be at least 8 characters")
                  .required("Password is required"),
                passwordConfirmation: Yup.string()
                  .oneOf([Yup.ref("password"), null], "Passwords must match")
                  .required("Password Confirmation is required"),
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
                { name, surname, username, password, birthdate, phone, email },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const User = {
                  name: name,
                  surname: surname,
                  username: username,
                  password: password,
                  birthdate: birthdate,
                  phone: phone,
                  email: email,
                };
                userService.register(User).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/login" },
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
                  <h2>Registration</h2>
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
                    <label htmlFor="username">Username</label>
                    <Field
                      name="username"
                      type="text"
                      className={
                        "form-control" +
                        (errors.username && touched.username
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="username"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-row">
                    <div className="form-group col">
                      <label htmlFor="password">Password</label>
                      <Field
                        name="password"
                        type="password"
                        className={
                          "form-control" +
                          (errors.password && touched.password
                            ? " is-invalid"
                            : "")
                        }
                      />
                      <ErrorMessage
                        name="password"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
                    <div className="form-group col">
                      <label htmlFor="passwordConfirmation">
                        Password Confirmation
                      </label>
                      <Field
                        name="passwordConfirmation"
                        type="password"
                        className={
                          "form-control" +
                          (errors.passwordConfirmation &&
                          touched.passwordConfirmation
                            ? " is-invalid"
                            : "")
                        }
                      />
                      <ErrorMessage
                        name="passwordConfirmation"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
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
                  <div className="form-row flexbox-row-register">
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
                      className="btn btn-primary flexbox-back-register"
                      disabled={isSubmitting}
                    >
                      Register
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

export { Register };
