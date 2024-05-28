
import { ColorButton } from "../Auxiliary";
import NoPage from "./NoPageF";
import { Faculties } from "../UniversitiesProvider";
import * as config from '../appconfig.json';

import { useState, useEffect, useContext } from "react";
import { Link, useParams } from "react-router-dom";
import axios from "axios";

import { Box, Grid } from "@mui/material";



export default function DetailsF() {
    let { id } = useParams();
    const { facultList, setFacultList } = useContext(Faculties);

    const mockFacult = { name: "", noOfStudents: -1, university: "" };
    const [objectReceived, setObjectReceived] = useState(mockFacult);
    

    useEffect(() => {
        /*async */function func() {
            /*const rez = await */axios.get(`${config.webapi}/faculties/details/${id}`).then((response) => {
                setObjectReceived(response.data);
            }).catch((error) => {
                if(error.request) {
                    const aux = facultList.filter(f => f.id === parseInt(id))[0];
                    setObjectReceived({ name: "", noOfStudents: -2, university: "" })
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
            { objectReceived.noOfStudents < 0 ? 
                objectReceived.noOfStudents === -2 ? 
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
                                    Number of students
                                </Grid>
                                <Grid item xs={8}>
                                    {objectReceived.noOfStudents}
                                </Grid>
                            </Grid>
                            <Grid container item spacing={3}>
                                <Grid item xs={3}>
                                    University
                                </Grid>
                                <Grid item xs={8}>
                                    {objectReceived.university}
                                </Grid>
                            </Grid>
                        </Grid>
                    </Grid>
                    <div className='backButtonCont'>
                        <Link to="/faculties"><ColorButton variant='text'>Back</ColorButton></Link>
                    </div>
                </Box>
            }
        </>
    );
}

