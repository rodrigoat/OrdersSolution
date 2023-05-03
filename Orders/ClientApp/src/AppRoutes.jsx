import Home from "./components/Home";
import Grid from "./components/Grid";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/grid',
    element: <Grid />
  },
];

export default AppRoutes;
