
import './App.css'
import Master from './pages/Master.jsx';
import Layout from './pages/Layout.jsx';
import NoPage from './pages/NoPage.jsx';
import Add from './pages/Add.jsx';
import Edit from './pages/Edit.jsx';
import Details from './pages/Details.jsx';
import MasterF from './pagesFaculty/MasterF.jsx';
import NoPageF from './pagesFaculty/NoPageF.jsx';
import AddF from './pagesFaculty/AddF.jsx';
import EditF from './pagesFaculty/EditF.jsx';
import DetailsF from './pagesFaculty/DetailsF.jsx';
import UniversitiesProvider from './UniversitiesProvider.jsx';
import Login from './Login.jsx';
import Register from './Register.jsx';

import { BrowserRouter, Routes, Route } from "react-router-dom";




export default function App() {

  return (
    <UniversitiesProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route index element={<Master />} />
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
            <Route path="add" element={<Add />} />
            <Route path="edit/:id" element={<Edit />} />
            <Route path="details/:id" element={<Details />} />
            <Route path="faculties" element={<MasterF />} />
            <Route path="faculties/add" element={<AddF />} />
            <Route path="faculties/edit/:id" element={<EditF />} />
            <Route path="faculties/details/:id" element={<DetailsF />} />
            <Route path="faculties/*" element={<NoPageF />} />
            <Route path="*" element={<NoPage />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </UniversitiesProvider>
  )
}


