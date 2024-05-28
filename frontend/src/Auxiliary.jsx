import { Button } from "@mui/material";
import { yellow } from "@mui/material/colors";
import { styled } from '@mui/material/styles';
import './App.css';
import axios from 'axios';
import * as config from './appconfig.json';



export const ColorButton = styled(Button)(({ theme }) => ({
  margin: '3px',
  color: '#ffffff',
  backgroundColor: yellow[700],
  '&:hover': {
    backgroundColor: yellow[900],
  },
}));


let nextId = 100;
export function createUni(name, location, score, description) {
    nextId++;
    return { id: nextId, name, location, score, description };
}

let nextIdF = 100;
export function createFacult(name, noOfStudents, university) {
    nextIdF++;
    return { id: nextIdF, name, noOfStudents, university };
}

export async function syncToServer(buffer) {
  console.log("in sync");
  console.log(buffer);
  while(buffer.length > 0) {
    console.log("sync time!");
    console.log(buffer);
    let current = buffer.shift();
    if(current.type === "uni") {
      delete current.type;
      if(Object.hasOwn(current, "name") && !Object.hasOwn(current, "id")) {
        // it's an add
        try {
          const response = await axios.post(`${config.webapi}/add`, current);
          if(response.status !== 200) {
            current.type = "uni";
            buffer.unshift(current);
            break;
          }
        }
        catch (err) {
          current.type = "uni";
          buffer.unshift(current);
          break;
        }
      }
      else if(Object.hasOwn(current, "name") && Object.hasOwn(current, "id")) {
        // it's an edit
        try {
          console.log(current);
          const response = await axios.put(`${config.webapi}/edit/${current.id}`, current);
          console.log(response.status);
          if(response.status !== 200) {
            console.log(response.status);
            current.type = "uni";
            buffer.unshift(current);
            break;
          }
        }
        catch (err) { 
          current.type = "uni";
          buffer.unshift(current);
          break; 
        }
      }
      else if(!Object.hasOwn(current, "name") && Object.hasOwn(current, "id")) {
        // it's a delete
        try {
          const response = await axios.delete(`${config.webapi}/delete/${current.id}`);
          if(response.status !== 200) {
            current.type = "uni";
            buffer.unshift(current);
            break;
          }
        }
        catch (err) {
          current.type = "uni";
          buffer.unshift(current);
          break;
        }
      }
      else {
        console.log("what the freak are you doin'??")
      }
    }
    else if(current.type === "facult") {
      delete current.type;
      if(Object.hasOwn(current, "name") && !Object.hasOwn(current, "id")) {
        // it's an add
        try {
          const response = await axios.post(`${config.webapi}/faculties/add`, current);
          if(response.status !== 200) {
            current.type = "facult";
            buffer.unshift(current);
            break;
          }
        }
        catch (err) {
          current.type = "facult";
          buffer.unshift(current);
          break;
        }
      }
      else if(Object.hasOwn(current, "name") && Object.hasOwn(current, "id")) {
        // it's an edit
        try {
          const response = await axios.put(`${config.webapi}/faculties/edit/${current.id}`, current);
          if(response.status !== 200) {
            current.type = "facult";
            buffer.unshift(current);
            break;
          }
        }
        catch (err) { 
          current.type = "facult";
          buffer.unshift(current);
          break; 
        }
      }
      else if(!Object.hasOwn(current, "name") && Object.hasOwn(current, "id")) {
        // it's a delete
        try {
          const response = await axios.delete(`${config.webapi}/faculties/delete/${current.id}`);
          if(response.status !== 200) {
            current.type = "facult";
            buffer.unshift(current);
            break;
          }
        }
        catch (err) {
          current.type = "facult";
          buffer.unshift(current);
          break;
        }
      }
      else {
        console.log("what the freak are you doin'??")
      }
    }
  }
  return buffer;
}

export function syncToServer2(buffer) {
  let valid = true;
  let current = buffer.shift();
  if(Object.hasOwn(current, "name") && !Object.hasOwn(current, "id")) {
    // it's an add
    axios.post(`${config.webapi}/add`, current)
      .then(() => {
        return syncToServer(buffer);
      })
      .catch((error) => {
        if(error.request) {
          setBuffer([
            current, 
            ...buffer
          ]);
        }
    });
  }
  else if(Object.hasOwn(current, "name") && Object.hasOwn(current, "id")) {
    // it's an edit
  }
  else if(!Object.hasOwn(current, "name") && Object.hasOwn(current, "id")) {
    // it's a delete
  }
  else {
    console.log("what the freak are you doin'??")
  }
}


export function popupConfirm() {

}

