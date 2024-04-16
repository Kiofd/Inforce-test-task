import { urlShorten } from "../../models/urlShorten";

interface Props{
    url: urlShorten
}

const styles = {
    card: {
        border: '1px solid #ccc',
        borderRadius: '8px',
        padding: '16px',
        marginBottom: '16px',
        boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
        backgroundColor: '#f9f9f9',
    },
};

export default function UrlCard({url} : Props){
    return (
        <div style={styles.card}>
            <h3>Short URL:</h3>
            <p>{url.shortUrl}</p>
            <h3>Original URL:</h3>
            <p>{url.longUrl}</p>
            <h3>Created Date:</h3>
            <p>{new Date(url.createdData).toLocaleString()}</p>
            <h3>Created By:</h3>
            <p>{url.createdBy}</p>
        </div>
    )
}