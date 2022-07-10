import React, { useState, useEffect } from "react"
import Download from "../image/download.png"
import Edit from "../image/edit.png"
import Add from "../image/add.png"
import { useNavigate } from "react-router-dom"
import Header from "./Header"
import {saveAs} from "file-saver"
import Modal from "react-bootstrap/Modal"

function AdminCatalogPage() {
    const navigate = useNavigate();
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const [items, setItems] = useState([]);
    const [search, setSearch] = useState("");
    const [url, setUrl] = useState("");
    const [name, setName] = useState("");
    const [isUpdating, update] = useState(false);
    const SendSearch = (e) => {
        e.preventDefault();
        fetch("https://localhost:7154/images/images?" + new URLSearchParams({filter: search}).toString())
            .then((res) => res.json())
            .then((result) => {
                    setItems(result);
                },
                (error) => {
                    alert("Something went wrong");
                }
            );
    }

    const DownloadImage = (e, item) => {
        e.preventDefault();
        saveAs(item.url, "example.png");
    };

    const EditImage = (e, itemid) => {
        e.preventDefault();
        navigate("/edit/"+ itemid.toString())
    }

    const AddImage = (e) => {
        e.preventDefault();
        handleClose();
        fetch("https://localhost:7154/images/create-image", {
                method: 'POST',
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
                    update(true);
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    alert("Something went wrong");
                }
            );
        
    }

    useEffect(() => {
        fetch("https://localhost:7154/images/images")
            .then((res) => res.json())
            .then(
                (result) => {
                    setItems(result);
                    update(false);
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    alert("Something went wrong");
                }
            );
    }, [isUpdating]);

    return (
        <>
            <Header/>
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Add new image</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form style={{padding: "2rem 1rem"}}>
                        <div className="form-group-add">
                            <label for="url-input" style={{marginBottom: "10px"}}>URL</label>
                            <br/>
                            <input
                                type="text"
                                className="add"
                                id="url-input"
                                placeholder="Enter URL"
                                onChange={e => setUrl(e.target.value)}
                                value={url}
                            />
                        </div>
                        <div className="form-group-add" style={{marginTop: "20px"}}>
                            <label for="name-input" style={{marginBottom: "10px"}}>Name</label>
                            <br/>
                            <input
                                type="text"
                                className="add"
                                id="name-input"
                                placeholder="Name"
                                onChange={e => setName(e.target.value)}
                                value={name}
                            />
                        </div>
                    </form>
                </Modal.Body>
                <Modal.Footer>
                    <button 
                        className="btn btn-danger"
                        onClick={handleClose}
                    >
                        Close
                    </button>
                    <button
                        className="btn btn-success"
                        onClick={AddImage}
                    >
                        Add
                    </button>
                </Modal.Footer>
            </Modal>
            <div className="page">
                <div className="main-container">
                    <form className="search-field">
                        <img
                            style={{marginRight: "10px"}}
                            type="button"
                            src={Add}
                            alt={"Add"}
                            onClick={handleShow}/>
                        <input 
                            className="search"
                            style={{width:"40%"}}
                            type="text"
                            placeholder="Uranus, Sun, Io, etc."
                            onChange={e => setSearch(e.target.value)}
                            value={search}/>
                        <input
                            className="btn btn-dark"
                            style={{marginLeft: "10px"}}
                            type="button"
                            value="Dive in!"
                            onClick={SendSearch}/>
                    </form>
                    <div className="catalog">
                        {items.map((item) => (
                            <div className="card">
                                <img src={item.url} alt={item.name}/>
                                <img className="icon" onClick={e => DownloadImage(e, item)} src={Download} alt={"Download"}/>
                                <img className="icon-edit" onClick={e => EditImage(e, item.id)} src={Edit} alt={"Edit"}/>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </>
    )
}

export default AdminCatalogPage;