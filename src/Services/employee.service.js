import { authHeader, handleResponse } from "@/Helpers";
import config from "config";

export const employeeService = {
  getAllEmployees,
  getEmployee,
  addEmployee,
  editEmployee,
  deleteEmployee,
  employeeSearch,
  getRoomByEmployee,
  getEmployeesByTimetable,
  getEmployeeByUser,
};

function errorHandleResponse(response) {
  return response.json().then((data) => {
    if (!response.ok) {
      if ([401, 403].indexOf(response.status) !== -1) {
        authenticationService.logout();
        location.reload();
      }
      return Promise.reject(data);
    }
    return data;
  });
}

function getAllEmployees() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/employee`, requestOptions).then(
    handleResponse
  );
}

function getEmployee(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/employee/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function addEmployee(Employee) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Employee),
  };
  return fetch(`${config.apiUrl}/api/employee`, requestOptions).then(
    errorHandleResponse
  );
}

function editEmployee(Employee, id) {
  const requestOptions = {
    method: "PUT",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Employee),
  };
  return fetch(`${config.apiUrl}/api/employee/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function deleteEmployee(id) {
  const requestOptions = { method: "DELETE", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/employee/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function employeeSearch(search) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/employee/=${search}`, requestOptions).then(
    errorHandleResponse
  );
}

function getRoomByEmployee(name) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/employee/+${name}`, requestOptions).then(
    errorHandleResponse
  );
}

function getEmployeesByTimetable(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/employee/~${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function getEmployeeByUser(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/employee/@${id}`, requestOptions).then(
    errorHandleResponse
  );
}
