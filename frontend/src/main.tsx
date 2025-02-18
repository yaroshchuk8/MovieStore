import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'

import "primereact/resources/primereact.min.css";
import "primereact/resources/themes/soho-light/theme.css";

import "./app/styles.css"
import App from './app/App.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
