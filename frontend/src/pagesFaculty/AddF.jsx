import '../App.css'
import { Refresher, Faculties, Universities, Buffer, AccessToken } from '../UniversitiesProvider.jsx';
import { ColorButton, syncToServer } from '../Auxiliary';
import * as config from '../appconfig.json';

import { FormControl, InputLabel, MenuItem, Select, TextField } from "@mui/material";
import { Link } from 'react-router-dom';
import { useContext, useState } from 'react';

import axios from 'axios';



export default function AddF() { 
    const { uniList, setUniList } = useContext(Universities);
    const { facultList, setFacultList } = useContext(Faculties);
    const { refreshNeeded, setRefreshNeeded } = useContext(Refresher);
    const [buffer, setBuffer] = useContext(Buffer);
    const [token, setToken] = useContext(AccessToken);

    const [nameValue, setNameValue] = useState("");
    const [noOfStudentsValue, setNoOfStudentsValue] = useState("");
    const [universityValue, setUniversityValue] = useState("");
  
    const handleName = e => {
        setNameValue(e.target.value);
    }
    const handleNoOfStudents = e => {
        setNoOfStudentsValue(e.target.value);
    }
    const handleUniversity = e => {
        setUniversityValue(e.target.value);
    }

    const addRequest = async () => {
        setBuffer(await syncToServer(buffer));
        axios.post(`${config.webapi}/faculties/add`, {
            "name": nameValue, 
            "noOfStudents": noOfStudentsValue, 
            "universityID": parseInt(universityValue) // should be int already
        }, {
            headers: {
              'Authorization': 'Bearer ' + token
            }
        }).catch((error) => {
            if(error.response) {
                if(error.response.status === 401) {
                    alert("Log in to add elements");
                }
            }
            else if(error.request) {
                setBuffer([
                    ...buffer,
                    {
                        "type": "facult",
                        "name": nameValue, 
                        "noOfStudents": noOfStudentsValue, 
                        "universityID": parseInt(universityValue)
                    }
                ]);
                alert("Connection lost, request could not be sent");
            }
        });
    }
  
  
    return (
        <>
            <div className='bigTitleCont'>
                <h1>Add a new faculty</h1>
            </div>
            <div className='txtField'>
                <TextField 
                    value={nameValue}     onChange={handleName} 
                    label="Name"          variant="standard" 
                    margin="normal"       fullWidth 
                    data-testid="nameAdd"
                /><br/>
                <TextField 
                    value={noOfStudentsValue} onChange={handleNoOfStudents}
                    label="Number of students"      variant="standard" 
                    margin="normal"       fullWidth 
                    data-testid="nostudAdd"
                /><br/>
                <FormControl fullWidth margin='normal'>
                    <InputLabel id="demo-simple-select-label">University</InputLabel>
                    <Select
                        labelId="demo-simple-select-label"
                        id="demo-simple-select"
                        variant='standard'
                        value={universityValue}
                        label="University"
                        onChange={handleUniversity}
                    >
                        {uniList.map(uni => (
                            <MenuItem value={uni.id}>{uni.name}</MenuItem>
                        ))}
                    </Select>
                </FormControl><br/>
            </div>
            <div className='generalButtonCont'>
                <Link to='/faculties'>
                    <ColorButton onClick={ 
                        () => {
                            setFacultList(
                                [
                                    ...facultList,
                                    createFacult(nameValue, noOfStudentsValue, universityValue)
                                ]
                            );
                            setRefreshNeeded(true);
                            addRequest();
                        } 
                    }>Add to list</ColorButton>
                </Link>
                <Link to='/faculties'>
                    <ColorButton>Cancel</ColorButton>
                </Link>
            </div>
        </>
    );
  }
  