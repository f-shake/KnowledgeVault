<template>
    <div style="display: flex; height:100%; flex-direction: column;">
        <!-- -----------搜索页面----------- -->
        <div class="search">
            <el-form :inline="true" v-model="itemSelect" class="demo-form-inline">
                <el-form-item label="标题">
                    <el-input v-model="itemSelect.title" placeholder="请输入关键字" maxlength clearable
                        style="width: 200px;" />
                </el-form-item>
                <el-form-item label="老师">
                    <el-input v-model="itemSelect.Correspond" placeholder="请输入" clearable style="width: 100px;" />
                </el-form-item>
                <el-form-item label="学生">
                    <el-input v-model="itemSelect.Author" placeholder="请输入" clearable style="width: 100px;" />
                </el-form-item>
                <el-form-item label="类别">
                    <el-select v-model="itemSelect.subType" placeholder="请选择项目类别" clearable style="width: 160px;">
                        <el-option label="国家级" value="国家级" />
                        <el-option label="省部级" value="省部级" />
                        <el-option label="厅市级及以下" value="厅市级及以下" />
                        <el-option label="横向" value="横向" />
                    </el-select>
                </el-form-item>
                <el-form-item label="获批年份">
                    <el-input v-model="itemSelect.Year" placeholder="请输入年份" type="number" clearable
                        style="width: 120px;" />
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="getPaper(data)">查询</el-button>
                    <el-button @click="resetSelectForm(data)">重置</el-button>
                </el-form-item>
            </el-form>
        </div>
        <!-- --------------分页-------------- -->
        <el-pagination small background layout="prev, pager, next" v-model:current-page="itemSelect.PageIndex"
            :page-size="itemSelect.PageSize" :total="totalCount" @current-change="getPaper(data)" class="mt-4"
            style="float: right;margin-bottom: 20px;" />
        <div class="flex gap-2" style="display: inline;">
            <el-tag type="primary">共{{ totalCount }}项记录</el-tag>
        </div>
        <!-- --------------编辑 具体页面-------------- -->
        <el-drawer v-model="drawer" :title="selectTypeTitle" size="75%" :before-close="handleClose">
            <AddDataBox :activeType="Number(selectType)" @toParentHandle="toParentHandle" />
        </el-drawer>
        <!-- --------------成果展示-------------- -->
        <el-table :data="paperData" v-loading="loading" highlight-current-row stripe border show-overflow-tooltip
            max-height="calc(100vh - 130px)" @sort-change="(event: any) => sortTableFun(event, data)"
            style="width: 100%">
            <el-table-column type="expand">
                <template #default="props">
                    <div m="5" style="line-height: 30px; margin: 0 20px;">
                        <p m="t-0 b-2"><span class="description">基金名称:</span> {{ props.row.title }}</p>
                        <p m="t-0 b-2"><span class="description">类别:</span> {{ props.row.subType }}</p>
                        <p m="t-0 b-2"><span class="description">类型:</span> {{ props.row.journal }}</p>
                        <p m="t-0 b-2"><span class="description">负责老师:</span> {{ props.row.correspond }}</p>
                        <!-- <p m="t-0 b-2"><span class="description">负责学生:</span> {{ props.row.firstAuthor }}</p> -->
                        <p m="t-0 b-2"><span class="description">全部负责人:</span> {{ props.row.allAuthors }}</p>
                        <p m="t-0 b-2"><span class="description">金额（万元）:</span> {{ props.row.amount }}</p>
                        <p m="t-0 b-2"><span class="description">获批年份:</span> {{ props.row.year }}</p>
                        <p m="t-0 b-2"><span class="description">执行日期:</span> {{ props.row.executionTime }}</p>
                        <p m="t-0 b-2"><span class="description">项目号:</span> {{ props.row.number }}</p> 
                        <p m="t-0 b-2"><span class="description">备注 :</span> {{ props.row.theme }}</p>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="ID" type="index" 
            :index="indexMethod(itemSelect.PageIndex, itemSelect.PageSize)" width="60" />
            <el-table-column label="基金名称" prop="title" min-width="200" />
            <el-table-column sortable="custom" label="老师" prop="correspond" />
            <!-- <el-table-column sortable="custom" label="学生" prop="firstAuthor" /> -->
            <el-table-column sortable="custom" label="类别" prop="subType" min-width="160" />
            <el-table-column sortable="custom" label="获批年份" prop="year" />
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
import { defineComponent, reactive, toRefs, watch, onActivated } from 'vue'
import { InitData } from '@/type/paper';
import { InitDataAll } from '@/type/all'
import {
    downloadItem,
    deleteItem,
    getPaper,
    resetSelectForm,
    editItemDraw,
    handleClose,
    sortTableFun,
    identityDeter,
    indexMethod
} from '@/utils/commonFunctions';   // 引入公共方法


export default defineComponent({
    setup() {
        // 实例化数据
        const data = reactive(new InitData());
        const dataAll = reactive(new InitDataAll());
        // console.log("dataAll.identityID", dataAll.identityID)
        // 生命周期
        onActivated(() => {
            data.itemSelect.type = 5
            getPaper(data)
            // 判断身份
            identityDeter(dataAll)
        })
        // 监听数据是否加载
        watch(() => data.paperData, (newVal, oldVal) => {
            data.loading = !data.paperData ? true : false
        },
            { deep: true }
        )


        // 编辑成功后，接收子组件传递的数据刷新页面并关闭遮罩层
        const toParentHandle = (status: boolean) => {
            getPaper(data)
            dataAll.drawer = status
        }

        return {
            data,
            dataAll,
            ...toRefs(data),
            ...toRefs(dataAll),
            toParentHandle,
            getPaper,
            resetSelectForm,
            downloadItem,
            editItemDraw,
            deleteItem,
            handleClose,
            onActivated,
            sortTableFun,
            indexMethod
        }
    }
})
</script>

<style scoped></style>