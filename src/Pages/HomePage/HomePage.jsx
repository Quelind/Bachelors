import { authenticationService } from "@/Services";
import React from "react";
import { Link } from "react-router-dom";

class HomePage extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      currentUser: authenticationService.currentUserValue,
    };
  }

  render() {
    const { currentUser, doctors } = this.state;
    return (
      <div>
        <header id="header">
          <div className="homepage">
            <div className="overlay">
              <div className="container">
                <div className="row">
                  {currentUser.role === "User" ? (
                    <div className="homepage-text color-black">
                      <h1>Welcome!</h1>
                      <p>Check out our staff:</p>
                      <button className="button-welcome button-welcome-big">
                        <Link style={{ color: "white" }} to={`/Employees`}>
                          Employees
                        </Link>
                      </button>
                    </div>
                  ) : currentUser.role === "Unconfirmed" ? (
                    <div className="homepage-text color-black">
                      <h1>Welcome!</h1>
                      <p>Verify your account:</p>
                      <button className="button-welcome button-welcome-verify">
                        <Link style={{ color: "white" }} to={`/Verify`}>
                          Verify
                        </Link>
                      </button>
                    </div>
                  ) : currentUser.role === "Employee" ? (
                    <div className="homepage-text color-black">
                      <h1>Welcome!</h1>
                      <p>Check out your schedule:</p>
                      <button className="button-welcome button-welcome-verify">
                        <Link style={{ color: "white" }} to={`/Schedule`}>
                          Schedule
                        </Link>
                      </button>
                    </div>
                  ) : (
                    currentUser.role === "Admin" && (
                      <div className="homepage-text color-black">
                        <h1>Welcome!</h1>
                        <p>Check out the schedules:</p>
                        <button className="button-welcome button-welcome-verify">
                          <Link style={{ color: "white" }} to={`/Schedule`}>
                            Schedules
                          </Link>
                        </button>
                      </div>
                    )
                  )}
                </div>
              </div>
            </div>
          </div>
        </header>
        {currentUser.role == "Unconfirmed" && (
          <header id="header">
            <div className="homepage">
              <div className="overlay">
                <div className="container">
                  <div className="row">
                    <div className="homepage-text color-black">
                      <h1>Welcome!</h1>
                      <p>Verify your account:</p>
                      <button className="button-welcome button-welcome-verify">
                        <Link style={{ color: "white" }} to={`/Verify`}>
                          Verify
                        </Link>
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </header>
        )}
      </div>
    );
  }
}

export { HomePage };
