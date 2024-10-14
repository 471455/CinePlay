import React, { useState } from "react";
import { register } from "../services/UserService";
import "bootstrap/dist/css/bootstrap.min.css";
import { useNavigate } from "react-router-dom";
import axios from "axios";

const RegisterForm: React.FC = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [passwordVisible, setPasswordVisible] = useState(false);
  const navigate = useNavigate();

  // Handle registration submission
  const handleRegisterSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    try {
      await register(email, password);
      console.log("User registered successfully.");
      navigate("/login");
    } catch (error: any) {
      if (axios.isAxiosError(error)) {
        console.error("Registration error:", {
          status: error.response?.status,
          data: error.response?.data,
          message: error.message,
        });
        if (error.response?.data?.Errors) {
          const firstErrorKey = Object.keys(error.response.data.Errors)[0];
          setError(error.response.data.Errors[firstErrorKey]);
        } else {
          setError("An unexpected error occurred.");
        }
      } else {
        console.error("Unexpected error:", error);
        setError("An unexpected error occurred.");
      }
    }
  };

  // Toggle password visibility
  const handlePasswordVisibilityToggle = () => {
    setPasswordVisible((prevState) => !prevState);
  };

  return (
    <form className="register-form" onSubmit={handleRegisterSubmit}>
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
            placeholder="Enter your password"
            required
          />
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={handlePasswordVisibilityToggle}
          >
            {passwordVisible ? "🙉 Hide" : "🙈 Show"}
          </button>
        </div>
        
        {error && (
          <div className="alert alert-danger mt-2" role="alert">
            {error}
          </div>
        )}

        <div className="form-text text-warning mt-2">
          Your password must be at least 6 characters long and include uppercase letters, lowercase letters, numbers, and special characters.
        </div>
      </div>

      <button type="submit" className="btn btn-success">
        Sign Up
      </button>
    </form>
  );
};

export default RegisterForm;
