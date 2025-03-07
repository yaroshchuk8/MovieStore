import axios from "axios";
import {useEffect, useState} from "react";
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import Paper from '@mui/material/Paper';
import {MovieSmallInDto} from "../../types/movie.ts";
import {GenreInDto} from "../../types/genre.ts";
import {Box, Button, IconButton, Tooltip, Typography} from "@mui/material";
import {useNavigate} from "react-router";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';

function GenreTable() {
  const [genres, setGenres] = useState<GenreInDto[]>([]);
  const apiUrl = "http://localhost:5000/api/genres";

  const navigate = useNavigate();

  useEffect(() => {
    axios.get<GenreInDto[]>(apiUrl)
      .then((response) => {
        setGenres(response.data);
      })
      .catch((error) => {
        console.error("Error fetching genres:", error);
      });
  }, []);

  const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', flex: 1 },
    { field: 'name', headerName: 'Name', flex: 1 },
    {
      field: 'associatedMovies',
      headerName: 'Associated Movies',
      flex: 2,
      description: 'Movies associated with respective genre',
      sortable: false,
      valueGetter: (_value, row) =>
        row.movies.length > 0 ? row.movies.map((movie: MovieSmallInDto) => movie.title).join(", ") : "No movies"
    },
    {
      field: 'Actions',
      headerName: '',
      sortable: false,
      disableColumnMenu: true,
      renderCell: (params) => {
        const genreId = params.row.id;
        return (
          <>
            <Tooltip title="Click to edit">
              <IconButton
                onClick={() => navigate(`/genres/edit/${genreId}`, {state: {genre: params.row}})}
                color="info"
              >
                <EditIcon />
              </IconButton>
            </Tooltip>
            <Tooltip title="Click to delete">
              <IconButton
                onClick={() => axios.delete(`http://localhost:5000/api/genres/${genreId}`)
                  .then(() => setGenres(genres.filter(genre => genre.id !== genreId)))}
              >
                <DeleteIcon />
              </IconButton>
            </Tooltip>
          </>
        );
      },
    },
  ];

  return (
    <>
      <Box display="flex" justifyContent="space-between" sx={{marginY: 2, paddingX: 1}} >
        <Typography variant="h4">Genre table</Typography>
        <Tooltip title="Click to create">
          <Button onClick={() => navigate(`/genre/create`)} variant="text" color="primary">
            <AddCircleOutlineIcon fontSize="large" />
          </Button>
        </Tooltip>
      </Box>
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
}

export default GenreTable;