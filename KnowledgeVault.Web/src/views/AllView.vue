<template>
    <div style="display: flex; height:100%; flex-direction: column;">
        <div class="addSearch">
            <!-- --------------搜索页面-------------- -->
            <div class="search">
                <el-form :inline="true" v-model="itemSelect" class="demo-form-inline" @keydown.enter="getPaper(data)">
                    <el-form-item label="标题">
                        <el-input v-model="itemSelect.title" placeholder="请输入关键字" maxlength clearable
                            style="max-width: 160px;" />
                    </el-form-item>
                    <el-form-item label="老师">
                        <el-input v-model="itemSelect.Correspond" placeholder="请输入" clearable style="width: 100px;" />
                    </el-form-item>
                    <el-form-item label="学生">
                        <el-input v-model="itemSelect.Author" placeholder="请输入" clearable style="width: 100px;" />
                    </el-form-item>
                    <el-form-item label="年份">
                        <el-input v-model="itemSelect.Year" placeholder="请输入年份" type="number" clearable
                            style="width: 120px;" />
                    </el-form-item>
                    <el-form-item>
                        <el-button type="primary" @click="getPaper(data)">查询</el-button>
                        <el-button @click="resetSelectForm(data)">重置</el-button>
                    </el-form-item>
                </el-form>
            </div>
            <!-- --------------添加成果-------------- -->
            <div class="add" v-if="dataAll.identityID">
                <el-form :inline="true" class="demo-form-inline">
                    <el-form-item>
                        <el-button type="primary" @click="addDataDraw(dataAll)">添加成果</el-button>
                    </el-form-item>
                </el-form>
                <el-select v-model="selectType" placeholder="请选择成果类型" style="width: 160px; margin-right: 20px;">
                    <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value" />
                </el-select>
            </div>
        </div>
        <!-- --------------分页-------------- -->
        <el-pagination small background layout="prev, pager, next" v-model:current-page="itemSelect.PageIndex"
            :page-size="itemSelect.PageSize" :total="totalCount" @current-change="getPaper(data)" class="mt-4"
            style="float: right;margin-bottom: 20px;" />
        <div class="flex gap-2" style="display: inline;">
            <el-tag type="primary">共{{ totalCount }}项记录</el-tag>
        </div>
        <!-- --------------添加--编辑 具体页面-------------- -->
        <el-drawer v-model="drawer" :title="selectTypeTitle" size="75%" :direction="direction"
            :before-close="handleClose">
            <AddDataBox :activeType="Number(selectType)" @toParentHandle="toParentHandle" />
        </el-drawer>

        <!-- --------------全部成果展示-------------- -->
        <el-table :data="paperData" highlight-current-row stripe border show-overflow-tooltip
            @sort-change="(event: any) => sortTableFun(event, data)" max-height="calc(100vh - 130px)"
            style="width: 100%; flex-grow: 1;" v-loading="loading">
            <el-table-column type="expand">
                <template #default="props">
                    <!-- 论文 -->
                    <div m="5" v-if="props.row.type == 1" style="line-height: 30px; margin: 0 20px;">
                        <p m="t-0 b-2"><span class="description">论文题目:</span> {{ props.row.title }}</p>
                        <p m="t-0 b-2"><span class="description">论文类型:</span> {{ props.row.subType }}</p>
                        <p m="t-0 b-2">
                            <span class="description" v-if="props.row.subType == '期刊论文'">期刊名称:</span>
                            <span class="description" v-else-if="props.row.subType == '学位论文'">学位:</span> 
                            {{ props.row.journal }}
                        </p>
                        <p m="t-0 b-2"><span class="description">负责老师:</span> {{ props.row.correspond }}</p>
                        <p m="t-0 b-2"><span class="description">负责学生:</span> {{ props.row.firstAuthor }}</p>
                        <p m="t-0 b-2"><span class="description">全部作者:</span> {{ props.row.allAuthors }}</p>
                        <p m="t-0 b-2"><span class="description">发表年份:</span> {{ props.row.year }}</p>
                        <p m="t-0 b-2"><span class="description">主题 :</span> {{ props.row.theme }}</p>
                    </div>
                    <!-- 专利 -->
                    <div m="7" v-if="props.row.type == 2" style="line-height: 30px; margin: 0 20px;">
                        <p m="t-0 b-2"> <span class="description"> 专利名称:</span> {{ props.row.title }}</p>
                        <p m="t-0 b-2"> <span class="description"> 专利类型:</span> {{ props.row.subType }}</p>
                        <p m="t-0 b-2"> <span class="description"> 负责老师:</span> {{ props.row.correspond }}</p>
                        <p m="t-0 b-2"> <span class="description"> 负责学生:</span> {{ props.row.firstAuthor }}</p>
                        <p m="t-0 b-2"> <span class="description"> 全部作者:</span> {{ props.row.allAuthors }}</p>
                        <p m="t-0 b-2"> <span class="description"> 发表年份:</span> {{ props.row.year }}</p>
                        <p m="t-0 b-2"> <span class="description"> 专利号:</span> {{ props.row.number }}</p>
                        <p m="t-0 b-2"> <span class="description"> 主题 :</span> {{ props.row.theme }}</p>
                    </div>
                    <!-- 软著 -->
                    <div m="5" v-if="props.row.type == 3" style="line-height: 30px; margin: 0 20px;">
                        <p m="t-0 b-2"><span class="description">名称 :</span> {{ props.row.title }}</p>
                        <p m="t-0 b-2"><span class="description">负责老师:</span> {{ props.row.correspond }}</p>
                        <p m="t-0 b-2"><span class="description">负责学生:</span> {{ props.row.firstAuthor }}</p>
                        <p m="t-0 b-2"><span class="description">全部作者:</span> {{ props.row.allAuthors }}</p>
                        <p m="t-0 b-2"><span class="description">发表年份:</span> {{ props.row.year }}</p>
                        <p m="t-0 b-2"> <span class="description">申请号:</span> {{ props.row.number }}</p>
                        <p m="t-0 b-2"><span class="description">主题 :</span> {{ props.row.theme }}</p>
                    </div>
                    <!-- 奖项 -->
                    <div m="5" v-if="props.row.type == 4" style="line-height: 30px; margin: 0 20px;">
                        <p m="t-0 b-2"> <span class="description">奖项名称:</span> {{ props.row.title }}</p>
                        <p m="t-0 b-2"> <span class="description">老师 :</span> {{ props.row.correspond }}</p>
                        <p m="t-0 b-2"> <span class="description">学生 :</span> {{ props.row.firstAuthor }}</p>
                        <p m="t-0 b-2"> <span class="description">获奖年份:</span> {{ props.row.year }}</p>
                        <p m="t-0 b-2"> <span class="description">主题 :</span> {{ props.row.theme }}</p>
                    </div>
                    <!-- 基金项目 -->
                    <div m="5" v-if="props.row.type == 5" style="line-height: 30px; margin: 0 20px;">
                        <p m="t-0 b-2"><span class="description">基金名称:</span> {{ props.row.title }}</p>
                        <p m="t-0 b-2"><span class="description">类别:</span> {{ props.row.subType }}</p>
                        <p m="t-0 b-2"><span class="description">类型:</span> {{ props.row.journal }}</p>
                        <p m="t-0 b-2"><span class="description">负责老师:</span> {{ props.row.correspond }}</p>
                        <!-- <p m="t-0 b-2"><span class="description">负责学生:</span> {{ props.row.firstAuthor }}</p> -->
                        <p m="t-0 b-2"><span class="description">全部负责人:</span> {{ props.row.allAuthors }}</p>
                        <p m="t-0 b-2"><span class="description">金额（万元):</span> {{ props.row.amount }}</p>
                        <p m="t-0 b-2"><span class="description">获批年份:</span> {{ props.row.year }}</p>
                        <p m="t-0 b-2"><span class="description">执行日期:</span> {{ props.row.executionTime }}</p>
                        <p m="t-0 b-2"><span class="description">项目号:</span> {{ props.row.number }}</p> 
                        <p m="t-0 b-2"><span class="description">备注  :</span> {{ props.row.theme }}</p>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="ID" type="index" 
            :index="indexMethod(itemSelect.PageIndex, itemSelect.PageSize)" width="60" />
            <el-table-column label="名称" prop="title" min-width="200" />
            <el-table-column sortable="custom" label="类别" prop="type" width="120px">
                <template #default="scope">
                    <el-tag type="primary">{{ options.filter(item => item.value === scope.row.type)[0].label }}</el-tag>     
                </template>
            </el-table-column>
            <el-table-column sortable="custom" label="老师" prop="correspond" />
            <el-table-column sortable="custom" label="学生" prop="firstAuthor" />
            <el-table-column sortable="custom" lable label="获批年份" prop="year" />
            <el-table-column fixed="right" label="操作" width="160">
                <template #default="scope">
                    <el-button link type="primary" size="small" @click="downloadItem(scope.row)">
                        下载
                    </el-button>
                    <el-button link v-if="dataAll.identityID" type="primary" size="small"
                        @click="editItemDraw(scope.row, dataAll)">
                        编辑
                    </el-button>
                    <el-popconfirm v-if="dataAll.identityID" title="确定要删除吗？" @confirm="deleteItem(scope.row.id, data)">
                        <template #reference>
                            <el-button link type="primary" size="small">删除</el-button>
                        </template>
                    </el-popconfirm>
                </template>
            </el-table-column>
        </el-table>
    </div>
</template>

<script lang="ts">
import { defineComponent, reactive, toRefs, ref, watch, onActivated } from 'vue'
import { InitDataAll } from '@/type/all'
import { InitData } from '@/type/paper'
import type { DrawerProps } from 'element-plus'
import AddDataBox from '@/components/AddDataBox.vue'
import {
    downloadItem,
    deleteItem,
    getPaper,
    resetSelectForm,
    addDataDraw,
    editItemDraw,
    handleClose,
    resetQueryList,
    sortTableFun,
    identityDeter,
    indexMethod
} from '@/utils/commonFunctions';   // 引入公共方法




export default defineComponent({
    setup() {
        // 实例化数据
        const dataAll = reactive(new InitDataAll());
        const data = reactive(new InitData());
        const direction = ref<DrawerProps['direction']>('btt')


        // 生命周期
        onActivated(() => {
            // 获取论文列表
            getPaper(data)
            // 判断身份
            identityDeter(dataAll)
        })

        // 监听
        watch(() => dataAll.selectType, (newVal, oldVal) => {
            dataAll.selectTypeTitle = dataAll.options.filter((item) => item.value === dataAll.selectType)[0] ? dataAll.options.filter((item) => item.value === dataAll.selectType)[0].label : ""
        })


        // 监听数据是否加载
        watch(() => data.paperData, (newVal, oldVal) => {
            data.loading = !data.paperData ? true : false
        },
            { deep: true }
        )

        // 插入或编辑成功后，接收子组件传递的数据刷新页面并关闭遮罩层
        const toParentHandle = (status: boolean) => {
            getPaper(data)
            dataAll.drawer = status
        }


        return {
            data,
            dataAll,
            ...toRefs(dataAll),
            ...toRefs(data),
            direction,
            handleClose,
            toParentHandle,
            // 引入的公共方法
            getPaper,
            downloadItem,
            deleteItem,
            addDataDraw,
            resetSelectForm,
            editItemDraw,
            sortTableFun,
            indexMethod
        }
    },
    components: {
        AddDataBox,
    }
})
</script>

<style scoped>
.addSearch {
    display: flex;
    justify-content: space-between;

    .add {
        display: flex;
    }
}
</style>