import { authenticationService, historyService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";

class HistoryPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      histories: null,
      errorMessage: "",
      search: null,
    };
    this.handleChangeSearch = this.handleChangeSearch.bind(this);
    this.handleSubmitSearch = this.handleSubmitSearch.bind(this);
    this.handleReturn = this.handleReturn.bind(this);
    this.handleReturnUser = this.handleReturnUser.bind(this);
  }
  handleReturn() {
    this.props.history.push(`/Patients`);
  }

  handleReturnUser() {
    this.props.history.push(`/User`);
  }
  componentDidMount() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    historyService
      .getAllHistories(id)
      .then((histories) => this.setState({ histories }));
  }

  handleChangeSearch(event) {
    this.setState({ value: event.target.value });
  }

  handleSubmitSearch() {
    let url = window.location.pathname;
    let id = url.substring(url.lastIndexOf("/") + 1);
    this.props.history.push(`/HistorySearch/${id}=${this.state.value}`);
  }

  render() {
    const { histories } = this.state;
    {
      histories && histories.length && console.log(histories);
    }
    return (
      <div>
        <Formik
          render={({ errors, status, touched }) => (
            <div className="form-row flexbox-row">
              <div className="form-group col margin-from-top">
                {histories && histories.length != 0 && (
                  <h1>
                    History of {histories[0].patient_name}{" "}
                    {histories[0].patient_surname}{" "}
                  </h1>
                )}
              </div>
              {status && <div className={"alert alert-danger"}>{status}</div>}
              <div className="form-group col input-group flexbox-search-list margin-from-top">
                <input
                  type="text"
                  className={
                    "form-control search-style-list" +
                    (errors.name && touched.name ? " is-invalid" : "")
                  }
                  placeholder="Search"
                  onChange={this.handleChangeSearch}
                />
                <button
                  className="btn btn-primary button-search-list"
                  type="button"
                  id="button-addon2"
                  onClick={this.handleSubmitSearch}
                >
                  Search
                </button>
              </div>
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

export { HistoryPage };
