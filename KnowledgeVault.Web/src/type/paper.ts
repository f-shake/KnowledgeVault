export interface paperInt {
    id?: number,
    title: string,
    journal: string,
    firstAuthor?: string,
    correspond?: string,
    year?: number | null,
    executionTime? : string,    // 执行日期
    allAuthors?: string,
    type: number | null, 
    subType: string,
    theme?: string,
    fileID?: string,
    fileExtension?: string,
    isDeleted: boolean,
    number: string,
    amount?: number | null
}
interface SelectInt {
    type?: number | null,
    title: string,
    subType?: string,
    Author: string,
    Correspond: string,
    Year?: number | null,
    PageIndex?: number,
    PageSize?: number
    SortOrder: boolean
    SortField: string
}

export class InitData {
    // 成果总数 
    totalCount = 0
    // 原始论文数据 
    originPaperData: paperInt[] = []
    // 展示 论文- 专利数据
    paperData: paperInt[] = []
    // 插入数据 --- 也是点击编辑的数据
    paperInsert: paperInt = {
        id: 0,
        title: "",
        journal: "", 
        firstAuthor: "",
        correspond: "", 
        year: null,
        executionTime: "",
        allAuthors: "",
        type: null,
        subType: "",
        theme: "",
        fileID: "",
        fileExtension: "",
        isDeleted: false,
        number: "",
        amount: null
    }
    // 搜索数据
    itemSelect: SelectInt = {
        type: null,
        title: "",
        subType: "",
        Author: "",
        Correspond: "",
        PageSize: 50,
        PageIndex: 1,
        SortField: "createTime",
        SortOrder: false
    }
    // 正在加载
    loading = true
}