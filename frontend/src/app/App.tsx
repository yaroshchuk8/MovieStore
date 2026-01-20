import { Outlet } from "react-router";
import NavBar from "./layout/NavBar.tsx";
import {Container, CssBaseline} from "@mui/material";

function App() {
  return (
    <>
      <CssBaseline />
      <NavBar />
      <Container maxWidth='xl'>
        <Outlet />
      </Container>
    </>
  );
}

export default App;