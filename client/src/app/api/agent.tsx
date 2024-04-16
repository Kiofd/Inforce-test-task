import axios, { AxiosResponse } from "axios";

axios.defaults.baseURL = 'http://localhost:5000/api/'
axios.defaults.withCredentials = true;

const responseBody = (response: AxiosResponse) => response.data;

// axios.interceptors.request.use((config) => {
//     const token = store.getState()
// })
const request = {
    get: (url:string) => axios.get(url).then(responseBody),
    post: (url:string, body:{}) => axios.post(url, body).then(responseBody),
    delete: (url:string) => axios.delete(url).then(responseBody),
}

const Urls ={
    list: () => request.get("shorten/getAllUrls")
}

const agent ={
    Urls
}

export default agent