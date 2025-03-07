import {MovieSummaryDto} from "./movie.ts";

// api -> client
export interface GenreDto {
  id: string;
  name: string;
  movies: MovieSummaryDto[];
}

// client -> api
export interface GenreUpsertDto {
  id: string;
  name: string;
  movieIds: string[];
}