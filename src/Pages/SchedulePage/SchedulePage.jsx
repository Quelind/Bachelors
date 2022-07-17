import {
  authenticationService,
  scheduleService,
  visitService,
} from "@/Services";
import React, { Component } from "react";
import { Link } from "react-router-dom";

class SchedulePage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      days: null,
    };
  }

  componentDidMount() {
    scheduleService.getAllDays().then((days) => this.setState({ days }));
    visitService.getActiveVisits().then((visits) => this.setState({ visits }));
  }

  render() {
    {
      this.state.visits && console.log(this.state.visits);
    }
    const { currentUser, days, visits } = this.state;
    let times = [
      {
        time: "09:00",
        id: 0,
      },
      {
        time: "10:00",
        id: 1,
      },
      {
        time: "11:00",
        id: 2,
      },
      {
        time: "13:00",
        id: 3,
      },
      {
        time: "14:00",
        id: 4,
      },
      {
        time: "15:00",
        id: 5,
      },
      {
        time: "16:00",
        id: 6,
      },
    ];
    return (
      <div>
        <div className="form-row flexbox-row">
          <div className="form-group col margin-from-top">
            {currentUser.role === "Admin" && <h1>All schedules</h1>}
            {currentUser.role === "Employee" && <h1>My schedule</h1>}
          </div>
        </div>
        <div className="container">
          <div className="table-responsive">
            <table className="table table-bordered text-center schedule-margin-top">
              <thead>
                <tr className="bg-light-gray">
                  <th className="text-uppercase">Times</th>
                  {days &&
                    days.map((day) => (
                      <th className="text-uppercase">{day.date}</th>
                    ))}
                </tr>
              </thead>
              <tbody>
                {times &&
                  times.map((time) => (
                    <tr key={times.id}>
                      <td className="align-middle">{time.time}</td>
                      {days &&
                        days.map((day) => (
                          <td style={{ whiteSpace: "nowrap" }}>
                            {visits &&
                              visits
                                .filter((visit) =>
                                  currentUser.role === "Employee"
                                    ? visit.doctor_name == currentUser.name &&
                                      visit.doctor_surname ==
                                        currentUser.surname &&
                                      visit.date == day.date &&
                                      visit.time == time.time
                                    : visit.date == day.date &&
                                      visit.time == time.time
                                )
                                .map((visit) => (
                                  <div className="row">
                                    <button className="button-main get-button small-margin left-margin">
                                      <Link
                                        style={{ color: "white" }}
                                        to={`/GetVisit/${visit.id}`}
                                      >
                                        {currentUser.role === "Employee"
                                          ? visit.patient_name +
                                            " " +
                                            visit.patient_surname
                                          : visit.doctor_name +
                                            " " +
                                            visit.doctor_surname}
                                      </Link>
                                    </button>
                                  </div>
                                ))}
                          </td>
                        ))}
                    </tr>
                  ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    );
  }
}

export { SchedulePage };
