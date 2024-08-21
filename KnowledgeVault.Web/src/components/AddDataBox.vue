<template>
    <!-- ----------------------------------添加论文---------------------------------- -->
    <el-form :model="paperInsert" label-width="auto" style="max-width: 600px" v-if="activeType == 1">
        <el-form-item label="论文名称" required>
            <el-input v-model="paperInsert.title" />
        </el-form-item>
        <el-form-item label="论文类型" required>
            <el-select v-model="paperInsert.subType" placeholder="请选择论文类型">
                <el-option label="期刊论文" value="期刊论文" />
                <el-option label="学位论文" value="学位论文" />
            </el-select>
        </el-form-item>
        <transition name="fade">
            <el-form-item label="期刊" required v-show="paperType.journalShow" class="fade-item">
                <el-input v-model="paperInsert.journal" />
            </el-form-item>
        </transition>
        <transition name="fade">
            <el-form-item label="学位" required v-show="paperType.degreeShow" class="fade-item">
                <el-radio-group v-model="paperInsert.journal">
                    <el-radio value="博士论文">博士论文</el-radio>
                    <el-radio value="硕士论文">硕士论文</el-radio>
                </el-radio-group>
            </el-form-item>
        </transition>
        <el-form-item label="负责老师" required>
            <el-input v-model="paperInsert.correspond" />
        </el-form-item>
        <el-form-item label="负责学生" required>
            <el-input v-model="paperInsert.firstAuthor" />
        </el-form-item>
        <el-form-item label="全部作者">
            <el-input v-model="paperInsert.allAuthors" />
        </el-form-item>
        <el-form-item label="发表年份" required>
            <el-input v-model="paperInsert.year" type="number" />
        </el-form-item>
        <el-form-item label="领域">
            <el-input v-model="paperInsert.theme" />
        </el-form-item>
        <el-form-item label="文件">
            <el-upload ref="uploadRef" :on-success="uploadSuccess" :file-list="fileList" class="upload-demo"
                :on-remove="removeUpload" action="http://autodotua.top:12336/api/File"
                :headers="{ 'Authorization': '123456' }">
                <template #trigger>
                    <el-button type="primary" size="small">选择文件</el-button>
                </template>
            </el-upload>
        </el-form-item>
    </el-form>
    <!-- ----------------------------------添加专利---------------------------------- -->
    <el-form :model="paperInsert" label-width="auto" style="max-width: 600px" v-else-if="activeType == 2">
        <el-form-item label="专利名称" required>
            <el-input v-model="paperInsert.title" />
        </el-form-item>
        <el-form-item label="类型" required>
            <el-radio-group v-model="paperInsert.subType">
                <el-radio value="发明专利">发明专利</el-radio>
                <el-radio value="实用新型专利">实用新型专利</el-radio>
                <el-radio value="外观设计专利">外观设计专利</el-radio>
            </el-radio-group>
        </el-form-item>
        <el-form-item label="负责老师">
            <el-input v-model="paperInsert.correspond" />
        </el-form-item>
        <el-form-item label="负责学生">
            <el-input v-model="paperInsert.firstAuthor" />
        </el-form-item>
        <el-form-item label="全部作者">
            <el-input v-model="paperInsert.allAuthors" />
        </el-form-item>
        <el-form-item label="发表年份" required>
            <el-input v-model="paperInsert.year" type="number" />
        </el-form-item>
        <el-form-item label="专利号" required>
            <el-input v-model="paperInsert.number" />
        </el-form-item>
        <el-form-item label="领域">
            <el-input v-model="paperInsert.theme" />
        </el-form-item>
        <el-form-item label="文件">
            <el-upload ref="uploadRef" :on-success="uploadSuccess" class="upload-demo"
                action="http://autodotua.top:12336/api/File" :headers="{ 'Authorization': '123456' }">
                <template #trigger>
                    <el-button type="primary" size="small">选择文件</el-button>
                </template>
            </el-upload>
        </el-form-item>
    </el-form>
    <!-- ----------------------------------添加软著---------------------------------- -->
    <el-form :model="paperInsert" label-width="auto" style="max-width: 600px" v-else-if="activeType == 3">
        <el-form-item label="软著名称" required>
            <el-input v-model="paperInsert.title" />
        </el-form-item>
        <el-form-item label="负责老师">
            <el-input v-model="paperInsert.correspond" />
        </el-form-item>
        <el-form-item label="负责学生">
            <el-input v-model="paperInsert.firstAuthor" />
        </el-form-item>
        <el-form-item label="全部作者">
            <el-input v-model="paperInsert.allAuthors" />
        </el-form-item>
        <el-form-item label="发表年份" required>
            <el-input v-model="paperInsert.year" type="number" />
        </el-form-item>
        <el-form-item label="申请号" required>
            <el-input v-model="paperInsert.number" />
        </el-form-item>
        <el-form-item label="领域">
            <el-input v-model="paperInsert.theme" />
        </el-form-item>
        <el-form-item label="文件">
            <el-upload ref="uploadRef" :on-success="uploadSuccess" class="upload-demo"
                action="http://autodotua.top:12336/api/File" :headers="{ 'Authorization': '123456' }">
                <template #trigger>
                    <el-button type="primary" size="small">选择文件</el-button>
                </template>
            </el-upload>
        </el-form-item>
    </el-form>
    <!-- ----------------------------------添加奖项---------------------------------- -->
    <el-form :model="paperInsert" label-width="auto" style="max-width: 600px" v-else-if="activeType == 4">
        <el-form-item label="奖项名称" required>
            <el-input v-model="paperInsert.title" />
        </el-form-item>
        <el-form-item label="老师">
            <el-input v-model="paperInsert.correspond" />
        </el-form-item>
        <el-form-item label="老师">
            <el-input v-model="paperInsert.firstAuthor" />
        </el-form-item>
        <el-form-item label="获奖年份" required>
            <el-input v-model="paperInsert.year" type="number" />
        </el-form-item>
        <el-form-item label="主题">
            <el-input v-model="paperInsert.theme" />
        </el-form-item>
        <el-form-item label="文件">
            <el-upload ref="uploadRef" :on-success="uploadSuccess" class="upload-demo"
                action="http://autodotua.top:12336/api/File" :headers="{ 'Authorization': '123456' }">
                <template #trigger>
                    <el-button type="primary" size="small">选择文件</el-button>
                </template>
            </el-upload>
        </el-form-item>
    </el-form>
    <!-- ----------------------------------添加项目---------------------------------- -->
    <el-form :model="paperInsert" label-width="auto" style="max-width: 600px" v-else-if="activeType == 5">
        <el-form-item label="项目名称" required>
            <el-input v-model="paperInsert.title" />
        </el-form-item>
        <el-form-item label="类别" required>
            <el-radio-group v-model="paperInsert.subType">
                <el-radio value="国家级">国家级</el-radio>
                <el-radio value="省部级">省部级</el-radio>
                <el-radio value="厅市级及以下">厅市级及以下</el-radio>
                <el-radio value="横向">横向</el-radio>
            </el-radio-group>
        </el-form-item>
        <el-form-item label="类别" required>
            <el-input v-model="paperInsert.journal" placeholder="如基金委面上、青年等" />
        </el-form-item>
        <el-form-item label="负责老师" required>
            <el-input v-model="paperInsert.correspond" />
        </el-form-item>
        <!-- <el-form-item label="负责学生" required>
            <el-input v-model="paperInsert.firstAuthor" />
        </el-form-item> -->
        <el-form-item label="所有负责人">
            <el-input v-model="paperInsert.allAuthors" />
        </el-form-item>
        <el-form-item label="获批年份" required>
            <el-input v-model="paperInsert.year" type="number" />
        </el-form-item>
        <el-form-item label="执行日期" required>
            <el-input v-model="paperInsert.executionTime" placeholder="格式要求如：2024-05-01"/>
        </el-form-item>
        <el-form-item label="金额（万元)" required>
            <el-input v-model="paperInsert.amount" />
        </el-form-item>
        <el-form-item label="项目号" required>
            <el-input v-model="paperInsert.number"/>
        </el-form-item>
        <el-form-item label="备注">
            <el-input v-model="paperInsert.theme" />
        </el-form-item>
        <el-form-item label="文件">
            <el-upload ref="uploadRef" :on-success="uploadSuccess" class="upload-demo"
                action="http://autodotua.top:12336/api/File" :headers="{ 'Authorization': '123456' }">
                <template #trigger>
                    <el-button type="primary" size="small">选择文件</el-button>
                </template>
            </el-upload>
        </el-form-item>
    </el-form>

    <el-button type="primary" @click="onSubmit">提交</el-button>
    <el-button @click="resetInsertForm(data)">重置</el-button>

</template>

<script lang="ts">
import { defineComponent, reactive, ref, toRefs, watch } from 'vue'
import type { UploadInstance } from 'element-plus'
import { InitData } from '@/type/paper'
import { insertPaper, editItemApi } from '@/request/api'
import { ElMessage } from 'element-plus'
import { resetInsertForm } from '@/utils/commonFunctions'
import bus from '@/utils/bus'



// 实例化数据
const data = reactive(new InitData())
let handleType = ref(0);  // 判断是添加页面还是编辑页面 添加为 1， 编辑为2
const fileList = ref([]);   // 上传数据列表

// 接收点击编辑的数据
bus.on('sendRow', (row) => {
    data.paperInsert = { ...data.paperInsert, ...row }  // 将数据赋值给编辑项
    console.log(data.paperInsert)
})

// 判断(handleType)是添加页面还是编辑页面 添加为 1， 编辑为2
bus.on('sendAddOrEdit', (pageType) => {
    handleType.value = pageType
})
bus.on('onClearDrawData', (res) => {
    resetInsertForm(data)   // 重置表单
    fileList.value = []  // 清除上传文件
})

export default defineComponent({
    props: {
        activeType: Number,
    },
    setup(props, ctx) {
        const uploadRef = ref<UploadInstance>(); // 上传文件

        // 定义论文类型状态
        const paperType = reactive({
            journalShow: false,
            degreeShow: false
        })
        // 监听论文类型
        watch(() => data.paperInsert.subType, (newVal, oldVal) => {
            if (newVal === "期刊论文") {
                paperType.degreeShow = false;
                paperType.journalShow = true;
                if (handleType.value == 1) {   // 添加操作
                    data.paperInsert.journal = "";
                }
            } else if (newVal === "学位论文") {
                paperType.journalShow = false;
                paperType.degreeShow = true;
                if (handleType.value == 1) {   // 添加操作
                    data.paperInsert.journal = "";
                }
            } else {
                paperType.degreeShow = false;
                paperType.journalShow = false;
                if (handleType.value == 1) {   // 添加操作
                    data.paperInsert.journal = "";
                }
            }
        },
            {
                immediate: true
            })

        // 上传文件成功后的回调
        const uploadSuccess = (response: any) => {
            data.paperInsert.fileID = response.fileID;
            data.paperInsert.fileExtension = response.extension
        }
        // 移除文件时的回调
        const removeUpload = (file: any, fileList: any) => {
            data.paperInsert.fileID = "";
            data.paperInsert.fileExtension = ""
        }
        // 确定提交 -- 添加 / 编辑
        const onSubmit = () => {
            // 判断(handleType)是添加页面还是编辑页面 添加为 1 ， 编辑为2
            if (handleType.value == 1) {   // 添加操作
                data.paperInsert.type = Number(props.activeType)
                // 如果添加的不是 项目， 将 amount 字段改为0
                if (data.paperInsert.type != 5) {
                    data.paperInsert.amount = 0
                }
                // // 判断是否上传了文件
                // if (!data.paperInsert.fileID) {
                //     ElMessage.info("请上传文件");
                //     return
                // }
                insertPaper(data.paperInsert).then((res) => {
                    if (typeof res === 'number') {  // 添加成功
                        ElMessage({
                            message: '添加成功!',
                            type: 'success',
                            plain: true,
                        })
                        // 通知父组件 AllView 组件刷新页面,并关闭遮罩层 drawer = false，第一个参数为刷新，第二个为关闭
                        ctx.emit("toParentHandle", false)
                        resetInsertForm(data) // 重置表单
                        fileList.value = []  // 清除上传文件
                    } else {
                        ElMessage.error('添加失败！')
                    }
                }).catch((error) => {
                    ElMessage({
                        message: '添加失败!',
                        type: 'error',
                        plain: true,
                    })
                    console.log(error)
                })
            } else if (handleType.value === 2) {  // 编辑操作
                if (!data.paperInsert.fileID) {
                    ElMessage.info("请上传文件");
                    return
                }
                editItemApi(data.paperInsert).then((res) => {
                    if (typeof res === "string") {    // 编辑成功
                        ctx.emit("toParentHandle", false)   // 通知父组件 AllView 组件刷新页面,并关闭遮罩层 drawer = false
                        resetInsertForm(data) // 重置表单
                        ElMessage.success("编辑成功")
                        fileList.value = []  // 清除上传文件
                    } else {
                        ElMessage.error("编辑失败！")
                    }

                }).catch((error) => {
                    console.log(error)
                })
            }
        }

        // 将方法暴露给父组件
        return { fileList, data, ...toRefs(data), onSubmit, uploadRef, uploadSuccess, removeUpload, paperType, resetInsertForm }
    }
})
</script>

<style lang="scss" scoped>
/* 定义过渡效果 */
.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.3s ease;
}

.fade-enter,
.fade-leave-to {
    opacity: 0;
    /* 初始状态为透明 */
}
</style>