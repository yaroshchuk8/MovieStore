import {Autocomplete, Box, Button, Paper, TextField, Typography,} from "@mui/material";
import {GenreDto, GenreUpsertDto} from "../../types/genre.ts";
import {Controller, useForm} from "react-hook-form";
import {MovieSummaryDto} from "../../types/movie.ts";
import axios from "axios";
import {useLocation, useNavigate, useParams} from "react-router";
import {useEffect, useState} from "react";
import { v4 } from 'uuid';

function GenreForm() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams();
  const [selectedGenre, setSelectedGenre] = useState<GenreDto>(location.state?.genre);
  const [movies, setMovies] = useState<MovieSummaryDto[]>([]);
  const { handleSubmit, control, register, formState: { errors }, reset } = useForm<GenreDto>({
    defaultValues: selectedGenre ? selectedGenre : {
      id: v4(), // random guid to create new genre, will be replaced in edit mode
      name: '',
      movies: []
    }}
  );

  useEffect(() => {
    // fetch genre if navigated directly via link
    if (!selectedGenre && id) {
      axios.get<GenreDto>(`http://localhost:5000/api/genres/${id}`)
        .then(response => setSelectedGenre(response.data))
        .catch(error => console.error("Failed to fetch genre:", error));
    }
    // fetch movies
    axios.get<MovieSummaryDto[]>('http://localhost:5000/api/movies/summary')
      .then(response => setMovies(response.data))
      .catch(error => console.error("Failed to fetch movies:", error));
  }, []);

  useEffect(() => {
    // update form
    if (selectedGenre) {
      reset(selectedGenre);
    }
  }, [selectedGenre]);

  const onSubmit = (data: GenreDto) => {
    const genre: GenreUpsertDto = {
      id: data.id,
      name: data.name,
      movieIds: data.movies.map((movie: MovieSummaryDto) => movie.id)
    }

    if (selectedGenre) {
      // update
      axios.put<GenreUpsertDto>("http://localhost:5000/api/genres", genre)
        .then(() => navigate("/genres"));
    } else {
      // create
      axios.post<GenreUpsertDto>("http://localhost:5000/api/genres", genre)
        .then(() => navigate("/genres"));
    }
  };

  if (!selectedGenre && id) {
    return <Typography>Loading...</Typography>;
  }

  return (
    <Paper sx={{ borderRadius: 3, padding: 3, marginTop: '2rem'}}>
      <Box component='form' onSubmit={handleSubmit(onSubmit)} display='flex' flexDirection='column' gap={3}>
        <Typography variant="h5" gutterBottom color="primary">
          {selectedGenre ? "Edit genre" : "Create genre"}
        </Typography>

        <TextField
          label="Id"
          {...register("id")}
          error={!!errors.id}
          helperText={errors.id?.message}
          disabled
        />

        <TextField
          label="Name"
          {...register("name", { required: "Genre name is required" })}
          error={!!errors.name}
          helperText={errors.name?.message}
        />

        <Controller
          name="movies"
          control={control}
          render={({ field }) => (
            <Autocomplete
              multiple
              options={movies}
              getOptionLabel={(option) => option.title}
              disableCloseOnSelect
              value={movies.filter((movie) =>
                field.value?.some((selected) => selected.id === movie.id)
              )}
              onChange={(_, newValue) => field.onChange(newValue)}
              renderInput={(params) => (
                <TextField {...params} label="Movies" variant="outlined" placeholder="Select movies" />
              )}
            />
          )}
        />

        <Box display='flex' justifyContent='end' gap={3}>
          <Button onClick={() => navigate("/genres")} color='inherit'>Cancel</Button>
          <Button
            type="submit"
            color='success'
            variant="contained"
          >Submit</Button>
        </Box>
      </Box>
    </Paper>
  )
}

export default GenreForm;