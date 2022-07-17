import { authenticationService, historyService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";

class HistorySearch extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      histories: null,
      errorMessage: "",
      search: null,
    };
    this.handleReturn = this.handleReturn.bind(this);
    this.handleReturnUser = this.handleReturnUser.bind(this);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let search = url.substring(url.lastIndexOf("=") + 1);
    let id = url.substring(url.lastIndexOf("/") + 1);
    let realId = id.charAt(0);
    historyService
      .historySearch(realId, search)
      .then((histories) => this.setState({ histories }));
  }

  handleReturn() {
    this.props.history.push(`/Patients`);
  }

  handleReturnUser(event) {
    this.props.history.push(`/User`);
  }

  render() {
    const { histories } = this.state;
    return (
      <div>
        <Formik
          render={({ status }) => (
            <div className="form-row flexbox-row">
              <div className="form-group col margin-from-top">
                <h1>Search Results</h1>
              </div>
              {status && <div className={"alert alert-danger"}>{status}</div>}
            </div>
          )}
        />
        {this.state.errorMessage && (
          <div className={"alert alert-danger"}>{this.state.errorMessage}</div>
        )}
        <table className="table table-striped">
          <thead>
            <tr>
              <th>Disease</th>
              <th>Description</th>
              <th>Date</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {histories &&
              histories.map((history) => (
                <tr key={history.id}>
                  <td>{history.name}</td>
                  <td>{history.description}</td>
                  <td>{history.date}</td>
                  <td style={{ whiteSpace: "nowrap" }}></td>
                </tr>
              ))}
          </tbody>
        </table>
        <div className="form-row">
          {this.state.currentUser && this.state.currentUser.role === "User" ? (
            <button
              className="btn btn-danger"
              type="button"
              id="button-addon2"
              onClick={this.handleReturnUser}
            >
              Back
            </button>
          ) : (
            <button
              className="btn btn-danger"
              type="button"
              id="button-addon2"
              onClick={this.handleReturn}
            >
              Back
            </button>
          )}
        </div>
      </div>
    );
  }
}

export { HistorySearch };
