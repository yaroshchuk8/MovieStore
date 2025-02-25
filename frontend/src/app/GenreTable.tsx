import axios from "axios";
import {useEffect, useState} from "react";
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import Paper from '@mui/material/Paper';

interface MovieSmallOutDto {
  id: string;
  title: string;
}

interface GenreOutDto {
  id: string;
  name: string;
  movies: MovieSmallOutDto[];
}

const columns: GridColDef[] = [
  { field: 'id', headerName: 'ID' },
  { field: 'name', headerName: 'Name' },
  {
    field: 'associatedMovies',
    headerName: 'Associated Movies',
    description: 'This column has a value getter and is not sortable.',
    sortable: false,
    valueGetter: (_value, row) => row.movies.length > 0 ? row.movies.map((movie: MovieSmallOutDto) => movie.title).join(", ") : "No movies"
  },
];

const GenreTable = () => {
  const [genres, setGenres] = useState<GenreOutDto[]>([]);
  const apiUrl = "http://localhost:5000/api/genre";

  useEffect(() => {
    axios.get<GenreOutDto[]>(apiUrl)
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