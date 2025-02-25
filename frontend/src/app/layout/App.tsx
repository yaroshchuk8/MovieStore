import { Outlet } from "react-router";
import NavBar from "./NavBar.tsx";
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