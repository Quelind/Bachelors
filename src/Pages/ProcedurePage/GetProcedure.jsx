import {
  authenticationService,
  procedureService,
  roomService,
} from "@/Services";
import { Field, Form, Formik } from "formik";
import React from "react";
import { Link } from "react-router-dom";

class GetProcedure extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      procedure: null,
      name: null,
      requirement: null,
      room_type: null,
      information: null,
      duration: null,
      personnel_count: null,
      image: null,
    };
    this.handleReturn = this.handleReturn.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Procedures`);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    procedureService.getProcedure(id).then((procedure) => {
      this.setState(procedure);
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
                personnel_count: this.state.personnel_count,
                image: this.state.image,
                price: this.state.price,
              }}
              render={() => (
                <Form>
                  <h2>More information:</h2>
                  <div className="form-row">
                    <div className="form-group col">
                      <div className="form-group">
                        <label htmlFor="name">Name</label>
                        <Field
                          name="name"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="requirement">Requirement</label>
                        <Field
                          name="requirement"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="room_type">Room type</label>
                        <Field
                          name="room_type"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <button
                        className="btn btn-danger"
                        type="button"
                        id="button-addon2"
                        onClick={this.handleReturn}
                      >
                        Back
                      </button>
                    </div>
                    <div className="form-group col">
                      <div className="form-group">
                        <label htmlFor="duration">Duration</label>
                        <Field
                          name="duration"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="price">Price</label>
                        <Field
                          name="price"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      <div className="form-group">
                        <label htmlFor="information">Information</label>
                        <Field
                          name="information"
                          type="text"
                          className={"form-control"}
                          readOnly
                        />
                      </div>
                      {this.state.currentUser &&
                        (this.state.currentUser.role === "Admin" ||
                          this.state.currentUser.role === "Employee") && (
                          <button className="btn btn-warning get-employee-back-button-margin-left2">
                            <Link
                              style={{ color: "white" }}
                              to={`/EditProcedure/${id}`}
                            >
                              Edit
                            </Link>
                          </button>
                        )}
                    </div>
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

export { GetProcedure };
