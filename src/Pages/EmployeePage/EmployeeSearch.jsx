import { authenticationService, employeeService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";
import Card from "react-bootstrap/Card";
import Col from "react-bootstrap/Col";
import ListGroup from "react-bootstrap/ListGroup";
import ListGroupItem from "react-bootstrap/ListGroupItem";
import Row from "react-bootstrap/Row";
import { Link } from "react-router-dom";

class EmployeeSearch extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      employees: null,
      errorMessage: "",
      search: null,
    };
    this.clickDelete = this.clickDelete.bind(this);
    this.handleReturn = this.handleReturn.bind(this);
  }

  handleReturn() {
    this.props.history.push(`/Employees`);
  }
  componentDidMount() {
    let url = window.location.pathname;
    let search = url.substring(url.lastIndexOf("/") + 1);
    employeeService
      .employeeSearch(search)
      .then((employees) => this.setState({ employees }));
  }

  clickDelete(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      employeeService.deleteEmployee(value).then(
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
    const { currentUser, employees } = this.state;
    return (
      <div>
        <Formik
          render={({}) => (
            <div className="form-row flexbox-row">
              <div className="form-group col margin-from-top">
                <h1>Search Results</h1>
              </div>
            </div>
          )}
        />
        <Row xs={1} md={2} className="g-4">
          {employees &&
            employees.map((employee) => (
              <Col>
                <Card style={{ width: "21rem", marginTop: "20px" }}>
                  <Card.Img variant="top" src={employee.image} />
                  <Card.Body>
                    <Card.Title>
                      {employee.name} {employee.surname}
                    </Card.Title>
                    <Card.Text>{employee.specialization}</Card.Text>
                  </Card.Body>
                  <ListGroup className="list-group-flush">
                    <ListGroupItem>Room: {employee.fk_room}</ListGroupItem>
                    <ListGroupItem>Phone: {employee.phone}</ListGroupItem>
                    <ListGroupItem>Email: {employee.email}</ListGroupItem>
                  </ListGroup>
                  <Card.Body className="card-style">
                    <button className="button-main get-button">
                      <Link
                        style={{ color: "white" }}
                        to={`/GetEmployee/${employee.id}`}
                      >
                        More
                      </Link>
                    </button>
                    {currentUser.role != "Employee" &&
                      currentUser.role != "Unconfirmed" && (
                        <button className="button-main register-button">
                          <Link
                            style={{ color: "white" }}
                            to={`/AddVisit/${employee.id}`}
                          >
                            Register
                          </Link>
                        </button>
                      )}
                    {(currentUser.role === "Admin" ||
                      currentUser.role === "Employee") && (
                      <button className="button-main edit-button">
                        <Link
                          style={{ color: "white" }}
                          to={`/EditEmployee/${employee.id}`}
                        >
                          Edit
                        </Link>
                      </button>
                    )}
                    {(currentUser.role === "Admin" ||
                      currentUser.role === "Employee") && (
                      <button
                        value={employee.id}
                        onClick={this.clickDelete}
                        className="button-main delete-button"
                      >
                        Delete
                      </button>
                    )}
                  </Card.Body>
                </Card>
              </Col>
            ))}
        </Row>

        <div className="form-row flexbox-row-card">
          <button
            className="btn btn-danger"
            type="button"
            id="button-addon2"
            onClick={this.handleReturn}
          >
            Back
          </button>
          {(currentUser.role === "Admin" ||
            currentUser.role === "Employee") && (
            <button className="button-main add-button">
              <Link style={{ color: "white" }} to={`/AddPatient`}>
                + Add
              </Link>
            </button>
          )}
        </div>
      </div>
    );
  }
}

export { EmployeeSearch };
