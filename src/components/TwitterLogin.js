import React, { Component } from "react";
import PropTypes from "prop-types";
import { Link, useNavigate } from "react-router-dom"




function TwitterLogin(){
  const navigate = useNavigate()

  const getRequestToken = () => {
    fetch(`https://localhost:7154/users/twitter-request`, {
              method: 'POST',
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
                  navigate("https://api.twitter.com/oauth/authenticate?" + data.oauth_token)
              })
              .catch(error => {
                  alert(error);
              });
  }
  
  return (
    <>
    
    </>
  )
}