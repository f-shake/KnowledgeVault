import { getPaperList, deleteItemApi } from '@/request/api'
import { ElMessage } from 'element-plus'
import { ElMessageBox } from 'element-plus'
import bus from '@/utils/bus'
import instance from '@/request'
import Cookies from 'js-cookie'
import baseUrl from '../request/url'


// 获取论文列表
export const getPaper = (data: any) => {
  getPaperList(data.itemSelect).then((res: any) => {
    data.paperData = res.items; // 论文展示数据
    data.totalCount = res.totalCount// 获取成果总数
  }).catch((error: any) => {
    console.log(error)
  })
}

// 重置请求参数
export const resetQueryList = (itemSelect: any) => {
  itemSelect.type = null
  itemSelect.title = ""
  itemSelect.subType = ""
  itemSelect.Author = ""
  itemSelect.PageSize = 10
  itemSelect.PageIndex = 1
  itemSelect.SortField = "createTime"
  itemSelect.SortOrder = true
}


// 下载文件
export const downloadItem = (row: any) => {
  const url = baseUrl+'/File/' + row.fileID
  // 使用 window.open() 方法打开文件下载链接
  window.open(url, '_blank');
}




// 点击“编辑”按钮
// export const editItem = (data: any) => {
//   editItemApi(data).then((res) => {
//     console.log(res)
//   }).catch((error) => { 
//     console.log(error)
//   })
// }


// 删除文件
export const deleteItem = (id: number, data: any) => {
  deleteItemApi(id).then((res) => {
    console.log("删除成功")
    getPaper(data)  // 删除后更新数据
  }).catch((error) => {
    console.log(error)
  })
}


// 点击添加成果方法 -- 目的打开弹出框
export const addDataDraw = (dataAll: any) => {
  console.log(dataAll);
  
  // 未选中成果类型弹出警告信息
  if (!dataAll.selectType) {
    ElMessage({
      message: '请选择成果类型!',
      type: 'warning',
      plain: true,
    })
    return;
  }
  dataAll.drawer = true; // 选中后打开遮罩层
  bus.emit("sendAddOrEdit", 1)  // 告诉AddDataBox组件页面进行的是添加操作： 添加为 1， 编辑为2
}

// 编辑论文方法 -- 目的是打开编辑弹出框
export const editItemDraw = (row: any, dataAll: any) => {
  dataAll.selectType = row.type;  // 传入编辑的成果类型
  bus.emit("sendRow", row);  // 将数据传给子组件
  bus.emit("sendAddOrEdit", 2)  // 告诉AddDataBox组件页面进行的是添加操作： 添加为 1， 编辑为2
  dataAll.drawer = true;  // 打开遮罩层
}

// 重置 插入/编辑 表单
export const resetInsertForm = (data: any) => {
  data.paperInsert.id = 0
  data.paperInsert.title = ""
  data.paperInsert.journal = ""
  data.paperInsert.firstAuthor = ""
  data.paperInsert.correspond = ""
  data.paperInsert.year = null
  data.paperInsert.allAuthors = ""
  data.paperInsert.subType = ""
  data.paperInsert.amount = ""
  data.paperInsert.theme = ""
  data.paperInsert.fileID = ""
  data.paperInsert.fileExtension = ""
  data.paperInsert.isDeleted = false
  data.paperInsert.number = ""
}

// 重置表单查询条件为默认值或空值
export const resetSelectForm = (data: any) => {
  data.itemSelect.title = '';
  data.itemSelect.Author = '';
  data.itemSelect.subType = '';
  data.itemSelect.Year = null; // 设置为 null

  getPaper(data); // 重置后查询
};


// 弹出框关闭前询问方法 ? 如果确认关闭清除 信息框信息
export const handleClose = (done: () => void) => {
  ElMessageBox.confirm('确定取消吗? 已操作信息将会丢失!')
    .then(() => {
      bus.emit("onClearDrawData", true)
      done()
    })
    .catch(() => {
      // catch error
    })
}


// Element 表格后端排序
export const sortTableFun = (column: any, data: any) => {
  data.itemSelect.SortField = column.prop;    // 排序字段
  if (column.order === "ascending") {   // 正序
    data.itemSelect.SortOrder = true
  } else if (column.order === "descending") {   // 降序
    data.itemSelect.SortOrder = false
  } else {    // 不排序  column.order === null
    data.itemSelect.SortField = "createTime";
    data.itemSelect.SortOrder = false;
  }
  getPaper(data)
}


// 判断身份状态是 管理员 还是 游客
export const identityDeter = (dataAll: any) => {
  dataAll.identityID = Cookies.get("token") != null
}

// 自定义索引
export const indexMethod = (page: number | undefined, pageSize: number | undefined) => {
  if (page && pageSize) {
    return (page - 1) * pageSize + 1
  }
}
