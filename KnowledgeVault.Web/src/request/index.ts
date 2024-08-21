import axios from 'axios'
import { ElMessage } from 'element-plus'

const baseUrl = 'http://autodotua.top:12336/api';

// 创建instance实例
const instance = axios.create({
    baseURL: baseUrl,
    timeout: 5000,
    headers: {
        "Content-Type": "application/json",
        'Cache-Control': 'max-age=3600'
    }
})

// 请求拦截
instance.interceptors.request.use((config) => {
    if(localStorage.NbuAchievementManagementSystemAdministrator && localStorage.NbuAchievementManagementSystemAdministrator){
        config.headers.Authorization = "123456"
    }
    return config
})


// 响应拦截
instance.interceptors.response.use((res) => {
    // if (res.status !== 200) {
    //     return false
    // }
    return res.data

}, (error) => {
    console.log(error)
    switch (error.response.status) {
        case 400:
            ElMessage.error(error.response.data.title)
            break
        case 401:
            ElMessage.error(error.response.data)
            break
        case 409:
            ElMessage.error(error.response.data)
            break
        default:
            console.log('未知错误')
    }
})

export default instance;