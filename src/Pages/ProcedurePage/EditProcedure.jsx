import { authenticationService, procedureService } from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class EditProcedure extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      procedures: null,
      name: null,
      requirement: null,
      room_type: null,
      information: null,
      duration: 0,
      image: "",
      price: 0,
    };
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Procedures`);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    procedureService.getProcedure(id).then((procedures) => {
      this.setState(procedures);
    });
  }

  render() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    return (
      <div>
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              enableReinitialize
              initialValues={{
                name: this.state.name,
                requirement: this.state.requirement,
                room_type: this.state.room_type,
                information: this.state.information,
                duration: this.state.duration,
                image: this.state.image,
                price: this.state.price,
              }}
              validationSchema={Yup.object().shape({
                name: Yup.string().required("Name is required"),
                requirement: Yup.string().required("Requirement is required"),
                room_type: Yup.string().required("Room Type is required"),
                information: Yup.string().required("Information is required"),
                duration: Yup.number().required("Duration is required"),
                price: Yup.number().required("Price is required"),
              })}
              onSubmit={(
                {
                  name,
                  requirement,
                  room_type,
                  information,
                  duration,
                  image,
                  price,
                },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const Procedure = {
                  name: name,
                  requirement: requirement,
                  room_type: room_type,
                  information: information,
                  duration: duration,
                  image: image,
                  price: price,
                };
                procedureService.editProcedure(Procedure, id).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/Procedures" },
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
                  <h2>Edit Procedure</h2>

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
                    <label htmlFor="requirement">Requirement</label>
                    <Field
                      name="requirement"
                      type="text"
                      className={
                        "form-control" +
                        (errors.requirement && touched.requirement
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="requirement"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
                  <div className="form-group">
                    <label htmlFor="room_type">Room Type</label>
                    <Field
                      name="room_type"
                      type="text"
                      className={
                        "form-control" +
                        (errors.room_type && touched.room_type
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="room_type"
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
                    <label htmlFor="duration">Duration</label>
                    <Field
                      name="duration"
                      type="number"
                      className={
                        "form-control" +
                        (errors.duration && touched.duration
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="duration"
                      component="div"
                      className="invalid-feedback"
                    />
                  </div>
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
                  <div className="form-group">
                    <label htmlFor="price">Price</label>
                    <Field
                      name="price"
                      type="number"
                      className={
                        "form-control" +
                        (errors.price && touched.price ? " is-invalid" : "")
                      }
                    />
                    <ErrorMessage
                      name="price"
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

export { EditProcedure };
