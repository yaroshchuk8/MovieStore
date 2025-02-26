import {MovieSmallInDto} from "./movie.ts";

// api -> client
export interface GenreInDto {
  id: string;
  name: string;
  movies: MovieSmallInDto[];
}