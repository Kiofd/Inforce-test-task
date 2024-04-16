import {useNavigate} from "react-router-dom"
import Catalog from "../features/urls/Catalog"

function App() {

    const navigate = useNavigate()

    return (
        <>
            <Catalog/>
        </>
    )
}


export default App
