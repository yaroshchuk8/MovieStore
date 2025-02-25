import {AppBar, Box, Container, MenuItem, Toolbar, Typography} from "@mui/material";
import MovieFilterIcon from '@mui/icons-material/MovieFilter';
import { NavLink } from "react-router";

export default function NavBar() {
  return (
    <Box sx={{flexGrow: 1}}>
      <AppBar position="static" sx={{backgroundImage: 'linear-gradient(135deg, #182a73 0%, #218aae 69%, #20a7ac 89%)'}}>
        <Container maxWidth='xl'>
          <Toolbar>
            <MenuItem component={NavLink} to='/' sx={{ gap: 2 }}>
              <MovieFilterIcon />
              <Typography variant="h5" sx={{flexGrow: 1}} fontWeight='bold'>
                MovieStore
              </Typography>
            </MenuItem>
            <MenuItem component={NavLink} to='/genre' sx={{
              fontSize: '1rem',
              textTransform: 'uppercase',
              fontWeight: 'bold',
              color: 'inherit',
              '&.active': {
                color: 'yellow'
              }
            }}>
              Genre
            </MenuItem>
          </Toolbar>
        </Container>
      </AppBar>
    </Box>
  );
}