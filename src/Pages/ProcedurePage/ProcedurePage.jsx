import { authenticationService, procedureService } from "@/Services";
import { Formik } from "formik";
import React, { Component } from "react";
import Card from "react-bootstrap/Card";
import Col from "react-bootstrap/Col";
import ListGroup from "react-bootstrap/ListGroup";
import ListGroupItem from "react-bootstrap/ListGroupItem";
import Row from "react-bootstrap/Row";
import { Link } from "react-router-dom";

class ProcedurePage extends Component {
  constructor(props) {
    super(props);
    this.state = {
      currentUser: authenticationService.currentUserValue,
      procedures: null,
      errorMessage: "",
      search: null,
    };
    this.clickDelete = this.clickDelete.bind(this);
    this.handleChangeSearch = this.handleChangeSearch.bind(this);
    this.handleSubmitSearch = this.handleSubmitSearch.bind(this);
  }

  componentDidMount() {
    procedureService
      .getAllProcedures()
      .then((procedures) => this.setState({ procedures }));
  }

  handleChangeSearch(event) {
    this.setState({ value: event.target.value });
  }

  handleSubmitSearch() {
    this.props.history.push(`/ProcedureSearch/${this.state.value}`);
  }

  clickDelete(event) {
    let result = window.confirm("Are you sure?");
    let { value } = event.target;
    if (result) {
      procedureService.deleteProcedure(value).then(
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
    const { currentUser, procedures } = this.state;
    return (
      <div>
        <Formik
          render={({ errors, status, touched }) => (
            <div className="form-row flexbox-row">
              <div className="form-group col margin-from-top">
                <h1>Procedures</h1>
              </div>
              {status && <div className={"alert alert-danger"}>{status}</div>}
              <div className="form-group col input-group flexbox-search-card margin-from-top">
                <input
                  type="text"
                  className={
                    "form-control search-style-card" +
                    (errors.name && touched.name ? " is-invalid" : "")
                  }
                  placeholder="Search"
                  onChange={this.handleChangeSearch}
                />
                <button
                  className="btn btn-primary button-search-card"
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
        <Row xs={1} md={2} className="g-4">
          {procedures &&
            procedures.map((procedure) => (
              <Col>
                <Card style={{ width: "21rem", marginTop: "20px" }}>
                  <Card.Img variant="top" src={procedure.image} />
                  <Card.Body>
                    <Card.Title>{procedure.name}</Card.Title>
                    <Card.Text>{procedure.information}</Card.Text>
                  </Card.Body>
                  <ListGroup className="list-group-flush">
                    <ListGroupItem>Price: {procedure.price}€</ListGroupItem>
                    <ListGroupItem>
                      Doctor: {procedure.requirement}
                    </ListGroupItem>
                    <ListGroupItem>
                      Duration: {procedure.duration} minutes
                    </ListGroupItem>
                  </ListGroup>
                  <Card.Body className="card-style">
                    <button className="button-main get-button">
                      <Link
                        style={{ color: "white" }}
                        to={`/GetProcedure/${procedure.id}`}
                      >
                        More Information
                      </Link>
                    </button>
                    {(currentUser.role === "Admin" ||
                      currentUser.role === "Employee") && (
                      <button className="button-main edit-button">
                        <Link
                          style={{ color: "white" }}
                          to={`/EditProcedure/${procedure.id}`}
                        >
                          Edit
                        </Link>
                      </button>
                    )}
                    {(currentUser.role === "Admin" ||
                      currentUser.role === "Employee") && (
                      <button
                        value={procedure.id}
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

        <div className="form-row flexbox-row-card2">
          {(currentUser.role === "Admin" ||
            currentUser.role === "Employee") && (
            <button className="button-main add-button flexbox-back-card ">
              <Link style={{ color: "white" }} to={`/AddProcedure`}>
                + Add
              </Link>
            </button>
          )}
        </div>
      </div>
    );
  }
}

export { ProcedurePage };
