import '../App.css'
import { Refresher, Universities, Buffer, AccessToken } from '../UniversitiesProvider.jsx';
import { ColorButton, syncToServer } from '../Auxiliary';
import NoPage from './NoPage';
import * as config from '../appconfig.json';

import { TextField } from "@mui/material";
import { Link, useParams } from 'react-router-dom';
import { useContext, useEffect, useState } from 'react';

import axios from 'axios';



export default function Edit() {
    let { id } = useParams();
    const { uniList, setUniList } = useContext(Universities);
    const { refreshNeeded, setRefreshNeeded } = useContext(Refresher);
    const [buffer, setBuffer] = useContext(Buffer);
    const [token, setToken] = useContext(AccessToken);

    const [nameValue, setNameValue] = useState("");
    const [locationValue, setLocationValue] = useState("");
    const [scoreValue, setScoreValue] = useState("");
    const [descrValue, setDescrValue] = useState("");

    useEffect(() => {
        function func() {
            axios.get(`${config.webapi}/details/${id}`).then((rez) => {
                setNameValue(rez.data.name);
                setLocationValue(rez.data.location);
                setScoreValue(rez.data.score);
                setDescrValue(rez.data.description);
            }).catch((error) => {
                if(error.request) {
                    const aux = uniList.filter(uni => uni.id === parseInt(id))[0];
                    setScoreValue(-2);
                    if(aux !== undefined)
                        console.log("in da if!!")
                        setNameValue(aux.name);
                        setLocationValue(aux.location);
                        setScoreValue(aux.score);
                        setDescrValue(aux.description);
                    alert("Connection lost, server is not accessible");
                }
            });
        }
        func();
    }, []);
  
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

    const sendNewElement = async () => {
        setBuffer(await syncToServer(buffer));
        await axios.put(`${config.webapi}/edit/${id}`, {
            "name": nameValue, 
            "location": locationValue, 
            "score": parseFloat(scoreValue), 
            description: descrValue
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
                        "type": "uni",
                        "id": id,
                        "name": nameValue, 
                        "location": locationValue, 
                        "score": parseFloat(scoreValue), 
                        "description": descrValue
                    }
                ]);
            }
        });
    }

    return (
        <>
            { scoreValue === "" ?
                <h1>Loading...</h1> :
                scoreValue === -2 ?
                    <NoPage /> :
                    <>
                        <div className='bigTitleCont'>
                            <h1>Edit university</h1>
                        </div>
                        <div className='txtField'>
                            <TextField 
                                value={nameValue}     onChange={handleName} 
                                label="Name"          variant="standard" 
                                margin="normal"       fullWidth 
                                data-testid="nameEdit"
                            /><br/>
                            <TextField 
                                value={locationValue} onChange={handleLocation}
                                label="Location"      variant="standard" 
                                margin="normal"       fullWidth 
                            /><br/>
                            <TextField 
                                value={scoreValue}    onChange={handleScore} 
                                label="Score"         variant="standard" 
                                margin="normal"       fullWidth 
                            /><br/>
                            <TextField 
                                value={descrValue}    onChange={handleDescr} 
                                label="Description"   variant="standard" 
                                margin="normal"       fullWidth 
                                multiline 
                            /><br/> 
                        </div>
                        <div className='generalButtonCont'>
                            <Link to='/'>
                                <ColorButton onClick={ 
                                    () => {
                                        setUniList(
                                            uniList.map(
                                                uni => {
                                                    if (uni.id === parseInt(id))
                                                        return { 
                                                            id: uni.id, 
                                                            name: nameValue, 
                                                            location: locationValue, 
                                                            score: scoreValue, 
                                                            description: descrValue
                                                        };
                                                    else
                                                        return uni;
                                                }
                                            )
                                        );
                                        setRefreshNeeded(true);
                                        sendNewElement();
                                    } 
                                }>Update element</ColorButton>
                            </Link>
                            <Link to='/'>
                                <ColorButton>Cancel</ColorButton>
                            </Link>
                        </div>
                    </>
            }
        </>
    );
}

