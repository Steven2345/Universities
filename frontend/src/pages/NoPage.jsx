import { Link } from "react-router-dom";
import { ColorButton } from "../Auxiliary";

export default function NoPage() {
    return (
        <>
            <h1>Oops!...</h1>
            <h3>We're sorry, but the page you are looking for doesn't exist</h3>
            <div className='backButtonCont'>
                <Link to="/"><ColorButton variant='text'>Back</ColorButton></Link>
            </div>
        </>
    );
}
