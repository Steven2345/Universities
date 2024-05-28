
import { ColorButton } from "../Auxiliary";
import NoPage from "./NoPage";
import { Universities } from "../UniversitiesProvider";
import * as config from '../appconfig.json';

import { useState, useEffect, useContext } from "react";
import { Link, useParams } from "react-router-dom";
import axios from "axios";

import { Box, Grid } from "@mui/material";



export default function Details() {
    let { id } = useParams();
    const { uniList, setUniList } = useContext(Universities);

    const mockUni = { name: "", location: "", noOfFaculties: -1, score: -1, description: "" };
    const [objectReceived, setObjectReceived] = useState(mockUni);
    

    useEffect(() => {
        /*async */function func() {
            /*const rez = await */axios.get(`${config.webapi}/details/${id}`).then((response) => {
                setObjectReceived(response.data);
            }).catch((error) => {
                if(error.request) {
                    const aux = uniList.filter(uni => uni.id === parseInt(id))[0];
                    setObjectReceived({ name: "", location: "", noOfFaculties: -1, score: -2, description: "" })
                    if(aux !== undefined)
                        setObjectReceived(aux);
                    alert("Connection lost, data could not be accessed");
                }
            });
        }
        func();
    }, []);

    return (
        <>
            { objectReceived.score < 0 ? 
                objectReceived.score === -2 ? 
                    <NoPage /> : 
                    <h1>Loading...</h1> :
                <Box sx={{ flexGrow: 1 }}>
                    <Grid container spacing={2}>
                        <h1>Details</h1>
                        <Grid container item spacing={5}>
                            <Grid container item spacing={3}>
                                <Grid item xs={3}>
                                    Name
                                </Grid>
                                <Grid item xs={8}>
                                    {objectReceived.name}
                                </Grid>
                            </Grid>
                            <Grid container item spacing={3}>
                                <Grid item xs={3}>
                                    Location
                                </Grid>
                                <Grid item xs={8}>
                                    {objectReceived.location}
                                </Grid>
                            </Grid>
                            <Grid container item spacing={3}>
                                <Grid item xs={3}>
                                    Number of faculties
                                </Grid>
                                <Grid item xs={8}>
                                    {objectReceived.noOfFaculties}
                                </Grid>
                            </Grid>
                            <Grid container item spacing={3}>
                                <Grid item xs={3}>
                                    Score
                                </Grid>
                                <Grid item xs={8}>
                                    {objectReceived.score}
                                </Grid>
                            </Grid>
                            <Grid container item spacing={3}>
                                <Grid item xs={3}>
                                    Description
                                </Grid>
                                <Grid item xs={8}>
                                    {objectReceived.description}
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
                    <div className='backButtonCont'>
                        <Link to="/"><ColorButton variant='text'>Back</ColorButton></Link>
                    </div>
                </Box>
            }
        </>
    );
}

