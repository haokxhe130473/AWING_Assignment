import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import CreateTreasureMap from "./pages/CreateTreasureMap";
import TreasureMapList from "./pages/TreasureMapList";
import { AppBar, Toolbar, Button } from "@mui/material";

function App() {
  return (
    <Router>
      <AppBar position="static">
        <Toolbar>
          <Button color="inherit" component={Link} to="/">
            Danh sách bản đồ
          </Button>
          <Button color="inherit" component={Link} to="/create">
            Tạo bản đồ
          </Button>
        </Toolbar>
      </AppBar>
      <Routes>
        <Route path="/" element={<TreasureMapList />} />
        <Route path="/create" element={<CreateTreasureMap />} />
      </Routes>
    </Router>
  );
}

export default App;
