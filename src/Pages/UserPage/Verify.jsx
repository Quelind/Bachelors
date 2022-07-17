import { authenticationService, userService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class Verify extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
    };
  }

  render() {
    return (
      <div>
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              initialValues={{
                token: "",
              }}
              validationSchema={Yup.object().shape({
                token: Yup.string().required("Token is required"),
              })}
              onSubmit={({ token }, { setStatus, setSubmitting }) => {
                setStatus();
                const Verify = {
                  token: token,
                };
                userService.verifyEmail(Verify).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/login" },
                    };
                    authenticationService.logout();
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
                  <h3 className="card-header">Verify your email address</h3>
                  <div className="form-group verify-margin-top">
                    {status && (
                      <div className={"alert alert-danger"}>{status}</div>
                    )}
                    <label htmlFor="email">Email</label>
                    <Field
                      name="email"
                      type="text"
                      defaultValue={this.state.currentUser.email}
                      readOnly
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
                  <div className="form-group">
                    <label htmlFor="token">Token</label>
                    <Field
                      name="token"
                      type="text"
                      className={
                        "form-control" +
                        (errors.token && touched.token ? " is-invalid" : "")
                      }
                    />
                    <ErrorMessage
                      name="token"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group ">
                    <button
                      type="submit"
                      className="btn btn-primary"
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

export { Verify };
