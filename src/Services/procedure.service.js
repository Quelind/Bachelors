import { authHeader, handleResponse } from "@/Helpers";
import config from "config";

export const procedureService = {
  getAllProcedures,
  getProcedure,
  addProcedure,
  editProcedure,
  deleteProcedure,
  procedureSearch,
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

function getAllProcedures() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/procedure`, requestOptions).then(
    handleResponse
  );
}

function getProcedure(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/procedure/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function addProcedure(Procedure) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Procedure),
  };
  return fetch(`${config.apiUrl}/api/procedure`, requestOptions).then(
    errorHandleResponse
  );
}

function editProcedure(Procedure, id) {
  const requestOptions = {
    method: "PUT",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Procedure),
  };
  return fetch(`${config.apiUrl}/api/procedure/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function deleteProcedure(id) {
  const requestOptions = { method: "DELETE", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/procedure/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function procedureSearch(search) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(
    `${config.apiUrl}/api/procedure/=${search}`,
    requestOptions
  ).then(errorHandleResponse);
}
