import React from "react"
import FormSignup from "./FormSignup"
import Image from "../image/loginPage.jpg"
import { Link } from "react-router-dom"
function Signup() {
    return (
        <>
        <div className="login-page">
            <div className="login-page-element" style={{width: "45%"}}>
                <div className="login-form" style={{padding: "15% 3rem"}}>
                    <span className="form-title">
                        <h2>Enter the Black Hole</h2>
                    </span>
                    <span className="form-title">
                        <h3>Already have an account? <Link className="link"to="/login">Log In</Link></h3>
                    </span>
                    <FormSignup/>
                </div>
            </div>
            <div className="login-page-element">
                <img className="login-page-image" src={Image} alt={""}/>
                <span className="image-title">
                    <h1>
                        <span className="black-part">Black.</span>
                        <span className="white-part">hole</span>
                    </h1>
                </span>
            </div>
        </div>
        </>
    )
}

export default Signup;