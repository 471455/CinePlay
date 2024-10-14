import React, { useState, useEffect } from "react";
import { API_URL, userProfile } from "../Data";
import {
  fetchProfile,
  updateProfile,
  getExtension,
} from "../services/UserService";
import "bootstrap/dist/css/bootstrap.min.css";

const Profile: React.FC = () => {
  const [profile, setProfile] = useState<userProfile | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const [newUsername, setNewUsername] = useState("");
  const [newProfilePic, setNewProfilePic] = useState<File | null>(null);

  useEffect(() => {
    const getProfile = async () => {
      const data = await fetchProfile();
      setProfile(data);
      setNewUsername(data.userName);
    };

    getProfile();
  }, []);

  const handleEdit = () => {
    setIsEditing(true);
  };

  const handleCancel = () => {
    setIsEditing(false);
    if (profile) {
      setNewUsername(profile.userName);
      setNewProfilePic(null);
    }
  };

  const handleSave = async () => {
    if (profile) {
      const formData = new FormData();
      formData.append("UserId", profile.userId);
      formData.append("UserName", newUsername);

      if (newProfilePic) {
        formData.append("ProfilePic", newProfilePic);
        formData.append(
          "ProfilePicPath",
          profile.userId + getExtension(newProfilePic?.name || "")
        );
      }

      const updatedProfile = await updateProfile(formData);
      setProfile(updatedProfile);
      setIsEditing(false);
    }
  };

  const handleProfilePicChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    if (event.target.files && event.target.files[0]) {
      setNewProfilePic(event.target.files[0]);
    }
  };

  if (!profile) {
    return <div>Loading...</div>;
  }

  return (
    <div className="container mt-5">
      <div className="card bg-dark text-white">
        <div className="card-header">
          <h2>User Profile</h2>
        </div>
        <div className="card-body text-center">
          <img
            src={
              profile.profilePicPath
                ? API_URL + profile.profilePicPath
                : "profilePic.jpg"
            }
            alt="Profil"
            className="rounded-circle mb-3"
            style={{ width: "150px", height: "150px", objectFit: "cover" }}
          />
          {isEditing && (
            <div className="mb-3">
              <label htmlFor="profilePic" className="form-label">
                Profil Resmini Değiştir
              </label>
              <input
                type="file"
                id="profilePic"
                className="form-control"
                onChange={handleProfilePicChange}
              />
            </div>
          )}
          <input
            type="text"
            id="disabledTextInput"
            className={`form-control ${
              isEditing ? "bg-light text-dark" : "bg-dark text-white"
            } mb-3`}
            value={newUsername}
            onChange={(e) => setNewUsername(e.target.value)}
            disabled={!isEditing}
          />
          {isEditing ? (
            <div className="d-flex justify-content-around">
              <button className="btn btn-secondary" onClick={handleCancel}>
              Cancel
              </button>
              <button className="btn btn-primary" onClick={handleSave}>
              Save
              </button>
            </div>
          ) : (
            <button className="btn btn-primary" onClick={handleEdit}>
              Edit Profile
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default Profile;
