import axios, { AxiosInstance } from "axios";
import { ReactNode } from "react";

export const API_URL = "https://localhost:7260/api";

export let axiosInstance: AxiosInstance = axios.create({
  baseURL: API_URL,
});


export const setAxiosInterceptors = () => {
  axiosInstance.interceptors.request.use(
    (config) => {
      const creditentialsString =
        localStorage.getItem("creditentials") ||
        sessionStorage.getItem("creditentials");
      if (creditentialsString) {
        const creditentials: Creditentials = JSON.parse(creditentialsString);
        config.headers.Authorization = `Bearer ${creditentials.token}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );
};

setAxiosInterceptors();

export interface Media {
  id: string;
  createDate: Date;
  modifiedDate: Date;
  title: string;
  description: string;
  mediaType: "Movie" | "Series" | "Episode";
  releaseDate?: Date;
  posterPath: string;
  videoPath: string;
  episodeNumber: number;
  seasonNumber: number;
  seasons: number;
  series: Media;
  episodes: { $values: Media[] };
  reviews: { $values: Review[] };
}

export interface Review {
  title: ReactNode;
  id: string;
  userProfile: userProfile;
  userId: string;
  mediaId: string;
  createDate: Date;
  modifiedDate: Date;
  rating: number;
  comment: string;
}

export interface Creditentials {
  nameid: string;
  email: string;
  role: string;
  exp: number;
  token: string;
}

export interface userProfile {
  userId: string;
  userName: string;
  profilePicPath: string;
}
