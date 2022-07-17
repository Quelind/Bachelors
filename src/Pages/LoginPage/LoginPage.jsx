import { authenticationService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import { Link } from "react-router-dom";
import * as Yup from "yup";

class LoginPage extends React.Component {
  constructor(props) {
    super(props);

    if (authenticationService.currentUserValue) {
      this.props.history.push("/");
    }
  }

  render() {
    return (
      <div className="flexbox-container-2-login flexbox-container-3">
        <div className="row">
          <div className="col-md-6 offset-md-3 flexbox-container-login">
            <div className="form-row">
              <h2>Login</h2>
              <div className="float-right forgot-password-margin-left"></div>
            </div>
            <Formik
              initialValues={{
                username: "",
                password: "",
              }}
              validationSchema={Yup.object().shape({
                username: Yup.string().required("Username is required"),
                password: Yup.string().required("Password is required"),
              })}
              onSubmit={(
                { username, password },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                authenticationService.login(username, password).then(
                  (user) => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/" },
                    };
                    localStorage.setItem("currentUser", JSON.stringify(user));
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
                  <div className="form-group">
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
                  <div className="form-group">
                    <button
                      type="submit"
                      className="btn btn-primary"
                      disabled={isSubmitting}
                    >
                      Login
                    </button>

                    <button
                      type="submit"
                      className="btn btn-success float-right forgot-password-margin-left"
                      disabled={isSubmitting}
                    >
                      <Link style={{ color: "white" }} to={`/Register`}>
                        Register
                      </Link>
                    </button>
                    <Link
                      to="/ForgotPassword"
                      className="btn btn-link pr-0 float-right"
                    >
                      Forgot Password?
                    </Link>
                  </div>
                  {status && (
                    <div className={"alert alert-danger"}>
                      The username or password is incorrect
                    </div>
                  )}
                </Form>
              )}
            />
          </div>
          <div className="call-margin-top">
            <p>Call 867288611 to register manually</p>
          </div>
        </div>
      </div>
    );
  }
}

export { LoginPage };
