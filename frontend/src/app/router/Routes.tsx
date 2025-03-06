import {createBrowserRouter} from "react-router";
import App from "../App.tsx";
import GenreTable from "../../features/tables/GenreTable.tsx";
import GenreForm from "../../features/tables/GenreForm.tsx";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "genre", element: <GenreTable /> },
      { path: "genre/create", element: <GenreForm /> },
      { path: "genre/edit/:id", element: <GenreForm /> },
      { path: "*", element: <h1>Not found</h1> },
    ]
  }
])