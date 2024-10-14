import { useEffect, useState, useCallback } from "react";
import { isAdmin, isAuthenticated, logout } from "../services/UserService";
import "bootstrap/dist/css/bootstrap.min.css";
import React from "react";

function Navbar() {
  const [isLogged, setIsLogged] = useState(isAuthenticated());
  const [isAdminUser, setIsAdminUser] = useState(isAdmin());

  const checkAuth = useCallback(() => {
    setIsLogged(isAuthenticated());
    setIsAdminUser(isAdmin());
  }, []);

  useEffect(() => {
    checkAuth();

    window.addEventListener("storage", checkAuth);
    window.addEventListener("authChange", checkAuth);

    return () => {
      window.removeEventListener("storage", checkAuth);
      window.removeEventListener("authChange", checkAuth);
    };
  }, [checkAuth]);

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark fixed-top d-flex p-3 justify-content-between align-items-center shadow-sm">
      <a className="navbar-brand text-light font-weight-bold" href="/">
      CinePlay Watch
      </a>
      <div className="btn-group">
        {isAdminUser && (
          <a className="btn btn-outline-success" href="/create">
          Add Media 
          </a>
        )}
        {isLogged ? (
          <>
            <a className="btn btn-outline-primary" href="/profile">
            Profile
            </a>
            <button className="btn btn-outline-danger" onClick={logout}>
            Sign Out
            </button>
          </>
        ) : (
          <a className="btn btn-primary text-white" href="/login">
            Login
          </a>
        )}
      </div>
    </nav>
  );
}

export default Navbar;
