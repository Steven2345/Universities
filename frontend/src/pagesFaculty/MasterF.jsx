
import { ColorButton, syncToServer } from '../Auxiliary.jsx';
import { Faculties, Universities, Refresher, Buffer } from '../UniversitiesProvider.jsx';
import '../App.css';
import * as config from '../appconfig.json';

import { useState, useContext, useEffect } from 'react';
import { Link } from "react-router-dom";

import axios from 'axios';
import InfiniteScroll from 'react-infinite-scroll-component';

import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import TableFooter from '@mui/material/TableFooter';
import Paper from '@mui/material/Paper';
import { IconButton, TablePagination } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';


let once = false;

export default function MasterF() {
    const { facultList, setFacultList } = useContext(Faculties);
    const { uniList, setUniList } = useContext(Universities);

    // infinite scroll
    const [hasMore, setHasMore] = useState(true);
    const [index, setIndex] = useState(1);

    const fetchMoreData = () => {
        axios.get(`${config.webapi}/faculties/${index}`)
            .then((res) => {
                setFacultList((prevItems) => [...prevItems, ...res.data]);
                res.data.length > 0 ? setHasMore(true) : setHasMore(false);                
            })
            .catch((err) => console.log(err));
        setIndex((prevIndex) => prevIndex + 1);
    }

    // request buffering if network is down
    const [buffer, setBuffer] = useContext(Buffer);

    // removal confirmation
    const [open, setOpen] = useState(false);
    const [victim, setVictim] = useState(null);
    
    const openRemoveConfirm = (number) => {
        setOpen(true);
        setVictim(number);
    };

    const closeRemoveConfirm = () => {
        setOpen(false);
        setVictim(null);
    };

    // populate tabel
    const { refreshNeeded, setRefreshNeeded } = useContext(Refresher);

    const requestListFromServer = () => {
        //if(refreshNeeded) {
            axios.get(`${config.webapi}/faculties`).then((response) => {
                setFacultList(response.data);
                setRefreshNeeded(false);
                ////////////////////////////////////
            }).catch((error) => {
                if(error.request) {
                    alert("Connection lost, data could not be fetched");
                }
            })
        //}
    }

    useEffect(() => {
        requestListFromServer();
    }, []);

    // requests to server
    const removeRequest = async () => {
        setBuffer(await syncToServer(buffer));
        axios.delete(`${config.webapi}/faculties/delete/${victim}`, {}, {
            headers: {
              'Authorization': 'Bearer ' + token
            }
        }).then(() => {
            setFacultList(
                facultList.filter(f => f.id !== victim)
            );
        }).catch(() => {
            if(error.response) {
                if(error.response.status === 401) {
                    alert("You are not the owner of the element you tried to delete");
                }
            }
            else {
                setBuffer([
                    ...buffer,
                    {
                        "type": "facult",
                        "id": victim
                    }
                ]);
                alert("Connection lost, request couldn't be processed");
            }
        });
    }

    // return
    return (
        <>
            <Link to="/"><ColorButton variant='text'>See universities</ColorButton></Link>

            <div className='bigTitleCont'><h1>Faculties in the USA</h1></div>

            <div className='generalButtonCont'>
                <Link to="/faculties/add"><ColorButton variant='text'>Add faculty</ColorButton></Link>
            </div>

            { facultList.length < 1 ?
                <div>No elements</div> : 
                <InfiniteScroll
                    dataLength={uniList.length}
                    next={fetchMoreData}
                    hasMore={hasMore}
                    loader={<h4>Loading...</h4>}
                >
                    <TableContainer component={Paper} className='tableCont'>
                    <Table sx={{ minWidth: 950 }} aria-label="simple table">
                        <TableHead>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell align="right">University</TableCell>
                                <TableCell align="right"></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {facultList.map((faculty) => (
                                <TableRow
                                    key={faculty.id}
                                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                >
                                    <TableCell component="th" scope="row">{faculty.name}</TableCell>
                                    <TableCell align="right">{faculty.university}</TableCell>
                                    <TableCell align="right">
                                        <Link to={`details/${faculty.id}`}>
                                            <ColorButton variant="text">Details</ColorButton>
                                        </Link>
                                        <Link to={`edit/${faculty.id}`}>
                                            <ColorButton variant="text">Edit</ColorButton>
                                        </Link>
                                        <IconButton data-testid={`id${faculty.id}`} aria-label="delete" 
                                                    onClick={() => openRemoveConfirm(faculty.id) }>
                                            <DeleteIcon />
                                        </IconButton>
                                        <Dialog
                                            open={open}
                                            onClose={closeRemoveConfirm}
                                            aria-labelledby="alert-dialog-title"
                                            aria-describedby="alert-dialog-description"
                                        >
                                            <DialogTitle id="alert-dialog-title">
                                                Are you sure you want to delete this entry?
                                            </DialogTitle>
                                            <DialogContent>
                                                <DialogContentText id="alert-dialog-description">
                                                    This action cannot be reverted.
                                                </DialogContentText>
                                            </DialogContent>
                                            <DialogActions>
                                                <ColorButton data-testid={`del${faculty.id}`} onClick={() => {
                                                    setOpen(false);
                                                    removeRequest();
                                                    }}>Confirm</ColorButton>
                                                <ColorButton onClick={closeRemoveConfirm} autoFocus>Cancel</ColorButton>
                                            </DialogActions>
                                        </Dialog>
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                    </TableContainer>
                </InfiniteScroll>
            }
        </>
    );
};

