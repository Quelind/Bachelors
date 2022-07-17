import { authenticationService, roomService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";
import { Link } from "react-router-dom";

class RoomSearch extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      rooms: null,
      errorMessage: "",
      search: null,
    };
    this.clickDelete = this.clickDelete.bind(this);
    this.handleReturn = this.handleReturn.bind(this);
  }

  componentDidMount() {
    let url = window.location.pathname;
    let search = url.substring(url.lastIndexOf("/") + 1);
    roomService.roomSearch(search).then((rooms) => this.setState({ rooms }));
  }

  handleReturn() {
    this.props.history.push(`/Rooms`);
  }

  clickDelete(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      roomService.deleteRoom(value).then(
        () => {
          this.props.history.push(`/Rooms`);
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
        <div className="form-row flexbox-row-list">
          <button
            className="btn btn-danger"
            type="button"
            id="button-addon2"
            onClick={this.handleReturn}
          >
            Back
          </button>
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

export { RoomSearch };
