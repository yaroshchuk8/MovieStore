import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import axios from "axios";
import {useEffect, useState} from "react";

interface MovieSmallOutDto {
  id: string;
  title: string;
}

interface GenreOutDto {
  id: string;
  name: string;
  movies: MovieSmallOutDto[];
}

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

  const moviesTemplate = (rowData: GenreOutDto) => {
    return rowData.movies.length > 0
      ? rowData.movies.map((movie) => movie.title).join(", ")
      : "No movies";
  };

  return (
    <div className="card">
      <DataTable value={genres} rows={5} responsiveLayout="scroll">
        <Column field="id" header="Id"></Column>
        <Column field="name" header="Name"></Column>
        <Column header="Associated Movies" body={moviesTemplate}></Column>
      </DataTable>
    </div>
  );
};

export default GenreTable;