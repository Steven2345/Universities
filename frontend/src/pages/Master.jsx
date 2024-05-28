
import { ColorButton, syncToServer } from '../Auxiliary.jsx';
import { Universities, Refresher, Buffer, AccessToken } from '../UniversitiesProvider.jsx';
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
import Paper from '@mui/material/Paper';
import { IconButton } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import { PieChart } from '@mui/x-charts/PieChart';


let once = false;

function Master() {
    const { uniList, setUniList } = useContext(Universities);
    const [token, setToken] = useContext(AccessToken);

    // infinite scroll
    const [hasMore, setHasMore] = useState(true);
    const [index, setIndex] = useState(1);

    const fetchMoreData = () => {
        axios.get(`${config.webapi}/${index}`)
            .then((res) => {
                setUniList((prevItems) => [...prevItems, ...res.data]);
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

    // pie chart data
    const interval = 10;
    const uniListSplit = uniList.reduce(function (r, a) {
        var slot = Math.floor((a.score - 0.1) / interval);
        (r[slot] = r[slot] || []).push(a);
        return r;
    }, []);

    // computing maximum score
    const compMax = () => {
        let max = -1;
        uniList.forEach(element => {
            if(element.score > max)
                max = element.score
        });
        return max;
    }

    const initialMax = compMax();
    const [maxim, setMaxim] = useState(initialMax);

    useEffect(() => {
        setMaxim(compMax());
    }, [uniList]);

    // populate tabel
    const { refreshNeeded, setRefreshNeeded } = useContext(Refresher);

    const requestListFromServer = () => {
        if(refreshNeeded) {
            axios.get(`${config.webapi}/`).then((response) => {
                setUniList(response.data);
                setMaxim(compMax());
                setRefreshNeeded(false);
                ////////////////////////////////////
            }).catch((error) => {
                if(error.request) {
                    alert("Connection lost, data could not be fetched");
                }
            })
        }
    }

    useEffect(() => {
        requestListFromServer();
    }, []);

    // websocket
    if(!once) {
        //requestListFromServer();
        once = true;
        const ws = new WebSocket(`ws://${config.webapi}`);

        ws.addEventListener('error', console.error);

        ws.addEventListener('open', function open(event) {
            console.log('opened');
        });

        ws.addEventListener('message', function message(event) {
            console.log('received: %s', event.data);
            setRefreshNeeded(true);
            requestListFromServer();
        });
    }

    // requests to server
    const removeRequest = async () => {
        setBuffer(await syncToServer(buffer));
        axios.delete(`${config.webapi}/delete/${victim}`, {}, {
            headers: {
              'Authorization': 'Bearer ' + token
            }
        }).then(() => {
            setUniList(
                uniList.filter(uni => uni.id !== victim)
            );
        }).catch((error) => {
            if(error.response) {
                if(error.response.status === 401) {
                    alert("You are not the owner of the element you tried to delete");
                }
            }
            else {
                setBuffer([
                    ...buffer,
                    {
                        "type": "uni",
                        "id": victim
                    }
                ]);
                alert("Connection lost, request couldn't be processed");
            }
        });
    }

    const logoutRequest = async () => {
        axios.post(`${config.webapi}/logout`, {}, {
            headers: {
              'Authorization': 'Bearer ' + token
            }
        }).then(() => {
            setToken(null);
            alert("You logged out successfully!");
        }).catch((error) => {
            if(error.response) {
                if(error.response.status === 401) {
                    alert("You are not logged in");
                }
            }
            else if(error.request) {
                alert("Connection lost");
            }
        })
    }

    // return
    return (
        <>
            <Link to="faculties"><ColorButton variant='text'>See faculties</ColorButton></Link>
            { token === null ? 
                <Link to="/login"><ColorButton variant='text'>Log in</ColorButton></Link> :
                <ColorButton onClick={logoutRequest} variant='text'>Log out</ColorButton> 
            }

            <div className='bigTitleCont'><h1>Universities in the USA</h1></div>

            { uniList.length < 1 ?
                <div>No elements</div> : 
                <InfiniteScroll
                    dataLength={uniList.length}
                    next={fetchMoreData}
                    hasMore={hasMore}
                    loader={<h4>Loading...</h4>}
                >
                    <div>
                    <PieChart
                        series={[
                            {
                                data: uniListSplit.map((group, index) => {
                                return {
                                    id: index,
                                    value: group.length,
                                    label: (index * 10).toString() + '-' + ((index + 1) * 10).toString()
                                }
                                }).filter((elem) => {
                                    return elem.value > 0
                                }),
                            },
                        ]}
                        width={500}
                        height={200}
                    />
                    </div>

                    <div>
                        <h2>Maximum score: {maxim}</h2>
                    </div>

                    <div className='generalButtonCont'>
                        <Link to="add"><ColorButton variant='text'>Add university</ColorButton></Link>
                    </div>

                    <TableContainer component={Paper} className='tableCont'>
                    <Table sx={{ minWidth: 950 }} aria-label="simple table">
                        <TableHead>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell align="right">Location</TableCell>
                                <TableCell align="right">Number of faculties</TableCell>
                                <TableCell align="right"></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {uniList.map((university) => (
                                <TableRow
                                    key={university.id}
                                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                >
                                    <TableCell component="th" scope="row">{university.name}</TableCell>
                                    <TableCell align="right">{university.location}</TableCell>
                                    <TableCell align="right">{university.noOfFaculties}</TableCell>
                                    <TableCell align="right">
                                        <Link to={`details/${university.id}`}>
                                            <ColorButton variant="text">Details</ColorButton>
                                        </Link>
                                        <Link to={`edit/${university.id}`}>
                                            <ColorButton variant="text">Edit</ColorButton>
                                        </Link>
                                        <IconButton data-testid={`id${university.id}`} aria-label="delete" 
                                                    onClick={() => openRemoveConfirm(university.id) }>
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
                                                <ColorButton data-testid={`del${university.id}`} onClick={() => {
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

export default Master;
