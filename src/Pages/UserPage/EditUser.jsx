import { authenticationService, userService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class EditUser extends React.Component {
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

  componentDidMount() {
    userService.getUser(this.state.currentUser.id).then((users) => {
      this.setState(users);
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
                birthdate: this.state.birthdate,
                phone: this.state.phone,
                email: this.state.email,
              }}
              validationSchema={Yup.object().shape({
                name: Yup.string().required("Name is required"),
                surname: Yup.string().required("Surname is required"),
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
                { name, surname, birthdate, phone, email, id },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const User = {
                  name: name,
                  surname: surname,
                  birthdate: birthdate,
                  phone: phone,
                  email: email,
                  id: id,
                };
                var newUser = {
                  birthdate: birthdate,
                  email: email,
                  id: this.state.currentUser.id,
                  name: name,
                  password: null,
                  phone: phone,
                  role: this.state.currentUser.role,
                  surname: surname,
                  token: this.state.currentUser.token,
                  username: this.state.currentUser.username,
                  verified: this.state.currentUser.verified,
                };
                localStorage.setItem("currentUser", JSON.stringify(newUser));
                userService.editUser(User, id).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/User" },
                    };
                    this.props.history.push(from);
                    location.reload(true);
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
                  <h2>Edit User</h2>

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

export { EditUser };
