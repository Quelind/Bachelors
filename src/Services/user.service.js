import { authHeader } from "@/Helpers";
import config from "config";

export const userService = {
  register,
  verifyEmail,
  getUser,
  editUser,
  changePassword,
  forgotPassword,
  verifyPassword,
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

function register(User) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(User),
  };
  return fetch(`${config.apiUrl}/api/user/register`, requestOptions).then(
    errorHandleResponse
  );
}

function verifyEmail(token) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(token),
  };
  return fetch(`${config.apiUrl}/api/user/verify`, requestOptions).then(
    errorHandleResponse
  );
}

function getUser(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/user/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function editUser(User, id) {
  const requestOptions = {
    method: "PATCH",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(User),
  };
  return fetch(`${config.apiUrl}/api/user/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function changePassword(User, id) {
  const requestOptions = {
    method: "PATCH",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(User),
  };
  return fetch(
    `${config.apiUrl}/api/user/update-password/${id}`,
    requestOptions
  ).then(errorHandleResponse);
}

function forgotPassword(User) {
  const requestOptions = {
    method: "PATCH",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(User),
  };
  return fetch(
    `${config.apiUrl}/api/user/forgot-password`,
    requestOptions
  ).then(errorHandleResponse);
}

function verifyPassword(User) {
  const requestOptions = {
    method: "PATCH",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(User),
  };
  return fetch(
    `${config.apiUrl}/api/user/verify-password`,
    requestOptions
  ).then(errorHandleResponse);
}
