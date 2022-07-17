import { authHeader } from "@/Helpers";
import config from "config";

export const historyService = {
  getAllHistories,
  historySearch,
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

function getAllHistories(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/history/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function historySearch(id, search) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(
    `${config.apiUrl}/api/history/=${id}=${search}`,
    requestOptions
  ).then(errorHandleResponse);
}
