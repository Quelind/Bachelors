import {
  authenticationService,
  employeeService,
  patientService,
  procedureService,
  roomService,
  visitService,
} from "@/Services";
import { ErrorMessage, Field, Form, Formik } from "formik";
import React from "react";
import * as Yup from "yup";
import { timetableService } from "../../Services/timetable.service";
class AddVisit extends React.Component {
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
      fk_user: 0,
      name: null,
      specialization: null,
      surname: null,
      id: null,
      timetables: null,
      procedures: null,
      fk_procedure: null,
      history: null,
      count: 0,
    };
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Employees`);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let search = url.substring(url.lastIndexOf("/") + 1);
    timetableService
      .getAllTimetablesUnlocked(`fk${search}doc`)
      .then((timetables) => this.setState({ timetables }));
    {
      this.state.currentUser &&
        this.state.currentUser.role === "Admin" &&
        roomService.getAllRooms().then((result) => {
          this.setState({
            rooms: result.map(({ name }) => name),
          });
        });
    }
    {
      this.state.currentUser &&
        (this.state.currentUser.role === "Admin" ||
          this.state.currentUser.role === "User") &&
        employeeService.getAllEmployees().then((result) => {
          this.setState({
            employees: result.map((o) => ({
              name: o.name,
              surname: o.surname,
              specialization: o.specialization,
              id: o.id,
              fk_user: o.fk_user,
            })),
          });
        });
    }
    {
      this.state.currentUser &&
        (this.state.currentUser.role === "Admin" ||
          this.state.currentUser.role === "Employee") &&
        patientService.getAllPatients().then((result) => {
          this.setState({
            patients: result.map((o) => ({
              name: o.name,
              surname: o.surname,
              id: o.id,
              fk_user: o.fk_user,
            })),
          });
        });
    }
    employeeService
      .getEmployee(parseInt(search))
      .then((visitEmployee) => this.setState({ visitEmployee }));

    employeeService
      .getRoomByEmployee(parseInt(search))
      .then((visitRoom) => this.setState({ visitRoom }));
    procedureService
      .getAllProcedures()
      .then((procedures) => this.setState({ procedures }));
    {
      this.state.currentUser &&
        this.state.currentUser.role === "User" &&
        patientService
          .getPatientByUser(this.state.currentUser.id)
          .then((currentPatient) => {
            this.setState({ currentPatient });
          });
    }
  }
  render() {
    const { currentUser, procedures } = this.state;
    return (
      <div>
        <div className="row">
          <div className="col-md-6 offset-md-3 margin-from-top">
            <Formik
              initialValues={{
                patient_comment: "",
                fk_patient: currentUser.role === "User" ? null : 0,
                fk_timetable: 0,
                Patient_history: null,
                Patient_description: null,
              }}
              validationSchema={Yup.object().shape({
                fk_timetable: Yup.number().required("Date is required"),
                patient_comment: Yup.string().required(
                  "Patient Comment is required"
                ),
              })}
              onSubmit={(
                {
                  fk_timetable,
                  patient_comment,
                  fk_patient,
                  fk_doctor,
                  fk_room,
                  fk_procedure,
                  Patient_history,
                  Patient_description,
                },
                { setStatus, setSubmitting }
              ) => {
                setStatus();
                const Visit = {
                  fk_timetable: parseInt(fk_timetable),
                  patient_comment: patient_comment,
                  fk_patient:
                    currentUser.role === "User"
                      ? parseInt(this.state.currentPatient.id)
                      : parseInt(fk_patient),
                  fk_doctor: this.state.visitEmployee.id,
                  fk_room: this.state.visitRoom.name,
                  fk_procedure:
                    currentUser.role === "User" ? 0 : parseInt(fk_procedure),
                  Patient_history: Patient_history,
                  Patient_description: Patient_description,
                };
                visitService.addVisit(Visit).then(
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
                  <h2>Register a visit</h2>
                  {currentUser && currentUser.role == "User" && (
                    <div className="form-group">
                      <label htmlFor="fk_patient">Patient</label>
                      <Field
                        name="fk_patient"
                        type="text"
                        defaultValue={
                          currentUser.name + " " + currentUser.surname
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
                  {this.state.visitEmployee && (
                    <div className="form-group">
                      <label htmlFor="fk_doctor">Doctor</label>
                      <Field
                        name="fk_doctor"
                        type="text"
                        defaultValue={
                          this.state.visitEmployee.name +
                          " " +
                          this.state.visitEmployee.surname +
                          " (" +
                          this.state.visitEmployee.specialization +
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
                  {this.state.visitRoom && (
                    <div className="form-group">
                      <label htmlFor="fk_room">Room</label>
                      <Field
                        name="fk_room"
                        type="text"
                        defaultValue={this.state.visitRoom.name}
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
                  {currentUser.role === "User" && this.state.procedures && (
                    <div className="form-group">
                      <label htmlFor="fk_procedure">Procedure</label>
                      <Field
                        name="fk_procedure"
                        type="text"
                        defaultValue={procedures[0].name}
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
                  {currentUser.role != "User" &&
                    this.state.procedures &&
                    this.state.visitEmployee && (
                      <div>
                        <label htmlFor="fk_procedure">Procedure</label>
                        <Field
                          as="select"
                          className="custom-select d-block w-100"
                          name="fk_procedure"
                          required
                          defaultValue=""
                        >
                          <option></option>
                          {this.state.procedures
                            .filter(
                              (procedures) =>
                                procedures.requirement ===
                                  this.state.visitEmployee.specialization ||
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
                  {this.state.patients && currentUser.role != "User" && (
                    <div>
                      <label htmlFor="fk_patient">Patient</label>
                      <Field
                        as="select"
                        className="custom-select d-block w-100"
                        name="fk_patient"
                        required
                      >
                        <option></option>
                        {this.state.patients.map((patients) => (
                          <option value={patients.id} selected>
                            {patients.name} {patients.surname}
                          </option>
                        ))}
                      </Field>
                      <div className="invalid-feedback">
                        Please select a valid patient.
                      </div>
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
                  <div className="mt-3"></div>
                  {currentUser.role != "User" && (
                    <div
                      onChange={this.onChangeValue}
                      onClick={() =>
                        this.setState({ count: this.state.count + 1 })
                      }
                    >
                      <input type="radio" value="History" name="gender" />{" "}
                      History?
                    </div>
                  )}

                  {this.state.count > 0 && (
                    <div className="form-group">
                      <label htmlFor="Patient_history">Disease name</label>
                      <Field
                        name="Patient_history"
                        type="text"
                        className={
                          "form-control" +
                          (errors.Patient_history && touched.patient_comment
                            ? " is-invalid"
                            : "")
                        }
                      />
                      <ErrorMessage
                        name="Patient_history"
                        component="div"
                        className="invalid-feedback"
                      />
                    </div>
                  )}
                  {this.state.count > 0 && (
                    <div className="form-group">
                      <label htmlFor="Patient_description">
                        Disease description
                      </label>
                      <Field
                        name="Patient_description"
                        type="text"
                        className={
                          "form-control" +
                          (errors.Patient_description &&
                          touched.Patient_description
                            ? " is-invalid"
                            : "")
                        }
                      />
                      <ErrorMessage
                        name="Patient_description"
                        component="div"
                        className="invalid-feedback"
                      />
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

export { AddVisit };
