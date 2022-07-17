import { authenticationService, userService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class ForgotPassword extends React.Component {
  constructor(props) {
    super(props);
    this.state = {};
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/login`);
  }

  render() {
    return (
      <div className="flexbox-container-2-login flexbox-container-3">
        <div className="row">
          <div className="col-md-6 offset-md-3 flexbox-container-login">
            <Formik
              initialValues={{
                username: "",
              }}
              validationSchema={Yup.object().shape({
                username: Yup.string().required("Username is required"),
              })}
              onSubmit={({ username }, { setStatus, setSubmitting }) => {
                setStatus();
                const User = {
                  username: username,
                };
                userService.forgotPassword(User).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/VerifyPassword" },
                    };
                    this.props.history.push(from);
                  },
                  (error) => {
                    setSubmitting(false);
                    setStatus(error);
                  }
                );
              }}
              render={({ errors, touched, isSubmitting }) => (
                <Form>
                  <h2>Reset password</h2>
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
                      Reset
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

export { ForgotPassword };
