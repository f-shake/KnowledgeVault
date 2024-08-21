import mitt from 'mitt'

type Events = {
    onClearDrawData: boolean    // 清空 插入/编辑 表单数据
    sendRow: object // 传递编辑的表单选项
    sendAddOrEdit: number   // 告诉AddDataBox组件页面进行的是添加操作： 添加为 1， 编辑为2
    onRoutRefresh: any  // 路由切换重新请求数据
}

const bus = mitt<Events>()
export default bus 