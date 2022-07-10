import React, { useState } from "react"
import { Link, useNavigate } from "react-router-dom"

export default function FormSignup(){
    const navigate = useNavigate()
    const [mail, setMail] = useState("");
    const [password, setPass] = useState("");
    const [rePassword, setRePass] = useState("");

    const validation = (event) => {
        event.preventDefault();
        let item = {
            mail: mail,
            password: password,
            repeatPassword: rePassword
        }
        fetch(`https://localhost:7154/users/sign-up`, {
            method: 'POST',
            headers: {
                'Content-type': 'application/json',
            },
            body: JSON.stringify(item)
            })
            .then(response => {
                if(!response.ok) {
                    return response.text().then(text => { throw new Error(text) })
                }
                else
                {
                    return response.json();
                }
            })
            .then(data => {
                navigate("/login")
            })
            .catch(error => {
                alert(error);
            });
    }
    return (
            <form
                style={{padding: "2rem 1rem"}}
                onSubmit={validation}
            >
                <div className="form-group">
                    <label for="email-input" style={{marginBottom: "10px"}}>Email address</label>
                    <input
                        type="email"
                        className="form-control"
                        id="email-input"
                        placeholder="Enter email"
                        onChange={e => setMail(e.target.value)}
                        value={mail}/>
                </div>
                <div className="form-group" style={{marginTop: "20px"}}>
                    <label for="password-input" style={{marginBottom: "10px"}}>Password</label>
                    <input
                        type="password"
                        className="form-control"
                        id="password-input"
                        placeholder="Password"
                        onChange={e => setPass(e.target.value)}
                        value={password}/>
                </div>
                <div className="form-group" style={{marginTop: "20px"}}>
                    <label for="repeat-password-input" style={{marginBottom: "10px"}}>Repeat Password</label>
                    <input
                        type="password"
                        className="form-control"
                        id="repeat-password-input"
                        placeholder="Repeat password"
                        onChange={e => setRePass(e.target.value)}
                        value={rePassword}/>
                </div>
                <div className="form-group" style={{marginTop: "50px"}}>
                    <button type="submit" className="btn btn-primary" style={{width: "100%"}}>Sign Up</button>
                </div>
            </form>
    )
}