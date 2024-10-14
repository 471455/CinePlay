import { Media, axiosInstance } from "../Data";

const MEDIA_URL = "media/";

export const fetchMedia = async (): Promise<Media[]> => {
  try {
    const response = await axiosInstance.get<{ $values: Media[] }>(
      MEDIA_URL + "movies_and_series"
    );
    return response.data.$values;
  } catch (error) {
    console.error("Error fetching media:", error);
    throw error;
  }
};

export const fetchMovies = async (): Promise<Media[]> => {
  try {
    const response = await axiosInstance.get<{ $values: Media[] }>(
      MEDIA_URL + "movies"
    );
    return response.data.$values;
  } catch (error) {
    console.error("Error fetching movies:", error);
    throw error;
  }
};

export const fetchSeries = async (): Promise<Media[]> => {
  try {
    const response = await axiosInstance.get<{ $values: Media[] }>(
      MEDIA_URL + "series"
    );
    return response.data.$values;
  } catch (error) {
    console.error("Error fetching series:", error);
    throw error;
  }
};

export const fetchMediaById = async (id: string): Promise<Media> => {
  const response = await axiosInstance.get(MEDIA_URL + id);
  return response.data;
};

export const createMovie = async (
  newMovie: Partial<Media>,
  poster: File | null,
  video: File | null,
  onUploadProgress: (progressEvent: any) => void
): Promise<Media> => {
  const formData = new FormData();
  formData.append("title", newMovie.title || "");
  formData.append("description", newMovie.description || "");
  formData.append("releaseDate", newMovie.releaseDate?.toISOString() || "");

  if (poster) {
    const uniqueImageName = await generateUniqueFileName(
      newMovie.title || "N/A" + Date.now().toString(),
      poster
    );
    formData.append("poster", poster, uniqueImageName);
    formData.append("posterPath", uniqueImageName);
  }

  if (video) {
    const uniqueVideoName = await generateUniqueFileName(
      newMovie.title || "N/A",
      video
    );
    formData.append("video", video, uniqueVideoName);
    formData.append("videoPath", uniqueVideoName);
  }

  try {
    const response = await axiosInstance.post<Media>(
      MEDIA_URL + "add_movie",
      formData,
      {
        headers: { "Content-Type": "multipart/form-data" },
        onUploadProgress,
      }
    );

    return response.data;
  } catch (error) {
    console.error("Error creating movie:", error);
    throw error;
  }
};

export const createSeries = async (
  newSeries: Partial<Media>,
  poster: File | null,
  onUploadProgress: (progressEvent: any) => void
): Promise<Media> => {
  const formData = new FormData();
  formData.append("title", newSeries.title || "");
  formData.append("description", newSeries.description || "");
  formData.append("releaseDate", newSeries.releaseDate?.toISOString() || "");
  formData.append("seasons", newSeries.seasons?.toString() || "1");

  if (poster) {
    const uniqueImageName = await generateUniqueFileName(
      newSeries.title || "N/A" + Date.now().toString(),
      poster
    );
    formData.append("poster", poster, uniqueImageName);
    formData.append("posterPath", uniqueImageName);
  }

  try {
    const response = await axiosInstance.post<Media>(
      MEDIA_URL + "add_series",
      formData,
      {
        headers: { "Content-Type": "multipart/form-data" },
        onUploadProgress,
      }
    );

    return response.data;
  } catch (error) {
    console.error("Error creating series:", error);
    throw error;
  }
};

export const createEpisode = async (
  newEpisode: Partial<Media>,
  poster: File | null,
  video: File | null,
  onUploadProgress: (progressEvent: any) => void
): Promise<Media> => {
  const formData = new FormData();
  formData.append("seriesId", newEpisode.id || "");
  formData.append("title", newEpisode.title || "");
  formData.append("description", newEpisode.description || "");
  formData.append("releaseDate", newEpisode.releaseDate?.toISOString() || "");
  formData.append("seasonNumber", newEpisode.seasonNumber?.toString() || "");
  formData.append("episodeNumber", newEpisode.episodeNumber?.toString() || "");

  if (poster) {
    const imageName = `${newEpisode.seasonNumber?.toString() || ""}-${
      newEpisode.episodeNumber?.toString() || ""
    }.${getFileExtension(poster.name)}`;
    formData.append("poster", poster, imageName);
    formData.append("posterPath", imageName);
  }

  if (video) {
    const videoName = `S${newEpisode.seasonNumber?.toString() || ""}-E${
      newEpisode.episodeNumber?.toString() || ""
    }.${getFileExtension(video.name)}`;
    formData.append("video", video, videoName);
    formData.append("videoPath", videoName);
  }

  try {
    const response = await axiosInstance.post<Media>(
      MEDIA_URL + "add_episode",
      formData,
      {
        headers: { "Content-Type": "multipart/form-data" },
        onUploadProgress,
      }
    );

    return response.data;
  } catch (error) {
    console.error("Error creating series:", error);
    throw error;
  }
};

export const deleteMedia = async (id: string): Promise<void> => {
  try {
    await axiosInstance.delete(MEDIA_URL + "delete_media/" + id);
  } catch (error) {
    console.error("Error deleting media:", error);
    throw error;
  }
};

const generateUniqueFileName = async (
  title: string,
  file: File
): Promise<string> => {
  let extension = getFileExtension(file.name);
  if (extension == "mkv") {
    extension = "mp4";
  }
  //const baseName = file.name.replace(`.${extension}`, "");
  //const timestamp = Date.now();
  const uniqueFileName = `${title}.${extension}`;
  return uniqueFileName;
};

const getFileExtension = (fileName: string): string => {
  let extension = fileName.split(".").pop() || "";
  return extension;
};
