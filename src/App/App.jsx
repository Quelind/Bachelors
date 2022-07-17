import { PrivateRoute } from "@/Components";
import { history } from "@/Helpers";
import { EmployeePage } from "@/Pages/EmployeePage";
import { AddEmployee } from "@/Pages/EmployeePage/AddEmployee";
import { EditEmployee } from "@/Pages/EmployeePage/EditEmployee";
import { EmployeeSearch } from "@/Pages/EmployeePage/EmployeeSearch";
import { GetEmployee } from "@/Pages/EmployeePage/GetEmployee";
import { HistoryPage } from "@/Pages/HistoryPage";
import { HistorySearch } from "@/Pages/HistoryPage/HistorySearch";
import { HomePage } from "@/Pages/HomePage";
import { LoginPage } from "@/Pages/LoginPage";
import { Register } from "@/Pages/LoginPage/Register";
import { PatientPage } from "@/Pages/PatientPage";
import { AddPatient } from "@/Pages/PatientPage/AddPatient";
import { EditPatient } from "@/Pages/PatientPage/EditPatient";
import { GetPatient } from "@/Pages/PatientPage/GetPatient";
import { PatientSearch } from "@/Pages/PatientPage/PatientSearch";
import { ProcedurePage } from "@/Pages/ProcedurePage";
import { AddProcedure } from "@/Pages/ProcedurePage/AddProcedure";
import { EditProcedure } from "@/Pages/ProcedurePage/EditProcedure";
import { GetProcedure } from "@/Pages/ProcedurePage/GetProcedure";
import { ProcedureSearch } from "@/Pages/ProcedurePage/ProcedureSearch";
import { RoomPage } from "@/Pages/RoomPage";
import { AddRoom } from "@/Pages/RoomPage/AddRoom";
import { EditRoom } from "@/Pages/RoomPage/EditRoom";
import { RoomSearch } from "@/Pages/RoomPage/RoomSearch";
import { SchedulePage } from "@/Pages/SchedulePage";
import { UserPage } from "@/Pages/UserPage";
import { PaymentPage } from "@/Pages/PaymentPage";
import { Verify } from "@/Pages/UserPage/Verify";
import { EditUser } from "@/Pages/UserPage/EditUser";
import { ChangePassword } from "@/Pages/UserPage/ChangePassword";
import { ForgotPassword } from "@/Pages/LoginPage/ForgotPassword";
import { VisitPage } from "@/Pages/VisitPage";
import { AddVisit } from "@/Pages/VisitPage/AddVisit";
import { ChangeDoctor } from "@/Pages/VisitPage/ChangeDoctor";
import { EditVisit } from "@/Pages/VisitPage/EditVisit";
import { GetVisit } from "@/Pages/VisitPage/GetVisit";
import { VisitSearch } from "@/Pages/VisitPage/VisitSearch";
import { authenticationService } from "@/Services";
import React from "react";
import { Link, Route, Router } from "react-router-dom";
import { VerifyPassword } from "../Pages/UserPage/VerifyPassword";
import "../styles.less";

class App extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      currentUser: null,
      message: null,
      succeeded: null,
    };
    this.alertNotify = this.alertNotify.bind(this);
  }

  alertNotify(message, succeeded) {
    this.setState({ message, succeeded });
  }
  componentDidMount() {
    authenticationService.currentUser.subscribe((x) =>
      this.setState({ currentUser: x })
    );
  }

  logout() {
    authenticationService.logout();
    history.push("/login");
  }

  render() {
    const { currentUser } = this.state;
    return (
      <Router history={history}>
        <div>
          {currentUser && (
            <nav className="navbar navbar-expand navbar-dark background-color-blue">
              <div className="navbar-nav">
                <div className="button-navbar">
                  <Link
                    style={{ color: "white" }}
                    to="/"
                    className="nav-item nav-link"
                  >
                    Home
                  </Link>
                </div>
                {currentUser.role != "Employee" && (
                  <div className="button-navbar">
                    <Link
                      style={{ color: "white" }}
                      to="/Employees"
                      className="nav-item nav-link"
                    >
                      Employees
                    </Link>
                  </div>
                )}
                {(currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <div className="button-navbar">
                    <Link
                      style={{ color: "white" }}
                      to="/Patients"
                      className="nav-item nav-link"
                    >
                      Patients
                    </Link>
                  </div>
                )}
                <div className="button-navbar">
                  <Link
                    style={{ color: "white" }}
                    to="/Procedures"
                    className="nav-item nav-link"
                  >
                    Procedures
                  </Link>
                </div>
                {currentUser.role === "DoNotDisplay" && (
                  <div className="button-navbar">
                    <Link
                      style={{ color: "white" }}
                      to="/Rooms"
                      className="nav-item nav-link"
                    >
                      Rooms
                    </Link>
                  </div>
                )}
                {currentUser.role != "Unconfirmed" && (
                  <div className="button-navbar">
                    <Link
                      style={{ color: "white" }}
                      to="/Visits"
                      className="nav-item nav-link"
                    >
                      Visits
                    </Link>
                  </div>
                )}
                {(currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <div className="button-navbar">
                    <Link
                      style={{ color: "white" }}
                      to="/Schedule"
                      className="nav-item nav-link"
                    >
                      Schedule
                    </Link>
                  </div>
                )}
                {(currentUser.role === "Admin" ||
                  currentUser.role === "User") && (
                  <div className="button-navbar">
                    <Link
                      style={{ color: "white" }}
                      to="/Payments"
                      className="nav-item nav-link"
                    >
                      Payments
                    </Link>
                  </div>
                )}
              </div>
              <div className="navbar-nav flexbox-navbar background-color-blue">
                {currentUser.role != "Unconfirmed" && (
                  <div className="button-navbar button-navbar-yellow">
                    <Link
                      style={{ color: "white" }}
                      to="/User"
                      className="nav-item nav-link"
                    >
                      {currentUser.username}
                    </Link>
                  </div>
                )}
                <div className="button-navbar button-navbar-red">
                  <a
                    onClick={this.logout}
                    style={{ color: "white" }}
                    className="nav-item nav-link"
                  >
                    Logout
                  </a>
                </div>
              </div>
            </nav>
          )}

          <div className="flexbox-container-2">
            <div className="container">
              {currentUser && (
                <PrivateRoute exact path="/" component={HomePage} />
              )}
              <PrivateRoute exact path="/Employees" component={EmployeePage} />
              {currentUser && currentUser.role === "Admin" && (
                <PrivateRoute
                  exact
                  path="/AddEmployee"
                  component={AddEmployee}
                />
              )}
              {currentUser && currentUser.role === "Admin" && (
                <PrivateRoute
                  exact
                  path="/EditEmployee/:id"
                  component={EditEmployee}
                />
              )}
              {currentUser && (
                <PrivateRoute
                  exact
                  path="/GetEmployee/:id"
                  component={GetEmployee}
                />
              )}
              {currentUser && (
                <PrivateRoute
                  exact
                  path="/EmployeeSearch/:search"
                  component={EmployeeSearch}
                />
              )}
              {currentUser &&
                (currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <PrivateRoute
                    exact
                    path="/Patients"
                    component={PatientPage}
                  />
                )}
              {currentUser &&
                (currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <PrivateRoute
                    exact
                    path="/AddPatient"
                    component={AddPatient}
                  />
                )}
              {currentUser &&
                (currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <PrivateRoute
                    exact
                    path="/EditPatient/:id"
                    component={EditPatient}
                  />
                )}
              {currentUser &&
                (currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <PrivateRoute
                    exact
                    path="/GetPatient/:id"
                    component={GetPatient}
                  />
                )}
              {currentUser &&
                (currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <PrivateRoute
                    exact
                    path="/PatientSearch/:search"
                    component={PatientSearch}
                  />
                )}
              {currentUser && (
                <PrivateRoute
                  exact
                  path="/Procedures"
                  component={ProcedurePage}
                />
              )}
              {currentUser &&
                (currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <PrivateRoute
                    exact
                    path="/AddProcedure"
                    component={AddProcedure}
                  />
                )}
              {currentUser &&
                (currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <PrivateRoute
                    exact
                    path="/EditProcedure/:id"
                    component={EditProcedure}
                  />
                )}
              {currentUser && (
                <PrivateRoute
                  exact
                  path="/GetProcedure/:id"
                  component={GetProcedure}
                />
              )}
              {currentUser && (
                <PrivateRoute
                  exact
                  path="/ProcedureSearch/:search"
                  component={ProcedureSearch}
                />
              )}
              {currentUser && currentUser.role === "Admin" && (
                <PrivateRoute exact path="/Rooms" component={RoomPage} />
              )}
              {currentUser && currentUser.role === "Admin" && (
                <PrivateRoute exact path="/AddRoom" component={AddRoom} />
              )}
              {currentUser && currentUser.role === "Admin" && (
                <PrivateRoute exact path="/EditRoom/:id" component={EditRoom} />
              )}
              {currentUser && currentUser.role === "Admin" && (
                <PrivateRoute
                  exact
                  path="/RoomSearch/:search"
                  component={RoomSearch}
                />
              )}
              {currentUser && (
                <PrivateRoute exact path="/Visits" component={VisitPage} />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute exact path="/AddVisit" component={AddVisit} />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute exact path="/AddVisit/:id" component={AddVisit} />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute
                  exact
                  path="/EditVisit/:id"
                  component={EditVisit}
                />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute exact path="/GetVisit/:id" component={GetVisit} />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute
                  exact
                  path="/VisitSearch/:search"
                  component={VisitSearch}
                />
              )}
              {currentUser && currentUser.role === "Admin" && (
                <PrivateRoute
                  exact
                  path="/ChangeDoctor/:id"
                  component={ChangeDoctor}
                />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute
                  exact
                  path="/Histories/:id"
                  component={HistoryPage}
                />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute
                  exact
                  path="/HistorySearch/:search"
                  component={HistorySearch}
                />
              )}
              {currentUser && (
                <PrivateRoute exact path="/Verify" component={Verify} />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute exact path="/User" component={UserPage} />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute exact path="/EditUser" component={EditUser} />
              )}
              {currentUser && currentUser.role != "Unconfirmed" && (
                <PrivateRoute
                  exact
                  path="/ChangePassword"
                  component={ChangePassword}
                />
              )}
              <Route path="/ForgotPassword" component={ForgotPassword} />
              <Route path="/Register" component={Register} />
              <Route path="/login" component={LoginPage} />
              <Route path="/VerifyPassword" component={VerifyPassword} />
              {currentUser &&
                (currentUser.role === "Admin" ||
                  currentUser.role === "Employee") && (
                  <PrivateRoute
                    exact
                    path="/Schedule"
                    component={SchedulePage}
                  />
                )}
              <PrivateRoute exact path="/Payments" component={PaymentPage} />
            </div>
          </div>
        </div>
      </Router>
    );
  }
}

export { App };
