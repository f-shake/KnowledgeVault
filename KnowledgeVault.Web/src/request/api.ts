import instance from './index'
import { LoginFormInt } from "@/type/login"
import { paperInt } from '@/type/paper'

export function login(data: LoginFormInt) {
    return instance({
        url: "/login",
        method: "POST",
        data
    })
}

// 验证身份
export function verifyIdentity(params: any) {
    return instance({
        url: `/User/${params}`,
        method: "GET",
    })
}


// 获取论文列表
export function getPaperList(params: any) {
    return instance({
        url: "/Achievement/List",
        method: "GET",
        params
    })
}

// 获取论文列表
export function exportTable(params: any) {
    return instance({
        url: "/Archive/ExportTable",
        method: "GET",
        params
    })
}



// 插入数据
export function insertPaper(data: any) {
    return instance({
        url: "/Achievement/Insert",
        method: "POST",
        data 
    })
}

// 编辑数据
export function editItemApi(data: any) {
    return instance({
        url: "/Achievement/Update",
        method: "POST",
        data
    })
}

// 删除数据
export function deleteItemApi(id: number) {
    return instance({
        url: `/Achievement/Delete/${id}`,
        method: "POST",
    })
} 

