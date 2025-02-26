import axios from "axios";
import {useEffect, useState} from "react";
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import Paper from '@mui/material/Paper';
import {MovieSmallInDto} from "../../types/movie.ts";
import {GenreInDto} from "../../types/genre.ts";

const columns: GridColDef[] = [
  { field: 'id', headerName: 'ID', flex: 1 },
  { field: 'name', headerName: 'Name', flex: 1 },
  {
    field: 'associatedMovies',
    headerName: 'Associated Movies',
    flex: 2,
    description: 'This column has a value getter and is not sortable.',
    sortable: false,
    valueGetter: (_value, row) => row.movies.length > 0 ? row.movies.map((movie: MovieSmallInDto) => movie.title).join(", ") : "No movies"
  },
];

const GenreTable = () => {
  const [genres, setGenres] = useState<GenreInDto[]>([]);
  const apiUrl = "http://localhost:5000/api/genre";

  useEffect(() => {
    axios.get<GenreInDto[]>(apiUrl)
      .then((response) => {
        setGenres(response.data);
      })
      .catch((error) => {
        console.error("Error fetching genres:", error);
      });
  }, []);

  return (
    <>
      <h1>Genre table</h1>
      <Paper sx={{ width: '100%' }}>
        <DataGrid
          rows={genres}
          columns={columns}
          initialState={{
            pagination: {
              paginationModel: {
                pageSize: 5,
              },
            },
          }}
          pageSizeOptions={[5, 10]}
          disableRowSelectionOnClick
        />
      </Paper>
    </>
  );
};

export default GenreTable;