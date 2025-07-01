import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import CreateTreasureMap from "./pages/CreateTreasureMap";
import TreasureMapList from "./pages/TreasureMapList";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<TreasureMapList />} />
        <Route path="/create" element={<CreateTreasureMap />} />
      </Routes>
    </Router>
  );
}

export default App;
