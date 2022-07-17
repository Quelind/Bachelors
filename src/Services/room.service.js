import { authHeader, handleResponse } from "@/Helpers";
import config from "config";

export const roomService = {
  getAllRooms,
  getRoom,
  addRoom,
  editRoom,
  deleteRoom,
  roomSearch,
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

function getAllRooms() {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/room`, requestOptions).then(
    handleResponse
  );
}

function getRoom(id) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/room/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function addRoom(Room) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Room),
  };
  return fetch(`${config.apiUrl}/api/room`, requestOptions).then(
    errorHandleResponse
  );
}

function editRoom(Room, id) {
  const requestOptions = {
    method: "PUT",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(Room),
  };
  return fetch(`${config.apiUrl}/api/room/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function deleteRoom(id) {
  const requestOptions = { method: "DELETE", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/room/${id}`, requestOptions).then(
    errorHandleResponse
  );
}

function roomSearch(search) {
  const requestOptions = { method: "GET", headers: authHeader() };
  return fetch(`${config.apiUrl}/api/room/=${search}`, requestOptions).then(
    errorHandleResponse
  );
}
