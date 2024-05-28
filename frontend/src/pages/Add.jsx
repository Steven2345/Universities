import '../App.css'
import { Refresher, Universities, Buffer, AccessToken } from '../UniversitiesProvider.jsx';
import { ColorButton, syncToServer } from '../Auxiliary';
import { createUni } from '../Auxiliary';
import * as config from '../appconfig.json';

import { TextField } from "@mui/material";
import { Link } from 'react-router-dom';
import { useContext, useState } from 'react';

import axios from 'axios';



export default function Add() { 
    const { uniList, setUniList } = useContext(Universities);
    const { refreshNeeded, setRefreshNeeded } = useContext(Refresher);
    const [buffer, setBuffer] = useContext(Buffer);
    const [token, setToken] = useContext(AccessToken);

    const [nameValue, setNameValue] = useState("");
    const [locationValue, setLocationValue] = useState("");
    const [scoreValue, setScoreValue] = useState("");
    const [descrValue, setDescrValue] = useState("");
  
    const handleName = e => {
        setNameValue(e.target.value);
    }
    const handleLocation = e => {
        setLocationValue(e.target.value);
    }
    const handleScore = e => {
        setScoreValue(e.target.value);
    }
    const handleDescr = e => {
        setDescrValue(e.target.value);
    }

    const addRequest = async () => {
        setBuffer(await syncToServer(buffer));
        axios.post(`${config.webapi}/add`, {
            "name": nameValue, 
            "location": locationValue, 
            "score": parseFloat(scoreValue), 
            "description": descrValue
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
                        "type": "uni",
                        "name": nameValue, 
                        "location": locationValue, 
                        "score": parseFloat(scoreValue), 
                        description: descrValue
                    }
                ]);
                alert("Connection lost, request could not be sent");
            }
        });
    }
  
  
    return (
        <>
            <div className='bigTitleCont'>
                <h1>Add a new university</h1>
            </div>
            <div className='txtField'>
                <TextField 
                    value={nameValue}     onChange={handleName} 
                    label="Name"          variant="standard" 
                    margin="normal"       fullWidth 
                    data-testid="nameAdd"
                /><br/>
                <TextField 
                    value={locationValue} onChange={handleLocation}
                    label="Location"      variant="standard" 
                    margin="normal"       fullWidth 
                    data-testid="locationAdd"
                /><br/>
                <TextField 
                    value={scoreValue}    onChange={handleScore} 
                    label="Score"         variant="standard" 
                    margin="normal"       fullWidth 
                    data-testid="scoreAdd"
                /><br/>
                <TextField 
                    value={descrValue}    onChange={handleDescr} 
                    label="Description"   variant="standard" 
                    margin="normal"       fullWidth 
                    multiline 
                    data-testid="descrAdd"
                /><br/> 
            </div>
            <div className='generalButtonCont'>
                <Link to='/'>
                    <ColorButton onClick={ 
                        () => {
                            setUniList(
                                [
                                    ...uniList,
                                    createUni(nameValue, locationValue, scoreValue, descrValue)
                                ]
                            );
                            console.log(nameValue, locationValue, scoreValue, descrValue);
                            setRefreshNeeded(true);
                            addRequest();
                        } 
                    }>Add to list</ColorButton>
                </Link>
                <Link to='/'>
                    <ColorButton>Cancel</ColorButton>
                </Link>
            </div>
        </>
    );
  }
  