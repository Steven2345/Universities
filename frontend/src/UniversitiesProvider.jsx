
import { createContext, useState } from 'react';
  

export const Universities = createContext(null);
export const Refresher = createContext(null);
export const Paging = createContext(null);
export const RowsPerPage = createContext(null);
export const Buffer = createContext(null);
export const Faculties = createContext(null);
export const UniNames = createContext(null);
export const AccessToken = createContext(null);

export default function UniversitiesProvider({ children }) {
    let initialUniList = [];
    const [uniList, setUniList] = useState(initialUniList);
    const [facultList, setFacultList] = useState([]);
    const [uniNamesList, setUniNamesList] = useState([]);

    const [refreshNeeded, setRefreshNeeded] = useState(true);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);
    const [buffer, setBuffer] = useState([]);
    const [token, setToken] = useState(null);

    return (
        <Universities.Provider value={{ uniList, setUniList }}>
            <Faculties.Provider value={{ facultList, setFacultList }}>
                <UniNames.Provider value={{ uniNamesList, setUniNamesList }}>
                    <Refresher.Provider value={{ refreshNeeded, setRefreshNeeded }}>
                        <Paging.Provider value={ [page, setPage] }>
                            <RowsPerPage.Provider value={ [rowsPerPage, setRowsPerPage] }>
                                <Buffer.Provider value={ [buffer, setBuffer] }>
                                    <AccessToken.Provider value={ [token, setToken] }>
                                        {children}
                                    </AccessToken.Provider>
                                </Buffer.Provider>
                            </RowsPerPage.Provider>
                        </Paging.Provider>
                    </Refresher.Provider>
                </UniNames.Provider>
            </Faculties.Provider>
        </Universities.Provider>
  )
}


