import { authHeader, handleResponse } from "@/Helpers";
import config from "config";

export const scheduleService = {
  getAllDays,
  getAllTimes,
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

function getAllDays() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/schedule`, requestOptions).then(
    handleResponse
  );
}

function getAllTimes(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/schedule/=${id}`, requestOptions).then(
    errorHandleResponse
  );
}
