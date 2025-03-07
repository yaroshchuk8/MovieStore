import {Autocomplete, Box, Button, Paper, TextField, Typography,} from "@mui/material";
import {GenreInDto, GenreOutDto} from "../../types/genre.ts";
import {Controller, useForm} from "react-hook-form";
import {MovieSmallInDto} from "../../types/movie.ts";
import axios from "axios";
import {useLocation, useNavigate, useParams} from "react-router";
import {useEffect, useState} from "react";
import { v4 } from 'uuid';

function GenreForm() {
  const navigate = useNavigate();
  const location = useLocation();
  const { id } = useParams();
  const [selectedGenre, setSelectedGenre] = useState<GenreInDto>(location.state?.genre);
  const [movies, setMovies] = useState<MovieSmallInDto[]>([]);
  const { handleSubmit, control, register, formState: { errors }, reset } = useForm<GenreInDto>({
    defaultValues: selectedGenre ? selectedGenre : {
      id: v4(), // random guid to create new genre, will be replaced in edit mode
      name: '',
      movies: []
    }}
  );

  useEffect(() => {
    // fetch genre if navigated directly via link
    if (!selectedGenre && id) {
      axios.get<GenreInDto>(`http://localhost:5000/api/genres/${id}`)
        .then(response => setSelectedGenre(response.data))
        .catch(error => console.error("Failed to fetch genre:", error));
    }
    // fetch movies
    axios.get<MovieSmallInDto[]>('http://localhost:5000/api/genres/movies')
      .then(response => setMovies(response.data))
      .catch(error => console.error("Failed to fetch movies:", error));
  }, []);

  useEffect(() => {
    // update form
    if (selectedGenre) {
      reset(selectedGenre);
    }
  }, [selectedGenre]);

  const onSubmit = (data: GenreInDto) => {
    const genre: GenreOutDto = {
      id: data.id,
      name: data.name,
      movieIds: data.movies.map((movie: MovieSmallInDto) => movie.id)
    }

    if (selectedGenre) {
      // update
      axios.put<GenreOutDto>("http://localhost:5000/api/genres", genre)
        .then(() => navigate("/genre"));
    } else {
      // create
      axios.post<GenreOutDto>("http://localhost:5000/api/genres", genre)
        .then(() => navigate("/genre"));
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
          <Button onClick={() => navigate("/genre")} color='inherit'>Cancel</Button>
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