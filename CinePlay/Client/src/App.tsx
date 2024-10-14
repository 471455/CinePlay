import React from "react";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import ListMedia from "./components/ListMedia";
import { CreateMedia, AddEpisode } from "./components/CreateMedia";
import AddReview from "./components/AddReview";
import Register from "./components/Register";
import Login from "./components/Login";
import ShowMedia from "./components/ShowMedia";
import Navbar from "./components/Navbar";
import Profile from "./components/Profile";
import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";

const App: React.FC = () => {
  return (
    <div className="App">
      <Navbar />
      <div className="content pt-content px-0 px-md-5 px-lg-5">
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Navigate to="/media" />} />
            <Route path="/media" element={<ListMedia />} />
            <Route path="/media/:id" element={<ShowMedia />} />
            <Route path="/create" element={<CreateMedia />} />
            <Route path="/add-episode/:id" element={<AddEpisode />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/profile" element={<Profile />} />
            <Route path="*" element={<Navigate to="/media" />} />
            <Route path="/add-review/:id" element={<AddReview />} />
          </Routes>
        </BrowserRouter>
      </div>
    </div>
  );
};

export default App;
