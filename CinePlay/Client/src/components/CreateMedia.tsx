import React, { useState } from "react";
import {
  createMovie,
  createSeries,
  createEpisode,
} from "../services/MediaService";
import { useNavigate, useParams } from "react-router-dom";
import { Media } from "../Data";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";

export const CreateMedia: React.FC = () => {
  const [mediaType, setMediaType] = useState<"Movie" | "Series">("Movie");
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [releaseDate, setReleaseDate] = useState("");
  const [seasons, setSeasons] = useState(1);
  const [image, setImage] = useState<File | null>(null);
  const [video, setVideo] = useState<File | null>(null);
  const [isCreating, setIsCreating] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsCreating(true);

    const newMedia: Partial<Media> = {
      title,
      description,
      releaseDate: releaseDate ? new Date(releaseDate) : undefined,
      seasons,
    };

    const handleUploadProgress = (progressEvent: ProgressEvent) => {
      const progress = Math.round(
        (progressEvent.loaded / progressEvent.total) * 100
      );
      setUploadProgress(progress);
    };

    try {
      if (mediaType === "Movie") {
        const createdMovie = await createMovie(
          newMedia,
          image,
          video,
          handleUploadProgress
        );
        console.log("Movie created successfully:", createdMovie);
      } else {
        const createdSeries = await createSeries(
          newMedia,
          image,
          handleUploadProgress
        );
        console.log("Series created successfully:", createdSeries);
      }
      navigate("/media");
    } catch (error) {
      console.error("Error creating media:", error);
    } finally {
      setIsCreating(false);
      setUploadProgress(0);
    }
  };

  return (
    <form className="create-form" onSubmit={handleSubmit}>
      <div className="mb-3">
        <label htmlFor="mediaType" className="form-label">
          Media Type
        </label>
        <select
          className="form-select"
          id="mediaType"
          value={mediaType}
          onChange={(e) => setMediaType(e.target.value as "Movie" | "Series")}
        >
          <option value="Movie">Movie</option>
          <option value="Series">Series</option>
        </select>
      </div>
      <div className="mb-3">
        <label htmlFor="title" className="form-label">
          Title
        </label>
        <input
          className="form-control"
          type="text"
          id="title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
        />
      </div>
      <div className="mb-3">
        <label htmlFor="description" className="form-label">
          Description
        </label>
        <textarea
          className="form-control"
          id="description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="releaseDate" className="form-label">
          Release Date
        </label>
        <input
          className="form-control"
          type="date"
          id="releaseDate"
          value={releaseDate}
          onChange={(e) => setReleaseDate(e.target.value)}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="image" className="form-label">
          Poster
        </label>
        <input
          className="form-control"
          type="file"
          id="image"
          accept="image/*"
          onChange={(e) => setImage(e.target.files?.[0] || null)}
        />
      </div>
      {mediaType === "Movie" && (
        <div className="mb-3">
          <label htmlFor="video" className="form-label">
            Video
          </label>
          <input
            className="form-control"
            type="file"
            id="video"
            accept="video/*"
            onChange={(e) => setVideo(e.target.files?.[0] || null)}
          />
        </div>
      )}

      {mediaType === "Series" && (
        <div className="mb-3">
          <label htmlFor="seasons" className="form-label">
            Seasons
          </label>
          <input
            className="form-control"
            type="number"
            id="seasons"
            value={seasons}
            onChange={(e) => setSeasons(parseInt(e.target.value))}
          />
        </div>
      )}

      <button className="btn btn-success" type="submit" disabled={isCreating}>
        {isCreating ? "Creating..." : `Create ${mediaType}`}
      </button>

      {uploadProgress > 0 && (
        <div className="mt-3">
          <p>Upload progress: {uploadProgress}%</p>
          <progress value={uploadProgress} max="100" />
        </div>
      )}
    </form>
  );
};

const CreateEpisode: React.FC<{ seriesId: string }> = ({ seriesId: id }) => {
  const [seriesId] = useState(id);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [releaseDate, setReleaseDate] = useState("");
  const [seasonNumber, setSeason] = useState(1);
  const [episodeNumber, setEpisodeNumber] = useState(1);
  const [image, setImage] = useState<File | null>(null);
  const [video, setVideo] = useState<File | null>(null);
  const [isCreating, setIsCreating] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsCreating(true);

    const newEpisode: Partial<Media> = {
      id: seriesId,
      title,
      description,
      releaseDate: releaseDate ? new Date(releaseDate) : undefined,
      seasonNumber,
      episodeNumber,
    };

    const handleUploadProgress = (progressEvent: ProgressEvent) => {
      const progress = Math.round(
        (progressEvent.loaded / progressEvent.total) * 100
      );
      setUploadProgress(progress);
    };

    try {
      const createdEpisode = await createEpisode(
        newEpisode,
        image,
        video,
        handleUploadProgress
      );
      console.log("Episode added successfully:", createdEpisode);
      navigate("/media/" + seriesId);
    } catch (error) {
      console.error("Error creating media:", error);
    } finally {
      setIsCreating(false);
      setUploadProgress(0);
    }
  };

  return (
    <form className="create-form" onSubmit={handleSubmit}>
      <div className="mb-3">
        <label htmlFor="title" className="form-label">
          Title
        </label>
        <input
          className="form-control"
          type="text"
          id="title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="season" className="form-label">
          Season
        </label>
        <input
          className="form-control"
          type="number"
          id="season"
          value={seasonNumber}
          onChange={(e) => setSeason(parseInt(e.target.value))}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="episode-number" className="form-label">
          Episode Number
        </label>
        <input
          className="form-control"
          type="number"
          id="episode-number"
          value={episodeNumber}
          onChange={(e) => setEpisodeNumber(parseInt(e.target.value))}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="description" className="form-label">
          Description
        </label>
        <textarea
          className="form-control"
          id="description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="releaseDate" className="form-label">
          Release Date
        </label>
        <input
          className="form-control"
          type="date"
          id="releaseDate"
          value={releaseDate}
          onChange={(e) => setReleaseDate(e.target.value)}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="image" className="form-label">
          Poster
        </label>
        <input
          className="form-control"
          type="file"
          id="image"
          accept="image/*"
          onChange={(e) => setImage(e.target.files?.[0] || null)}
        />
      </div>
      <div className="mb-3">
        <label htmlFor="video" className="form-label">
          Video
        </label>
        <input
          className="form-control"
          type="file"
          id="video"
          accept="video/*"
          onChange={(e) => setVideo(e.target.files?.[0] || null)}
        />
      </div>

      <button className="btn btn-success" type="submit" disabled={isCreating}>
        {isCreating ? "Creating..." : "Create Episode"}
      </button>

      {uploadProgress > 0 && (
        <div className="mt-3">
          <p>Upload progress: {uploadProgress}%</p>
          <progress value={uploadProgress} max="100" />
        </div>
      )}
    </form>
  );
};

export const AddEpisode: React.FC = () => {
  let { id } = useParams();

  return <CreateEpisode seriesId={id!} />;
};

export default CreateMedia;
