import React from "react";
import { Formik, Field, Form, ErrorMessage } from "formik";
import * as Yup from "yup";
import { Button, Modal } from "react-bootstrap";
import {
  visitService,
  roomService,
  patientService,
  employeeService,
  authenticationService,
  timetableService,
  procedureService,
} from "@/Services";
import { history } from "@/Helpers";
import { Link } from "react-router-dom";

class ChangeDoctor extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      visits: null,
      date: null,
      time: null,
      field: null,
      patient_comment: null,
      fk_patient: 0,
      fk_doctor: 0,
      fk_room: null,
      name: null,
      specialization: null,
      surname: null,
      id: null,
    };
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Visits`);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    const urlArray = id.split("+");
    visitService.getVisit(parseInt(urlArray[0])).then((visits) => {
      this.setState(visits);
    });
    employeeService
      .getEmployeesByTimetable(urlArray[1])
      .then((doctors) => this.setState({ doctors }));
  }
  render() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    const urlArray = id.split("+");
    const { currentUser } = this.state;

    return (
      <div>
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              enableReinitialize
              initialValues={{
                fk_doctor: this.state.fk_doctor,
                fk_timetable: this.state.fk_timetable,
              }}
              validationSchema={Yup.object().shape({
                fk_doctor: Yup.number().required("Doctor is required"),
              })}
              onSubmit={({ fk_doctor }, { setStatus, setSubmitting }) => {
                setStatus();
                const Visit = {
                  fk_doctor: parseInt(fk_doctor),
                  fk_timetable: parseInt(this.state.fk_timetable),
                };
                visitService.changeDoctor(Visit, urlArray[0]).then(
                  () => {
                    const { from } = this.props.location.state || {
                      from: { pathname: "/Visits" },
                    };
                    this.props.history.push(from);
                  },
                  (error) => {
                    setSubmitting(false);
                    setStatus(error);
                  }
                );
              }}
              render={({ status, isSubmitting }) => (
                <Form>
                  {status && (
                    <div className={"alert alert-danger"}>{status}</div>
                  )}
                  <h2>Change Doctor</h2>
                  {this.state.doctors && currentUser.role === "Admin" && (
                    <div>
                      <label htmlFor="fk_doctor">Doctor</label>
                      <Field
                        as="select"
                        className="custom-select d-block w-100"
                        name="fk_doctor"
                        required
                      >
                        {this.state.doctors.map((doctors) => (
                          <option value={doctors.id} selected>
                            {doctors.name} {doctors.surname}
                          </option>
                        ))}
                      </Field>
                      <div className="invalid-feedback">
                        Please select a valid doctor.
                      </div>
                    </div>
                  )}
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

export { ChangeDoctor };
