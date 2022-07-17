import { authHeader, handleResponse } from "@/Helpers";
import config from "config";

export const timetableService = {
  getAllTimetables,
  getTimetable,
  getAllTimetablesUnlocked,
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

function getAllTimetables() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/timetable`, requestOptions).then(
    handleResponse
  );
}

function getTimetable(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/timetable/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function getAllTimetablesUnlocked(search) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(
    `${config.apiUrl}/api/timetable/=${search}`,
    requestOptions
  ).then(handleResponse);
}
