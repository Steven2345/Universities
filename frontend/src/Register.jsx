import { Link, useNavigate } from "react-router-dom";
import { ColorButton } from "./Auxiliary";
import * as config from './appconfig.json';
import './App.css';

import { createRef, useState } from "react";

import axios from "axios";

import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import InputAdornment from '@mui/material/InputAdornment';
import FormControl from '@mui/material/FormControl';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import { FormHelperText } from "@mui/material";


export default function Register() {
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmpassword, setShowConfirmpassword] = useState(false);

    const handleClickShowPassword = () => setShowPassword((show) => !show);

    const handleMouseDownPassword = (event) => {
        event.preventDefault();
    };

    const handleClickShowConfirmpassword = () => setShowConfirmpassword((show) => !show);

    const handleMouseDownConfirmpassword = (event) => {
        event.preventDefault();
    };

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmpassword, setConfirmpassword] = useState("");

    const handleUsername = e => {
        setUsername(e.target.value);
    }
    const handlePassword = e => {
        setPassword(e.target.value);
    }
    const handleConfirmpassword = e => {
        setConfirmpassword(e.target.value);
        if(password !== confirmpassword) {
            const conf = confirmInput.current;
            console.log(conf);
            conf.error = true;
        }
    }
    const confirmInput = createRef();

    const navigate = useNavigate();

    const registerRequest = async () => {
        axios.post(`${config.webapi}/register`, {
            "email": username,
            "password": password
        }).then((rez) => {
            navigate('/login');
        }).catch((error) => {
            if(error.response) {
                if(error.response.status === 400) {
                    alert(`Invalid data: ${Object.values(error.response.data.errors)[0]}`);
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
            <h1>Register</h1>
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
                <FormControl ref={confirmInput} sx={{ m: 1, width: '500px' }} variant="outlined">
                <InputLabel htmlFor="outlined-adornment-confirmpassword">Confirm password</InputLabel>
                <OutlinedInput
                    value={confirmpassword}
                    onChange={handleConfirmpassword}
                    id="outlined-adornment-confirmpassword"
                    type={showConfirmpassword ? 'text' : 'password'}
                    endAdornment={
                    <InputAdornment position="end">
                        <IconButton
                        aria-label="toggle password visibility"
                        onClick={handleClickShowConfirmpassword}
                        onMouseDown={handleMouseDownConfirmpassword}
                        edge="end"
                        >
                        {showConfirmpassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                    </InputAdornment>
                    }
                    label="Confirm password"
                />
                <FormHelperText id="component-error-text">Passwords do not match</FormHelperText>
                </FormControl>
            </div>
        </Box>

        <div className='accountManagerButton'>
            <ColorButton onClick={ 
                registerRequest
            }>Sign up</ColorButton>
        </div>
        </>
    );
}
