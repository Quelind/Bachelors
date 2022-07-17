import { authenticationService, roomService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class AddRoom extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      name: null,
      type: null,
      capacity: 0,
    };
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Rooms`);
  }
  render() {
    return (
      <div>
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              initialValues={{
                name: "",
                type: "",
                capacity: 1,
              }}
              validationSchema={Yup.object().shape({
                name: Yup.string().required("Name is required"),
                type: Yup.string().required("Type is required"),
                capacity: Yup.number().required("Capacity is required"),
              })}
              onSubmit={(
                { name, type, capacity },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const Room = {
                  name: name,
                  type: type,
                  capacity: capacity,
                };
                roomService.addRoom(Room).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/Rooms" },
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
                  <h2>Add Room</h2>
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
                    <label htmlFor="type">Type</label>
                    <Field
                      name="type"
                      type="text"
                      className={
                        "form-control" +
                        (errors.type && touched.type ? " is-invalid" : "")
                      }
                    />
                    <ErrorMessage
                      name="type"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="capacity">Capacity</label>
                    <Field
                      name="capacity"
                      type="number"
                      className={
                        "form-control" +
                        (errors.capacity && touched.capacity
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="capacity"
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

export { AddRoom };
