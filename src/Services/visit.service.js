import { authHeader, handleResponse } from "@/Helpers";
import config from "config";

export const visitService = {
  getAllVisits,
  getVisit,
  addVisit,
  editVisit,
  deleteVisit,
  visitSearch,
  confirmVisit,
  changeDoctor,
  getActiveVisits,
  visitSearchActive,
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

function getAllVisits() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/visit`, requestOptions).then(
    handleResponse
  );
}

function getActiveVisits() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/visit/~`, requestOptions).then(
    handleResponse
  );
}

function getVisit(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/visit/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function addVisit(Visit) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Visit),
  };
  return fetch(`${config.apiUrl}/api/visit`, requestOptions).then(
    errorHandleResponse
  );
}

function editVisit(Visit, id) {
  const requestOptions = {
    method: "PUT",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Visit),
  };
  return fetch(`${config.apiUrl}/api/visit/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function deleteVisit(id) {
  const requestOptions = { method: "DELETE", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/visit/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function visitSearch(search) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/visit/=${search}`, requestOptions).then(
    errorHandleResponse
  );
}

function visitSearchActive(search) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/visit/*${search}`, requestOptions).then(
    errorHandleResponse
  );
}

function confirmVisit(id) {
  const requestOptions = { method: "PATCH", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/visit/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function changeDoctor(Visit, id) {
  const requestOptions = {
    method: "PATCH",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Visit),
  };
  return fetch(`${config.apiUrl}/api/visit/+${id}`, requestOptions).then(
    errorHandleResponse
  );
}
