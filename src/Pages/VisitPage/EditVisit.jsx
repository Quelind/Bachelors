import {
  authenticationService,
  procedureService,
  timetableService,
  visitService,
} from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";

class EditVisit extends React.Component {
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
    visitService.getVisit(parseInt(id)).then((visits) => {
      this.setState(visits);
    });
    visitService
      .getVisit(id)
      .then((currentVisit) => this.setState({ currentVisit }));
    procedureService
      .getAllProcedures()
      .then((procedures) => this.setState({ procedures }));
    timetableService
      .getAllTimetables()
      .then((timetables) => this.setState({ timetables }));
  }

  render() {
    const { currentUser } = this.state;
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    return (
      <div>
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              enableReinitialize
              initialValues={{
                patient_comment: this.state.patient_comment,
                fk_patient: this.state.fk_patient,
                fk_doctor: this.state.fk_doctor,
                fk_room: this.state.fk_room,
                fk_timetable: this.state.fk_timetable,
                fk_procedure: this.state.fk_procedure,
                doctor_name: this.state.doctor_name,
              }}
              validationSchema={Yup.object().shape({
                patient_comment: Yup.string().required(
                  "Patient Comment is required"
                ),
                fk_doctor: Yup.number().required("Doctor is required"),
                fk_room: Yup.string().required("Room is required"),
              })}
              onSubmit={(
                {
                  patient_comment,
                  fk_patient,
                  fk_doctor,
                  fk_room,
                  fk_timetable,
                  fk_procedure,
                },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const Visit = {
                  patient_comment: patient_comment,
                  fk_patient: parseInt(fk_patient),
                  fk_doctor: parseInt(fk_doctor),
                  fk_room: fk_room,
                  fk_timetable: parseInt(fk_timetable),
                  fk_procedure: parseInt(fk_procedure),
                };
                visitService.editVisit(Visit, id).then(
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
              render={({ errors, status, touched, isSubmitting }) => (
                <Form>
                  {status && (
                    <div className={"alert alert-danger"}>{status}</div>
                  )}
                  <h2>Edit Visit</h2>
                  {this.state.currentVisit && (
                    <div className="form-group">
                      <label htmlFor="fk_patient">Patient</label>
                      <Field
                        name="asdas"
                        type="text"
                        defaultValue={
                          this.state.currentVisit.patient_name +
                          " " +
                          this.state.currentVisit.patient_surname
                        }
                        readOnly
                        className={
                          "form-control" +
                          (errors.fk_patient && touched.fk_patient
                            ? " is-invalid"
                            : "")
                        }
                      />
                      <ErrorMessage
                        name="fk_patient"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
                  )}
                  {this.state.currentVisit && (
                    <div className="form-group">
                      <label htmlFor="fk_doctor">Doctor</label>
                      <Field
                        name="asdasdas"
                        type="text"
                        defaultValue={
                          this.state.currentVisit.doctor_name +
                          " " +
                          this.state.currentVisit.doctor_surname +
                          " (" +
                          this.state.currentVisit.specialization +
                          ")"
                        }
                        readOnly
                        className={
                          "form-control" +
                          (errors.fk_doctor && touched.fk_doctor
                            ? " is-invalid"
                            : "")
                        }
                      />
                      <ErrorMessage
                        name="fk_doctor"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
                  )}
                  {this.state.currentVisit && (
                    <div className="form-group">
                      <label htmlFor="fk_room">Room</label>
                      <Field
                        name="asdasasddas"
                        type="text"
                        defaultValue={this.state.currentVisit.fk_room}
                        readOnly
                        className={
                          "form-control" +
                          (errors.fk_room && touched.fk_room
                            ? " is-invalid"
                            : "")
                        }
                      />
                      <ErrorMessage
                        name="fk_room"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
                  )}
                  {this.state.currentVisit &&
                    currentUser &&
                    currentUser.role === "User" && (
                      <div className="form-group">
                        <label htmlFor="fk_procedure">Procedure</label>
                        <Field
                          name="asdaasdassasddas"
                          type="text"
                          defaultValue={this.state.currentVisit.procedure_name}
                          readOnly
                          className={
                            "form-control" +
                            (errors.fk_procedure && touched.fk_procedure
                              ? " is-invalid"
                              : "")
                          }
                        />
                        <ErrorMessage
                          name="fk_procedure"
                          component="div"
                          className="invalid-feedback"
                        />
                      </div>
                    )}
                  {this.state.timetables && (
                    <div>
                      <label htmlFor="fk_timetable">Date</label>
                      <Field
                        as="select"
                        className="custom-select d-block w-100"
                        name="fk_timetable"
                        required
                      >
                        <option></option>
                        {this.state.timetables.map((timetables) => (
                          <option value={timetables.id} selected>
                            {timetables.date} | {timetables.time}
                          </option>
                        ))}
                      </Field>
                      <div className="invalid-feedback">
                        Please select a valid date.
                      </div>
                    </div>
                  )}
                  {this.state.procedures &&
                    this.state.currentVisit &&
                    currentUser &&
                    currentUser.role != "User" && (
                      <div>
                        <label htmlFor="fk_procedure">Procedure</label>
                        <Field
                          as="select"
                          className="custom-select d-block w-100"
                          name="fk_procedure"
                          required
                        >
                          <option></option>{" "}
                          {this.state.procedures
                            .filter(
                              (procedures) =>
                                procedures.requirement ===
                                  this.state.currentVisit.specialization ||
                                procedures.name === "Initial visit"
                            )
                            .map((procedures) => (
                              <option value={procedures.id} selected>
                                {procedures.name}
                              </option>
                            ))}
                        </Field>
                        <div className="invalid-feedback">
                          Please select a valid procedure.
                        </div>
                      </div>
                    )}
                  <div className="form-group">
                    <label htmlFor="patient_comment">Patient Comment</label>
                    <Field
                      name="patient_comment"
                      type="text"
                      className={
                        "form-control" +
                        (errors.patient_comment && touched.patient_comment
                          ? " is-invalid"
                          : "")
                      }
                    />
                    <ErrorMessage
                      name="patient_comment"
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

export { EditVisit };
