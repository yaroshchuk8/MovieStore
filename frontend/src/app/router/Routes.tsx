import {createBrowserRouter} from "react-router";
import App from "../layout/App.tsx";
import GenreTable from "../../features/tables/GenreTable.tsx";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "genre", element: <GenreTable /> },
      { path: "*", element: <div>Not found</div> }
    ]
  }
])