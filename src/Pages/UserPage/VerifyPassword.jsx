import { authenticationService, userService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class VerifyPassword extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
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
                reset_token: "",
                password: "",
                passwordConfirmation: "",
              }}
              validationSchema={Yup.object().shape({
                reset_token: Yup.string().required("Token is required"),

                password: Yup.string()
                  .min(8, "Password must be at least 8 characters")
                  .required("Password is required"),
                passwordConfirmation: Yup.string()
                  .oneOf([Yup.ref("password"), null], "Passwords must match")
                  .required("Password Confirmation is required"),
              })}
              onSubmit={(
                { password, reset_token },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const VerifyPassword = {
                  password: password,
                  reset_token: reset_token,
                };
                userService.verifyPassword(VerifyPassword).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/login" },
                    };
                    this.props.history.push(from);
                    authenticationService.logout();
                  },
                  (error) => {
                    setSubmitting(false);
                    setStatus(error);
                  }
                );
              }}
              render={({ errors, status, touched, isSubmitting }) => (
                <Form>
                  <h3 className="card-header">Verify your password</h3>
                  {status && (
                    <div className={"alert alert-danger"}>{status}</div>
                  )}
                  <div className="form-group verify-margin-top">
                    <label htmlFor="reset_token">Token</label>
                    <Field
                      name="reset_token"
                      type="text"
                      className={
                        "form-control" +
                        (errors.reset_token && touched.reset_token
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="reset_token"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group ">
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
                  <div className="form-group ">
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
                  </div>
                  <ErrorMessage
                    name="passwordConfirmation"
                    component="div"
                    className="invalid-feedback"
                  />
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

export { VerifyPassword };
