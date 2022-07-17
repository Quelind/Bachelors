import { authHeader, handleResponse } from "@/Helpers";
import config from "config";

export const patientService = {
  getAllPatients,
  getPatient,
  addPatient,
  editPatient,
  deletePatient,
  patientSearch,
  eraseDebt,
  requestPayment,
  getPatientByUser,
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

function getAllPatients() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/patient`, requestOptions).then(
    handleResponse
  );
}

function getPatient(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/patient/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function addPatient(Patient) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Patient),
  };
  return fetch(`${config.apiUrl}/api/patient`, requestOptions).then(
    errorHandleResponse
  );
}

function editPatient(Patient, id) {
  const requestOptions = {
    method: "PUT",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Patient),
  };
  return fetch(`${config.apiUrl}/api/patient/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function eraseDebt(id) {
  const requestOptions = { method: "PATCH", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/patient/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function deletePatient(id) {
  const requestOptions = { method: "DELETE", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/patient/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function patientSearch(search) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/patient/=${search}`, requestOptions).then(
    errorHandleResponse
  );
}

function requestPayment(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/patient/+${id}`, requestOptions).then(
    errorHandleResponse
  );
}
function getPatientByUser(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/patient/~${id}`, requestOptions).then(
    errorHandleResponse
  );
}
