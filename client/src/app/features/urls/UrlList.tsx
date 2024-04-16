import {urlShorten} from "../../models/urlShorten";
import UrlCard from "./UrlCard";

interface Props {
    urls: urlShorten[];
}


export default function UrlList({urls}: Props) {
    return (
        <>
            {urls.map(url => (
                <UrlCard url={url}/>
            ))}
        </>
    )
}