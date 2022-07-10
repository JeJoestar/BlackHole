import React, { useState, useEffect } from "react"
import Download from "../image/download.png"
import Header from "./Header"
import {saveAs} from "file-saver"

function CatalogPage() {
    const [items, setItems] = useState([]);
    const [search, setSearch] = useState("");

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

    useEffect(() => {
        fetch("https://localhost:7154/images/images")
            .then((res) => res.json())
            .then(
                (result) => {
                    setItems(result);
                },
                // Note: it's important to handle errors here
                // instead of a catch() block so that we don't swallow
                // exceptions from actual bugs in components.
                (error) => {
                    alert("Something went wrong");
                }
            );
    }, []);

    return (
        <>
            <Header/>
            <div className="page">
                <div className="main-container">
                    <form className="search-field">
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
                                <img className="icon" onClick={e => DownloadImage(e)} src={Download} alt={"Download"}/>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </>
    )
}

export default CatalogPage;