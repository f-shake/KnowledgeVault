import axios from 'axios'
import { ElMessage } from 'element-plus'
import baseUrl from './url'
import Cookies from 'js-cookie'

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
    const token=Cookies.get("token")
    console.log(token)
    if(token){
        config.headers.Authorization = token
    }
    return config
})


// 响应拦截
instance.interceptors.response.use(
    (res) => {
        // 如果状态码是2xx，直接返回数据
        if (res.status === 200) {
            return res.data;
        } else {
            // 如果状态码不是200，直接抛出错误
            return Promise.reject(new Error('Request failed with status ' + res.status));
        }
    },
    (error) => {
        // 处理具体的错误
        if (error.response) {
            switch (error.response.status) {
                case 400:
                    ElMessage.error(error.response.data.title || 'Bad Request');
                    break;
                case 401:
                    ElMessage.error(error.response.data || 'Unauthorized');
                    break;
                case 409:
                    ElMessage.error(error.response.data || 'Conflict');
                    break;
                default:
                    ElMessage.error('Unknown error');
            }
        } else {
            ElMessage.error('Network Error');
        }
        return Promise.reject(error); // 确保错误进入 `catch`
    }
);

export default instance;