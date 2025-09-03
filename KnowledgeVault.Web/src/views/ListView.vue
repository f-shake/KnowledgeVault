<template>
  <div class="result-container">
    <!-- 搜索筛选区域 -->
    <div class="search-filter">
      <el-form :inline="true" v-model="itemSelect" class="filter-form" @keydown.enter="getPaper(data)">
        <el-form-item label="标题">
          <el-input v-model="itemSelect.title" placeholder="请输入关键字" clearable />
        </el-form-item>
        <el-form-item label="老师">
          <el-input v-model="itemSelect.Correspond" placeholder="请输入" clearable />
        </el-form-item>
        <el-form-item label="学生" v-if="config.showStudent">
          <el-input v-model="itemSelect.Author" placeholder="请输入" clearable />
        </el-form-item>
        <el-form-item label="类型" v-if="config.typeOptions && !isAllView">
          <el-select v-model="itemSelect.subType" :placeholder="config.typePlaceholder" clearable>
            <el-option v-for="option in config.typeOptions" :key="option.value" :label="option.label" :value="option.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="年份">
          <el-input v-model="itemSelect.Year" placeholder="请输入年份" type="number" clearable />
        </el-form-item>
        <el-form-item class="action-buttons">
          <el-button type="primary" @click="getPaper(data)">查询</el-button>
          <el-button @click="resetSelectForm(data)">重置</el-button>
          <el-button @click="getExport(data)" v-if="dataAll.identityID && isAllView">导出</el-button>
         <div class="add-action" v-if="dataAll.identityID && isAllView">
        <el-dropdown @command="addData">
          <el-button type="primary">
            添加成果<el-icon class="el-icon--right"><arrow-down /></el-icon>
          </el-button>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item v-for="item in options" :key="item.value" :command="item.value">
                {{ item.label }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </div> </el-form-item>
      </el-form>

      <!-- 添加成果按钮 -->
    
    </div>

    <!-- 分页和记录数 -->
    <div class="pagination-container">
      <el-tag type="primary" class="total-count">共{{ totalCount }}项记录</el-tag>
      <el-pagination 
        small 
        background 
        layout="prev, pager, next" 
        v-model:current-page="itemSelect.PageIndex"
        :page-size="itemSelect.PageSize" 
        :total="totalCount" 
        @current-change="getPaper(data)" 
      />
    </div>

    <!-- 成果展示表格 -->
    <div class="table-container">
      <el-table 
        :data="paperData" 
        v-loading="loading" 
        highlight-current-row 
        stripe 
        border 
        show-overflow-tooltip
        @sort-change="(event: any) => sortTableFun(event, data)"
      >
        <el-table-column type="expand">
          <template #default="props">
            <div class="expand-content">
              <!-- 论文 -->
              <div v-if="props.row.type == 1">
                <p><span class="description">论文题目:</span> {{ props.row.title }}</p>
                <p><span class="description">论文类型:</span> {{ props.row.subType }}</p>
                <p>
                  <span class="description" v-if="props.row.subType == '期刊论文'">期刊名称:</span>
                  <span class="description" v-else-if="props.row.subType == '学位论文'">学位:</span>
                  {{ props.row.journal }}
                </p>
                <p><span class="description">负责老师:</span> {{ props.row.correspond }}</p>
                <p><span class="description">负责学生:</span> {{ props.row.firstAuthor }}</p>
                <p><span class="description">全部作者:</span> {{ props.row.allAuthors }}</p>
                <p><span class="description">发表年份:</span> {{ props.row.year }}</p>
                <p><span class="description">主题 :</span> {{ props.row.theme }}</p>
              </div>
              
              <!-- 专利 -->
              <div v-if="props.row.type == 2">
                <p><span class="description">专利名称:</span> {{ props.row.title }}</p>
                <p><span class="description">专利类型:</span> {{ props.row.subType }}</p>
                <p><span class="description">负责老师:</span> {{ props.row.correspond }}</p>
                <p><span class="description">负责学生:</span> {{ props.row.firstAuthor }}</p>
                <p><span class="description">全部作者:</span> {{ props.row.allAuthors }}</p>
                <p><span class="description">发表年份:</span> {{ props.row.year }}</p>
                <p><span class="description">专利号:</span> {{ props.row.number }}</p>
                <p><span class="description">主题 :</span> {{ props.row.theme }}</p>
              </div>
              
              <!-- 软著 -->
              <div v-if="props.row.type == 3">
                <p><span class="description">名称 :</span> {{ props.row.title }}</p>
                <p><span class="description">负责老师:</span> {{ props.row.correspond }}</p>
                <p><span class="description">负责学生:</span> {{ props.row.firstAuthor }}</p>
                <p><span class="description">全部作者:</span> {{ props.row.allAuthors }}</p>
                <p><span class="description">发表年份:</span> {{ props.row.year }}</p>
                <p><span class="description">申请号:</span> {{ props.row.number }}</p>
                <p><span class="description">主题 :</span> {{ props.row.theme }}</p>
              </div>
              
              <!-- 奖项 -->
              <div v-if="props.row.type == 4">
                <p><span class="description">奖项名称:</span> {{ props.row.title }}</p>
                <p><span class="description">老师 :</span> {{ props.row.correspond }}</p>
                <p><span class="description">学生 :</span> {{ props.row.firstAuthor }}</p>
                <p><span class="description">获奖年份:</span> {{ props.row.year }}</p>
                <p><span class="description">主题 :</span> {{ props.row.theme }}</p>
              </div>
              
              <!-- 基金项目 -->
              <div v-if="props.row.type == 5">
                <p><span class="description">基金名称:</span> {{ props.row.title }}</p>
                <p><span class="description">类别:</span> {{ props.row.subType }}</p>
                <p><span class="description">类型:</span> {{ props.row.journal }}</p>
                <p><span class="description">负责老师:</span> {{ props.row.correspond }}</p>
                <p><span class="description">全部负责人:</span> {{ props.row.allAuthors }}</p>
                <p><span class="description">金额（万元):</span> {{ props.row.amount }}</p>
                <p><span class="description">获批年份:</span> {{ props.row.year }}</p>
                <p><span class="description">执行日期:</span> {{ props.row.executionTime }}</p>
                <p><span class="description">项目号:</span> {{ props.row.number }}</p>
                <p><span class="description">备注 :</span> {{ props.row.theme }}</p>
              </div>
            </div>
          </template>
        </el-table-column>
        <el-table-column label="ID" type="index" 
          :index="indexMethod(itemSelect.PageIndex, itemSelect.PageSize)"
          width="60" />
        <el-table-column :label="isAllView ? '名称' : config.tableTitleLabel" prop="title" min-width="200" />
        <el-table-column v-if="isAllView" sortable="custom" label="类别" prop="type" width="120px">
          <template #default="scope">
            <el-tag type="primary">{{ getTypeLabel(scope.row.type) }}</el-tag>
          </template>
        </el-table-column>
        <el-table-column sortable="custom" label="老师" prop="correspond" />
        <el-table-column sortable="custom" label="学生" prop="firstAuthor" v-if="config.showStudent || isAllView" />
        <el-table-column v-if="!isAllView && config.showTableType" sortable="custom" :label="config.tableTypeLabel" prop="subType" min-width="160" />
        <el-table-column v-if="!isAllView && config.showTableJournal" sortable="custom" :label="config.tableJournalLabel" prop="journal" min-width="160" />
        <el-table-column sortable="custom" :label="isAllView ? '年份' : config.yearLabel" prop="year" />
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

    <!-- 编辑抽屉 -->
    <el-drawer v-model="drawer" :title="selectTypeTitle" :size="isAllView ? '75%' : '75%'" 
      :direction="isAllView ? 'btt' : 'rtl'" :before-close="handleClose">
      <AddDataBox :activeType="Number(selectType)" @toParentHandle="toParentHandle" />
    </el-drawer>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive, toRefs, watch, onActivated, computed } from 'vue'
import { InitData } from '@/type/paper';
import { InitDataAll } from '@/type/all'
import { ArrowDown } from '@element-plus/icons-vue'
import {
  downloadItem,
  deleteItem,
  getPaper,
  getExport,
  resetSelectForm,
  editItemDraw,
  handleClose,
  sortTableFun,
  identityDeter,
  indexMethod,
  addDataDraw
} from '@/utils/commonFunctions';

// 配置映射表
const CONFIG_MAP = {
  0: { // 全部
    showStudent: true,
    showType: false,
    showJournal: false,
    showTableType: false,
    showTableJournal: false,
    showAmount: false,
    showExecutionTime: false,
    showNumber: false
  },
  1: { // 论文
    typeOptions: [
      { label: '期刊论文', value: '期刊论文' },
      { label: '学位论文', value: '学位论文' }
    ],
    typePlaceholder: '请选择成果类型',
    titleLabel: '论文题目',
    typeLabel: '论文类型',
    journalLabel: '期刊名称',
    authorsLabel: '作者',
    yearLabel: '发表年份',
    themeLabel: '主题',
    tableTitleLabel: '论文题目',
    tableTypeLabel: '类型',
    tableJournalLabel: '期刊',
    showStudent: true,
    showType: true,
    showJournal: true,
    showTableType: false,
    showTableJournal: true,
    showAmount: false,
    showExecutionTime: false,
    showNumber: false
  },
  2: { // 专利
    typeOptions: [
      { label: '发明专利', value: '发明专利' },
      { label: '实用新型专利', value: '实用新型专利' },
      { label: '外观设计专利', value: '外观设计专利' }
    ],
    typePlaceholder: '请选择专利类型',
    titleLabel: '专利名称',
    typeLabel: '专利类型',
    journalLabel: '',
    authorsLabel: '作者',
    yearLabel: '发表年份',
    themeLabel: '主题',
    tableTitleLabel: '专利名称',
    tableTypeLabel: '类型',
    tableJournalLabel: '',
    showStudent: true,
    showType: true,
    showJournal: false,
    showTableType: true,
    showTableJournal: false,
    showAmount: false,
    showExecutionTime: false,
    showNumber: true,
    numberLabel: '专利号'
  },
  3: { // 软著
    typeOptions: null,
    typePlaceholder: '',
    titleLabel: '名称',
    typeLabel: '',
    journalLabel: '',
    authorsLabel: '作者',
    yearLabel: '发表年份',
    themeLabel: '主题',
    tableTitleLabel: '软著题目',
    tableTypeLabel: '',
    tableJournalLabel: '',
    showStudent: true,
    showType: false,
    showJournal: false,
    showTableType: false,
    showTableJournal: false,
    showAmount: false,
    showExecutionTime: false,
    showNumber: true,
    numberLabel: '申请号'
  },
  4: { // 奖项
    typeOptions: null,
    typePlaceholder: '',
    titleLabel: '奖项名称',
    typeLabel: '',
    journalLabel: '',
    authorsLabel: '',
    yearLabel: '获奖年份',
    themeLabel: '主题',
    tableTitleLabel: '奖项名称',
    tableTypeLabel: '',
    tableJournalLabel: '',
    showStudent: true,
    showType: false,
    showJournal: false,
    showTableType: false,
    showTableJournal: false,
    showAmount: false,
    showExecutionTime: false,
    showNumber: false
  },
  5: { // 基金项目
    typeOptions: [
      { label: '国家级', value: '国家级' },
      { label: '省部级', value: '省部级' },
      { label: '厅市级及以下', value: '厅市级及以下' },
      { label: '横向', value: '横向' }
    ],
    typePlaceholder: '请选择项目类别',
    titleLabel: '基金名称',
    typeLabel: '类别',
    journalLabel: '类型',
    authorsLabel: '负责人',
    yearLabel: '获批年份',
    themeLabel: '备注',
    tableTitleLabel: '基金名称',
    tableTypeLabel: '类别',
    tableJournalLabel: '类型',
    showStudent: false,
    showType: true,
    showJournal: true,
    showTableType: true,
    showTableJournal: true,
    showAmount: true,
    showExecutionTime: true,
    showNumber: true,
    numberLabel: '项目号'
  }
};

// 类型选项
const TYPE_OPTIONS = [
  { label: '论文', value: 1 },
  { label: '专利', value: 2 },
  { label: '软著', value: 3 },
  { label: '奖项', value: 4 },
  { label: '基金项目', value: 5 }
];

export default defineComponent({
  components: {
    ArrowDown
  },
  props: {
    type: {
      type: Number,
      default: 0 // 默认为全部视图
    }
  },
  setup(props) {
    // 实例化数据
    const data = reactive(new InitData());
    const dataAll = reactive(new InitDataAll());
    
    // 计算属性
    const isAllView = computed(() => props.type === 0);
    const config = computed(() => CONFIG_MAP[props.type as keyof typeof CONFIG_MAP]);
    const options = TYPE_OPTIONS;

    // 获取类型标签
    const getTypeLabel = (type: number) => {
      const option = options.find(item => item.value === type);
      return option ? option.label : '';
    };

    // 添加数据
    const addData = (command: number) => {
      dataAll.selectType = command;
      addDataDraw(dataAll);
    };

    // 初始化数据
    const initData = () => {
      data.itemSelect.type = props.type;
      getPaper(data);
      identityDeter(dataAll);
    };

    // 生命周期
    onActivated(initData);

    // 监听props.type变化
    watch(() => props.type, (newVal, oldVal) => {
      if (newVal !== oldVal) {
        initData();
      }
    }, { immediate: true });

    // 监听数据变化
    watch(() => data.paperData, (newVal) => {
      data.loading = !newVal;
    }, { deep: true });

    // 编辑成功后，接收子组件传递的数据刷新页面并关闭遮罩层
    const toParentHandle = (status: boolean) => {
      getPaper(data);
      dataAll.drawer = status;
    };

    return {
      data,
      dataAll,
      ...toRefs(data),
      ...toRefs(dataAll),
      isAllView,
      config,
      options,
      getTypeLabel,
      toParentHandle,
      addData,
      getPaper,
      downloadItem,
      deleteItem,
      resetSelectForm,
      editItemDraw,
      handleClose,
      sortTableFun,
      indexMethod,
      getExport
    };
  }
});
</script>

<style lang="scss" scoped>
.result-container {
  display: flex;
  flex-direction: column;
  height: calc(100% - 50px);
  padding: 16px;
  overflow: hidden;
}

.search-filter {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
  padding: 16px;
  background-color: #fff;
  border-radius: 4px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);

  .filter-form {
    flex: 1;
    min-width: 0;

    :deep(.el-form-item) {
      margin-bottom: 12px;
      margin-right: 12px;

      .el-input,
      .el-select {
        width: 160px;
      }

      &.action-buttons {
        margin-right: 0;
      }
    }
  }

  .add-action {
    margin-left: 16px;
  }
}

.pagination-container {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
  padding: 8px 16px;
  background-color: #fff;
  border-radius: 4px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);

  .total-count {
    font-size: 14px;
    padding: 8px 12px;
  }

  :deep(.el-pagination) {
    margin: 0;
  }
}

.table-container {
  flex: 1;
  overflow: hidden;
  background-color: #fff;
  border-radius: 4px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);
  
  :deep(.el-table) {
    height: 100%;
    
    .el-table__body-wrapper {
      overflow-y: auto;
    }
  }
  
  .expand-content {
    padding: 0 20px;
    line-height: 30px;
    
    p {
      margin: 8px 0;
    }
  }
}

.description {
  font-weight: bold;
  color: #606266;
  margin-right: 10px;
}

/* 响应式设计 */
@media (max-width: 1200px) {
  .search-filter {
    .filter-form :deep(.el-form-item) {
      .el-input,
      .el-select {
        width: 140px;
      }
    }
  }
}

@media (max-width: 992px) {
  .search-filter {
    flex-direction: column;
    align-items: flex-start;

    .filter-form {
      width: 100%;
    }

  }
}

@media (max-width: 768px) {
  .search-filter {
    .filter-form :deep(.el-form-item) {
      width: 100%;
      margin-right: 0;

      .el-input,
      .el-select {
        width: 100%;
      }
    }
  }

  .pagination-container {
    flex-direction: column;
    align-items: flex-start;
    gap: 8px;

    .el-pagination {
      width: 100%;
      justify-content: flex-start;
    }
  }
}
</style>