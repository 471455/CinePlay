import React, { useState } from "react";
import { isAdmin, login } from "../services/UserService";
import "bootstrap/dist/css/bootstrap.min.css";
import { useNavigate } from "react-router-dom";

const LoginForm: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [passwordVisible, setPasswordVisible] = useState(false);
  const [rememberMe, setRememberMe] = useState(false);
  const navigate = useNavigate();

  // Handle login submission
  const handleLoginSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    try {
      await login(email, password, rememberMe);
      console.log("User logged in successfully.");
      if (isAdmin()) {
        console.log("User has admin privileges.");
      }
      navigate("/");
    } catch (error: any) {
      const errorMessage =
        error?.response?.data?.message || "An unexpected error occurred.";
      setError(errorMessage);
      console.error("Login failed:", error);
    }
  };

  // Toggle password visibility
  const handlePasswordVisibilityToggle = () => {
    setPasswordVisible((prevState) => !prevState);
  };

  // Toggle remember me checkbox
  const handleRememberMeToggle = () => {
    setRememberMe((prevState) => !prevState);
  };

  return (
    <form className="login-form" onSubmit={handleLoginSubmit}>
      <div className="mb-3">
        <label htmlFor="email" className="form-label">
          Email
        </label>
        <input
          className="form-control"
          type="email"
          id="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="name@example.com"
          required
        />
      </div>

      <div className="mb-3">
        <label htmlFor="password" className="form-label">
          Password
        </label>
        <div className="input-group">
          <input
            className="form-control"
            type={passwordVisible ? "text" : "password"}
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={handlePasswordVisibilityToggle}
          >
            {passwordVisible ? "🔒 Hide" : "🔓 Show"}
          </button>         
        </div>
        
        {error && (
          <div className="alert alert-danger mt-2" role="alert">
            {error}
          </div>
        )}
      </div>

      <div className="d-flex justify-content-between align-items-center">
        <button type="submit" className="btn btn-success">
          Log In
        </button>
        <a href="/register">Don't have an account? Sign up.</a>
      </div>

      <div className="form-check mt-3">
        <input
          className="form-check-input"
          type="checkbox"
          id="rememberMe"
          checked={rememberMe}
          onChange={handleRememberMeToggle}
        />
        <label className="form-check-label" htmlFor="rememberMe">
          Remember Me
        </label>
      </div>
    </form>
  );
};

export default LoginForm;
