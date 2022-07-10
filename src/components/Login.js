import React from "react"
import Form from "./Form"
import Image from "../image/loginPage.jpg"
import { Link } from "react-router-dom"
function Login() {
    return (
        <>
        <div className="login-page">
            <div className="login-page-element" style={{width: "45%"}}>
                <div className="login-form">
                    <span className="form-title">
                        <h2>Enter the Black Hole</h2>
                    </span>
                    <span className="form-title">
                        <h3>Don't have an account yet? <Link className="link"to="/signup">Sign Up</Link></h3>
                    </span>
                    <Form/>
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

export default Login;