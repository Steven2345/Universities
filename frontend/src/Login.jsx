import { Link, useNavigate } from "react-router-dom";
import { ColorButton } from "./Auxiliary";
import * as config from './appconfig.json';
import './App.css';
import { AccessToken } from "./UniversitiesProvider";

import axios from 'axios';

import { useState, useContext } from "react";

import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import InputAdornment from '@mui/material/InputAdornment';
import FormControl from '@mui/material/FormControl';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';


export default function Login() {
    const [token, setToken] = useContext(AccessToken);

    const [showPassword, setShowPassword] = useState(false);

    const handleClickShowPassword = () => setShowPassword((show) => !show);

    const handleMouseDownPassword = (event) => {
        event.preventDefault();
    };

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const handleUsername = e => {
        setUsername(e.target.value);
    }
    const handlePassword = e => {
        setPassword(e.target.value);
    }

    const navigate = useNavigate();

    const loginRequest = async () => {
        axios.post(`${config.webapi}/login`, {
            "email": username,
            "password": password
        }).then((rez) => {
            setToken(rez.data.accessToken);
            navigate('/');
        }).catch((error) => {
            if(error.response) {
                if(error.response.status === 401) {
                    alert("Wrong credentials");
                }
            }
            else if(error.request) {
                alert("Connection lost");
            }
        });
    }

    return (
        <>
        <Link to="/"><ColorButton variant='text'>See universities</ColorButton></Link>
        <Link to="/faculties"><ColorButton variant='text'>See faculties</ColorButton></Link>
        
        <div className='accountManagerCont'>
            <h1>Login</h1>
        </div>
        
        <Box sx={{ display: 'flex', flexWrap: 'wrap' }}>
            <div className='loginField'>
                <FormControl sx={{ m: 1, width: '500px' }} variant="outlined">
                <InputLabel htmlFor="outlined-adornment-password">Username</InputLabel>
                <OutlinedInput
                    value={username}
                    onChange={handleUsername}
                    id="outlined-adornment-username"
                    label="Username"
                />
                </FormControl>
                <FormControl sx={{ m: 1, width: '500px' }} variant="outlined">
                <InputLabel htmlFor="outlined-adornment-password">Password</InputLabel>
                <OutlinedInput
                    value={password}
                    onChange={handlePassword}
                    id="outlined-adornment-password"
                    type={showPassword ? 'text' : 'password'}
                    endAdornment={
                    <InputAdornment position="end">
                        <IconButton
                        aria-label="toggle password visibility"
                        onClick={handleClickShowPassword}
                        onMouseDown={handleMouseDownPassword}
                        edge="end"
                        >
                        {showPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                    </InputAdornment>
                    }
                    label="Password"
                />
                </FormControl>
            </div>
        </Box>

        <div className='accountManagerButton'>
            <ColorButton onClick={ 
                loginRequest
            }>Sign in</ColorButton>
        </div>
        </>
    );
}
