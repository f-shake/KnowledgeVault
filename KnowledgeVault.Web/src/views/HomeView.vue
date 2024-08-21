<template>
  <div class="home">
      <!-- -----------------header----------------- -->
      <el-header>
        <el-row :gutter="20">
          <el-col :span="4" class="logo">
            <img src="../assets/logo.png" alt="">
          </el-col>
          <el-col :span="16">
            <h2>成果管理系统</h2>
          </el-col>
          <el-col :span="4" class="quit-login">
            <el-button type="primary" @click="exit">
              退出
            </el-button>
          </el-col>
        </el-row>
      </el-header>
      <el-container>
        <!-- -----------------Aside----------------- -->
        <el-aside width="200px">
          <el-menu active-text-color="#ffd04b" background-color="#545c64" text-color="#fff" :default-active="active"
            router>
            <!-- router 开启路由模式 -->
            <el-menu-item :index="need.path" v-for="need in needList" :key="need.path">
              <span>{{ need.meta.title }}</span>
            </el-menu-item>
          </el-menu>
        </el-aside>
        <!-- -----------------main----------------- -->
        <el-main>
          <router-view v-slot="{ Component }">
            <keep-alive>
              <component :is="Component" />
            </keep-alive>
          </router-view>
        </el-main>
      </el-container>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive } from 'vue';
import { useRoute, useRouter } from 'vue-router'
import { InitDataAll } from '@/type/all'

export default defineComponent({
  name: 'HomeView',
  setup() {
    // 实例化数据
    const dataAll = reactive(new InitDataAll());
    // 当前路由信息
    const route = useRoute();


    // 需要展示的路由列表
    const router = useRouter();
    const needList = router.getRoutes().filter(r => {
      return r.meta && r.meta.isShow
    })

    // 退出登录
    const exit = () => {
      router.push('/login')
      localStorage.removeItem("NbuAchievementManagementSystemAdministrator"); // 删除权限
    }

    return { dataAll, route, needList, active: route.path, exit }
  }
});
</script>

<style lang="scss" scoped>
.home {
  width: 100%;
  height: 100%;

  .el-header {
    height: 80px;
    text-align: center;
    position: relative;
    border-bottom: 2px solid rgb(49, 47, 44);

    img {
      height: 60px;
      position: absolute;
      top: 50%;
      transform: translate(-50%, -50%);
      user-select: none;
    }

    h2,
    .quit-login {
      height: 80px;
      line-height: 80px;
      color: rgb(49, 47, 44);
      user-select: none;
    }
  }

  .el-container {
    height: calc(100vh - 80px);
    // height: 100vh;

    .el-aside {
      height: 100%;
      background-color: #545c64;
    }

    .el-main {
      background-color: #fffff0;
    }
  }
}
</style>
