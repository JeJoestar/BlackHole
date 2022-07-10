import logo from './logo.svg';
import './App.css';
import {
  BrowserRouter as Router,
  Route,
  Link,
  Routes
} from "react-router-dom";
import Login from "./components/Login"
import Signup from './components/Signup';
import CatalogPage from './components/CatalogPage';
import AdminCatalogPage from './components/AdminCatalogPage';
import EditPage from './components/EditPage'

function App() {
  return (
    <div className="main">
    <Router>
      <Routes>
        <Route exec path="/" element={<Login/>} />
        <Route path="/login" element={<Login/>} />
        <Route path="/signup" element={<Signup/>}/>
        <Route path="/catalog" element={<CatalogPage/>}/>
        <Route path="/admin" element={<AdminCatalogPage/>}/>
        <Route path="/edit/:id" element={<EditPage/>}/>
      </Routes>
    </Router>
    </div>
  );
}

export default App;
