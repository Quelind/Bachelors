import { authenticationService, userService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class ChangePassword extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
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
    this.props.history.push(`/User`);
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
                id: currentUser.id,
                oldPassword: "",
                passwordConfirmation: "",
                newPassword: "",
                username: currentUser.username,
              }}
              validationSchema={Yup.object().shape({
                oldPassword: Yup.string().required("Password is required"),
                newPassword: Yup.string()
                  .min(8, "Password must be at least 8 characters")
                  .required("Password is required"),
                passwordConfirmation: Yup.string()
                  .oneOf([Yup.ref("newPassword"), null], "Passwords must match")
                  .required("Password Confirmation is required"),
              })}
              onSubmit={(
                { oldPassword, newPassword, username, id },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const User = {
                  oldPassword: oldPassword,
                  newPassword: newPassword,
                  id,
                  username,
                };
                userService.changePassword(User, id).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/User" },
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
                  <h2>Change Password</h2>
                  <label htmlFor="oldPassword">Old password</label>
                  <Field
                    name="oldPassword"
                    type="password"
                    className={
                      "form-control" +
                      (errors.oldPassword && touched.oldPassword
                        ? " is-invalid"
                        : "")
                    }
                  />
                  <ErrorMessage
                    name="oldPassword"
                    component="div"
                    className="invalid-feedback"
                  />
                  <label htmlFor="newPassword">Password</label>
                  <Field
                    name="newPassword"
                    type="password"
                    className={
                      "form-control" +
                      (errors.newPassword && touched.newPassword
                        ? " is-invalid"
                        : "")
                    }
                  />
                  <ErrorMessage
                    name="newPassword"
                    component="div"
                    className="invalid-feedback"
                  />
                  <label htmlFor="password">Password Confirmation</label>
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

export { ChangePassword };
