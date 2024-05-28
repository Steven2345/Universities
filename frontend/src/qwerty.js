import axios from 'axios';

const current = {
    "name": "uhsvjhsd", 
    "location": "fesziofsezij, sdjs", 
    "score": 87.3, 
    description: "df isfdz sdfzjkzd sfzjk dzsj czx"
}

let message = "hello";
try {
    const response = await axios.post("http://localhost:3000/add", current)
    message = response.status
}
catch(err) {
    message = "Oopsie"
}
console.log(message)

function f() {
    console.log("loop")
    setTimeout(f, 3000)
}
//f()
(async () => setTimeout(() => {}, 3000))()
console.log("did it!")
