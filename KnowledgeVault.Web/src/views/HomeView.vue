<template>
  <div class="home">
    <!-- -----------header----------- -->
    <el-header class="header">
      <div class="header-content">
        <div class="logo">
          <img src="../assets/logo.png" alt="系统Logo">
        </div>
        <h2 class="title">CIRSM 成果管理系统</h2>
        <div class="auth-btn">
          <el-button link type="primary" @click="showLoginDialog" v-if="!isAdmin">
            登录管理员
          </el-button>
          <el-button link type="danger" @click="logout" v-else>
            退出管理员
          </el-button>
        </div>
      </div>
    </el-header>

    <!-- 登录弹窗 -->
    <el-dialog v-model="loginDialogVisible" title="管理员登录" width="30%">
      <el-form :model="loginForm" label-width="100px">
        <el-form-item label="管理员密码">
          <el-input type="password" v-model="loginForm.password" @keyup.enter="login"></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="loginDialogVisible = false">取消</el-button>
        <el-button type="primary" @click="login">登录</el-button>
      </template>
    </el-dialog>

    <el-container>
      <!-- -----------Aside----------- -->
      <el-aside width="200px">
        <el-menu active-text-color="#ffd04b" background-color="#545c64" text-color="#fff" :default-active="active"
          router>
          <el-menu-item :index="need.path" v-for="need in needList" :key="need.path">
            <span>{{ need.meta.title }}</span>
          </el-menu-item>
        </el-menu>
      </el-aside>
      <!-- -----------main----------- -->
      <el-main>
        <router-view v-slot="{ Component }">
          <component :is="Component" :key="componentKey" />
        </router-view>
      </el-main>
    </el-container>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router'
import { InitDataAll } from '@/type/all'
import Cookies from 'js-cookie'
import { verifyIdentity } from '@/request/api'
import { ElMessage } from 'element-plus'

export default defineComponent({
  name: 'HomeView',
  setup() {
    const dataAll = reactive(new InitDataAll());
    const route = useRoute();
    const router = useRouter();

    const needList = router.getRoutes().filter(r => {
      return r.meta && r.meta.isShow
    })

    // 登录相关状态
    const isAdmin = ref(!!Cookies.get("token"));
    const loginDialogVisible = ref(false);
    const loginForm = reactive({
      password: ''
    });
    const componentKey = ref(0); // 用于强制刷新组件

    // 显示登录弹窗
    const showLoginDialog = () => {
      loginDialogVisible.value = true;
    }

    // 管理员登录
    const login = () => {
      verifyIdentity(loginForm.password).then((res: any) => {
        if (res === true) {
          Cookies.set("token", loginForm.password, { expires: 7 });
          isAdmin.value = true;
          loginDialogVisible.value = false;
          loginForm.password = '';
          componentKey.value++; // 强制刷新组件
          ElMessage.success('登录成功');
        } else {
          ElMessage.error('密码错误');
        }
      }).catch((error) => {
        console.log(error);
        ElMessage.error('登录失败');
      });
    }

    // 退出登录
    const logout = () => {
      Cookies.remove("token");
      isAdmin.value = false;
      componentKey.value++; // 强制刷新组件
      ElMessage.success('已退出管理员账号');
    }

    return {
      dataAll,
      route,
      needList,
      active: route.path,
      isAdmin,
      loginDialogVisible,
      loginForm,
      componentKey,
      showLoginDialog,
      login,
      logout
    }
  }
});
</script>

<style lang="scss" scoped>
.home {
  width: 100%;
  height: 100%;

  .header {
    height: 50px;
    line-height: 50px;
    border-bottom: 1px solid #e6e6e6;
    display: flex;
    align-items: center;
    padding: 0 10px;

    .header-content {
      width: 100%;
      display: flex;
      justify-content: space-between;
      align-items: center;

      .logo {
        img {
          height: 36px;
          margin-top:14px;
        }
      }

      .title {
        margin: 0;
        font-size: 18px;
        color: #333;
      }

      .auth-btn {
        .el-button {
          padding: 0;
          margin-left: 10px;
        }
      }
    }
  }

  .el-container {
    height: calc(100vh - 50px);

    .el-aside {
      height: 100%;
      background-color: #545c64;
    }

    .el-main {
      background-color: #fffff0;
      padding: 15px;
    }
  }
}
</style>