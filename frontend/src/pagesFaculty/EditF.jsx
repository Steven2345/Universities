import '../App.css'
import { Refresher, Faculties, Universities, Buffer, AccessToken } from '../UniversitiesProvider.jsx';
import { ColorButton, syncToServer } from '../Auxiliary.jsx';
import NoPage from './NoPageF.jsx';
import * as config from '../appconfig.json';

import { FormControl, InputLabel, MenuItem, Select, TextField } from "@mui/material";
import { Link, useParams } from 'react-router-dom';
import { useContext, useEffect, useState } from 'react';

import axios from 'axios';



export default function EditF() {
    let { id } = useParams();
    const { uniList, setUniList } = useContext(Universities);
    const { facultList, setFacultList } = useContext(Faculties);
    const { refreshNeeded, setRefreshNeeded } = useContext(Refresher);
    const [buffer, setBuffer] = useContext(Buffer);
    const [token, setToken] = useContext(AccessToken);

    const [nameValue, setNameValue] = useState("");
    const [noOfStudentsValue, setNoOfStudentsValue] = useState("");
    const [universityValue, setUniversityValue] = useState("");

    useEffect(() => {
        function func() {
            axios.get(`${config.webapi}/faculties/details/${id}`).then((rez) => {
                setNameValue(rez.data.name);
                setNoOfStudentsValue(rez.data.noOfStudents);
                setUniversityValue(rez.data.universityID);
            }).catch((error) => {
                if(error.request) {
                    const aux = facultList.filter(f => f.id === parseInt(id))[0];
                    setNoOfStudentsValue(-2);
                    if(aux !== undefined)
                        setNameValue(aux.name);
                        setNoOfStudentsValue(aux.noOfStudents);
                        setUniversityValue(aux.universityID);
                    alert("Connection lost, server is not accessible");
                }
            });
        }
        func();
    }, []);
  
    const handleName = e => {
        setNameValue(e.target.value);
    }
    const handleNoOfStudents = e => {
        setNoOfStudentsValue(e.target.value);
    }
    const handleUniversity = e => {
        setUniversityValue(e.target.value);
    }

    const sendNewElement = async () => {
        setBuffer(await syncToServer(buffer));
        await axios.put(`${config.webapi}/faculties/edit/${id}`, {
            "name": nameValue, 
            "noOfStudents": noOfStudentsValue, 
            "universityID": parseInt(universityValue)
        }, {
            headers: {
              'Authorization': 'Bearer ' + token
            }
        }).catch((error) => {
            if(error.response) {
                if(error.response.status === 401) {
                    alert("You are not the owner of the element you tried to modify");
                }
            }
            else if(error.request) {
                setBuffer([
                    ...buffer,
                    {
                        "type": "facult",
                        "id": id,
                        "name": nameValue, 
                        "noOfStudents": noOfStudentsValue, 
                        "universityID": parseInt(universityValue)
                    }
                ]);
            }
        });
    }

    return (
        <>
            { noOfStudentsValue === "" ?
                <h1>Loading...</h1> :
                noOfStudentsValue === -2 ?
                    <NoPage /> :
                    <>
                        <div className='bigTitleCont'>
                            <h1>Edit faculty</h1>
                        </div>
                        <div className='txtField'>
                            <TextField 
                                value={nameValue}     onChange={handleName} 
                                label="Name"          variant="standard" 
                                margin="normal"       fullWidth 
                                data-testid="nameEdit"
                            /><br/>
                            <TextField 
                                value={noOfStudentsValue} onChange={handleNoOfStudents}
                                label="Location"      variant="standard" 
                                margin="normal"       fullWidth 
                            /><br/>
                            <FormControl fullWidth margin="normal">
                                <InputLabel id="demo-simple-select-label">University</InputLabel>
                                <Select
                                    labelId="demo-simple-select-label"
                                    id="demo-simple-select"
                                    variant="standard"
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
                                            facultList.map(
                                                f => {
                                                    if (f.id === parseInt(id))
                                                        return { 
                                                            id: f.id, 
                                                            name: nameValue, 
                                                            noOfStudents: noOfStudentsValue, 
                                                            university: parseInt(universityValue)
                                                        };
                                                    else
                                                        return f;
                                                }
                                            )
                                        );
                                        setRefreshNeeded(true);
                                        sendNewElement();
                                    } 
                                }>Update element</ColorButton>
                            </Link>
                            <Link to='/faculties'>
                                <ColorButton>Cancel</ColorButton>
                            </Link>
                        </div>
                    </>
            }
        </>
    );
}

