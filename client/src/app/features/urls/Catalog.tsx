import {useEffect, useState } from "react";
import { urlShorten } from "../../models/urlShorten";
import UrlList from "./UrlList";

export default function Catalog(){
    const [urls, setUrls] = useState<urlShorten[]>([])

    useEffect(() => {
        fetch('http://localhost:5000/api/shorten/getAllUrls')
            .then(response => response.json())
            .then(data => setUrls(data))
    }, []);
    
    return(
        <>
            <UrlList urls = {urls}/>
        </>
    )
}