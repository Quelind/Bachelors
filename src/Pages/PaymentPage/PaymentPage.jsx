import { authenticationService, patientService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";
import { PayPalScriptProvider, PayPalButtons } from "@paypal/react-paypal-js";

class PaymentPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      patients: null,
      errorMessage: "",
      search: null,
    };
    this.clickPayment = this.clickPayment.bind(this);
  }

  componentDidMount() {
    {
      this.state.currentUser &&
        this.state.currentUser.role === "Admin" &&
        patientService
          .getAllPatients()
          .then((patients) => this.setState({ patients }));
    }
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

  clickPayment(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      patientService.requestPayment(value).then(
        () => {
          location.reload(true);
        },
        (error) => {
          this.setState({ errorMessage: error });
        }
      );
    }
  }

  render() {
    const { currentUser, currentPatient, patients } = this.state;
    return (
      <div>
        {currentUser.role == "User" && (
          <header id="header">
            <div className="homepage">
              <div className="overlay">
                <div className="container">
                  <div className="row">
                    {currentPatient &&
                      (currentPatient.debt != 0 ? (
                        <div className="homepage-text color-black">
                          <h1>Tab: {currentPatient.debt}€</h1>
                          <p>Pay now:</p>
                          <PayPalScriptProvider
                            options={{
                              "client-id":
                                "AXRHl16CbaM7x6hd79BXa_kwfOZEoxMpiq7fWPQw7ZWaMSh32_Ed1eVCo1HDbTptXi-hA51oWzkoN4Fp",
                              currency: "EUR",
                            }}
                          >
                            <PayPalButtons
                              createOrder={(data, actions) => {
                                return actions.order.create({
                                  purchase_units: [
                                    {
                                      amount: {
                                        value: currentPatient.debt,
                                      },
                                    },
                                  ],
                                });
                              }}
                              onApprove={(data, actions) => {
                                patientService.eraseDebt(
                                  parseInt(currentPatient.id)
                                );
                                return actions.order
                                  .capture()
                                  .then((details) => {
                                    const name = details.payer.name.given_name;
                                    alert(`Transaction completed`);
                                    location.reload(true);
                                  });
                              }}
                            />
                          </PayPalScriptProvider>
                        </div>
                      ) : (
                        <div className="homepage-text color-black">
                          <h1>Tab: {currentPatient.debt}€</h1>
                          <p>All good! You don't have to pay anything.</p>
                        </div>
                      ))}
                  </div>
                </div>
              </div>
            </div>
          </header>
        )}
        {currentUser.role != "User" && (
          <div>
            <Formik
              render={({ status }) => (
                <div className="form-row flexbox-row">
                  <div className="form-group col margin-from-top">
                    <h1>Tabs</h1>
                  </div>
                  {status && (
                    <div className={"alert alert-danger"}>{status}</div>
                  )}
                </div>
              )}
            />
            {this.state.errorMessage && (
              <div className={"alert alert-danger"}>
                {this.state.errorMessage}
              </div>
            )}
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Surname</th>
                  <th>Tab</th>
                  <th style={{ width: "10%" }}></th>
                </tr>
              </thead>
              <tbody>
                {patients &&
                  patients
                    .filter((patient) => patient.debt != 0)
                    .map((patient) => (
                      <tr key={patient.id}>
                        <td>{patient.name}</td>
                        <td>{patient.surname}</td>
                        <td>{patient.debt}€</td>
                        <td style={{ whiteSpace: "nowrap" }}>
                          <button
                            value={patient.id}
                            onClick={this.clickPayment}
                            className="btn btn-sm btn-primary"
                          >
                            Request Payment
                          </button>
                        </td>
                      </tr>
                    ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    );
  }
}

export { PaymentPage };
