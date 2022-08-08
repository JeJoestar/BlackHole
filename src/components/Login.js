import React from "react"
import Form from "./Form"
import Image from "../image/loginPage.jpg"
import { Link } from "react-router-dom"
import TwitterLogin from "../components/TwitterLogin"
import GoogleLogin from "react-google-login"

/*
 * Create form to request access token from Google's OAuth 2.0 server.
 */
function oauthSignIn() {
    // Google's OAuth 2.0 endpoint for requesting an access token
    var oauth2Endpoint = 'https://accounts.google.com/o/oauth2/v2/auth';
  
    // Create <form> element to submit parameters to OAuth 2.0 endpoint.
    var form = document.createElement('form');
    form.setAttribute('method', 'GET'); // Send as a GET request.
    form.setAttribute('action', oauth2Endpoint);
  
    // Parameters to pass to OAuth 2.0 endpoint.
    var params = {'client_id': '586487161634-82r4jahsdkjq21svjl3k9tlii6842h6k.apps.googleusercontent.com',
                  'redirect_uri': 'https://localhost:7154/users/google',
                  'response_type': 'token',
                  'scope': 'https://www.googleapis.com/auth/userinfo.profile',
                  'include_granted_scopes': 'true',
                  'state': 'pass-through value'};
  
    // Add form parameters as hidden input values.
    for (var p in params) {
      var input = document.createElement('input');
      input.setAttribute('type', 'hidden');
      input.setAttribute('name', p);
      input.setAttribute('value', params[p]);
      form.appendChild(input);
    }
  
    // Add form to page and submit it to open the OAuth 2.0 endpoint.
    document.body.appendChild(form);
    form.submit();
  }

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
                    <button onClick={oauthSignIn}></button>
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