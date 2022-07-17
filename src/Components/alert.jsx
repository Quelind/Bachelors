import React, { Component } from "react";
class Alert extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }
  render() {
    return (
      <div
        className={`alert ${
          this.props.succeeded == "Success"
            ? "forSuccess"
            : this.props.succeeded == "Error"
            ? "forError"
            : null
        }`}
      >
        {this.props.message ? this.props.message : null}
      </div>
    );
  }
}

export default Alert;
