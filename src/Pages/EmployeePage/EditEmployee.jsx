import {
  authenticationService,
  employeeService,
  roomService,
} from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class EditEmployee extends React.Component {
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
    roomService.getAllRooms().then((result) => {
      this.setState({
        rooms: result.map(({ name }) => name),
      });
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
                name: this.state.name,
                surname: this.state.surname,
                specialization: this.state.specialization,
                birthdate: this.state.birthdate,
                phone: this.state.phone,
                email: this.state.email,
                fk_room: this.state.fk_room,
                image: this.state.image,
                id: this.state.id,
                fk_user: this.state.fk_user,
              }}
              validationSchema={Yup.object().shape({
                name: Yup.string()
                  .min(3, "Name must be at least 3 characters")
                  .required("Name is required"),
                surname: Yup.string()
                  .min(3, "Surname must be at least 3 characters")
                  .required("Surname is required"),
                specialization: Yup.string().required(
                  "Specialization is required"
                ),
                birthdate: Yup.string()
                  .required("Birthdate is required")
                  .matches(
                    /^\d{4}\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])$/,
                    "The date format is invalid."
                  ),
                phone: Yup.string().required("Phone is required"),
                email: Yup.string()
                  .email("Email format invalid")
                  .required("Email is required"),
                fk_room: Yup.string().required("Room is required"),
              })}
              onSubmit={(
                {
                  name,
                  surname,
                  specialization,
                  birthdate,
                  phone,
                  email,
                  fk_room,
                  image,
                  fk_user,
                  id,
                },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const Employee = {
                  name: name,
                  surname: surname,
                  specialization: specialization,
                  birthdate: birthdate,
                  phone: phone,
                  email: email,
                  fk_room: fk_room,
                  image: image,
                  id: id,
                  fk_user: fk_user,
                };
                employeeService.editEmployee(Employee, id).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/Employees" },
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
                  <h2>Edit Employee</h2>

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
                    <label htmlFor="specialization">Specialization</label>
                    <Field
                      name="specialization"
                      type="text"
                      className={
                        "form-control" +
                        (errors.specialization && touched.specialization
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="specialization"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="birthdate">Birthdate</label>
                    <Field
                      name="birthdate"
                      type="text"
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
                  {this.state.rooms && (
                    <div>
                      <label htmlFor="fk_room">Room</label>
                      <Field
                        as="select"
                        className="custom-select d-block w-100"
                        name="fk_room"
                        required
                        defaultValue=""
                      >
                        <option></option>
                        {this.state.rooms.map((rooms) => (
                          <option value={rooms} selected>
                            {" "}
                            {rooms}
                          </option>
                        ))}
                      </Field>
                      <div className="invalid-feedback">
                        Please select a valid room.
                      </div>
                    </div>
                  )}
                  <div className="form-group">
                    <label htmlFor="image">Image Link</label>
                    <Field
                      name="image"
                      type="text"
                      className={
                        "form-control" +
                        (errors.image && touched.image ? " is-invalid" : "")
                      }
                    />
                    <ErrorMessage
                      name="image"
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

export { EditEmployee };
