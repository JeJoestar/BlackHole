import React, { useEffect, useState } from "react"
import { useParams, useNavigate } from "react-router-dom";
import Header from "./Header";

function Edit() {
    let {id} = useParams();
    const navigate = useNavigate();
    const [url, setUrl] = useState("");
    const [name, setName] = useState("");
    useEffect(() => {
        if(url === ""){
            fetch("https://localhost:7154/images/image?" + new URLSearchParams({id: id}).toString())
            .then((res) => res.json())
            .then(
                (result) => {
                    setUrl(result.url);
                    setName(result.name);
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    alert("Something went wrong");
                }
            );
        }
        
    })

    const editImage = e => {
        e.preventDefault();
            fetch("https://localhost:7154/images/update?" + new URLSearchParams({id: id}).toString(), {
                method: 'PUT',
                headers: {
                    'Content-type': 'application/json',
                },
                body: JSON.stringify({
                    url: url,
                    name: name
                })
            })
            .then((res) => res.json())
            .then(
                (result) => {
                    setUrl(result.url);
                    setName(result.name);
                    alert("Successfuly edited.")
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    alert("Something went wrong");
                }
            );

    }
    const deleteImage = e => {
        e.preventDefault();
        fetch("https://localhost:7154/images/delete?" + new URLSearchParams({id: id}).toString(), {
                method: 'DELETE'
            })
            .then(
                (result) => {
                    navigate("/admin");
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    alert("Something went wrong");
                }
            );
    }
    return (
        <>
            <Header/>
            <div className="edit-page">
                <div className="card">
                    <img
                        src={url}
                        alt={name}
                    />
                </div>
                <div>
                    <form
                        style={{padding: "2rem 1rem"}}
                        onSubmit={editImage}
                        onReset={deleteImage}
                        action={"/catalog"}>
                        <div className="form-group-edit">
                            <label for="url-input" style={{marginBottom: "10px"}}>URL</label>
                            <br/>
                            <input
                                type="text"
                                className="edit"
                                id="url-input"
                                placeholder="Enter URL"
                                onChange={e => setUrl(e.target.value)}
                                value={url}/>
                        </div>
                        <div className="form-group-edit" style={{marginTop: "20px"}}>
                            <label for="name-input" style={{marginBottom: "10px"}}>Name</label>
                            <br/>
                            <input
                                type="text"
                                className="edit"
                                id="name-input"
                                placeholder="Name"
                                onChange={e => setName(e.target.value)}
                                value={name}/>
                        </div>
                        <div className="controls">
                            <div style={{marginTop: "50px", marginRight:"5px"}}>
                                <button type="submit" className="btn btn-success" style={{width: "300px"}}>Edit</button>
                            </div>
                            <div style={{marginTop: "50px"}}>
                                <button type="reset" className="btn btn-danger" style={{width: "300px"}}>Delete</button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </>
    )
}

export default Edit;