import { authenticationService, roomService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";
import { Link } from "react-router-dom";

class RoomPage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      rooms: null,
      errorMessage: "",
      search: null,
    };
    this.clickDelete = this.clickDelete.bind(this);
    this.handleChangeSearch = this.handleChangeSearch.bind(this);
    this.handleSubmitSearch = this.handleSubmitSearch.bind(this);
  }

  componentDidMount() {
    roomService.getAllRooms().then((rooms) => this.setState({ rooms }));
  }

  handleChangeSearch(event) {
    this.setState({ value: event.target.value });
  }

  handleSubmitSearch() {
    this.props.history.push(`/RoomSearch/${this.state.value}`);
  }

  clickDelete(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      roomService.deleteRoom(value).then(
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
    const { rooms } = this.state;
    return (
      <div>
        <Formik
          render={({ errors, status, touched, isSubmitting }) => (
            <div className="form-row flexbox-row">
              <div className="form-group col margin-from-top">
                <h1>Rooms</h1>
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
              <th>Name</th>
              <th>Type</th>
              <th>Capacity</th>
              <th style={{ width: "10%" }}></th>
            </tr>
          </thead>
          <tbody>
            {rooms &&
              rooms.map((room) => (
                <tr key={room.id}>
                  <td>{room.name}</td>
                  <td>{room.type}</td>
                  <td>{room.capacity}</td>
                  <td style={{ whiteSpace: "nowrap" }}>
                    <button className="btn btn-sm btn-warning btn-edit-room">
                      <Link
                        style={{ color: "white" }}
                        to={`/EditRoom/${room.id}`}
                      >
                        Edit
                      </Link>
                    </button>
                    <button
                      value={room.id}
                      onClick={this.clickDelete}
                      className="btn btn-sm btn-danger btn-delete-room"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
          </tbody>
        </table>
        <div className="form-row flexbox-row-list2">
          <button className="button-main add-button  flexbox-back-list">
            <Link style={{ color: "white" }} to={`/AddRoom`}>
              + Add
            </Link>
          </button>
        </div>
      </div>
    );
  }
}

export { RoomPage };
