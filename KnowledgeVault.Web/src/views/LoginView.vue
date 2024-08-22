<template>
    <div class="login-box">
        <Transition name="fade">
            <div style="position: absolute;width: 100%; height: 100%; background-color: #7299df; z-index: -1;"></div>
        </Transition>

        <el-form ref="ruleFormRef" style="max-width: 600px" :model="ruleForm" label-width="auto" class="demo-ruleForm">
            <h2>海岸地理环境智能感知与建模团队<br />成果管理系统</h2>
            <el-form-item>


                <el-button class="login-btn" type="primary" @click="adminShow = true">管理员登录</el-button>
                <el-button class="login-btn login-btn-right" @click="jumpToHome">游客访问</el-button>
            </el-form-item>
            <el-form-item label="管理员密码" prop="password" class="admin" v-if="adminShow">
                <el-input type="password" v-model="ruleForm.password" @keyup.enter="submit"
                    style="width: 60%; margin-right: 10px;" />
                <el-button type="primary" @click="submit">登录</el-button>
            </el-form-item>
        </el-form>
    </div>

    <div class="developer">
        开发者：{{dev}}
    </div>
</template>

<script lang="ts">
import { defineComponent, reactive, toRefs, ref, inject } from 'vue'
import { LoginData } from '../type/login'
import type { FormInstance } from 'element-plus'
import { verifyIdentity } from '../request/api'
import { useRouter } from 'vue-router'
import { ElMessage } from 'element-plus'
import ParticleBox from "@/components/ParticleBox.vue"
import Cookies from 'js-cookie'

export default defineComponent({
    setup() {
        const ruleFormRef = ref<FormInstance>();
        const router = useRouter();
        const data = reactive(new LoginData());
        const adminShow = ref(false);   // 管理员登陆密码默认不显示

        let dev="JUU3JThFJThCJUU4JTkyJTk5JUU2JTgxJUE5JUVGJUJDJTg4JUU1JTg5JThEJUU3JUFCJUFGJUVGJUJDJTg5JUUzJTgwJTgxJUU2JTk2JUI5JUU5JTlDJTg3JUVGJUJDJTg4JUU1JTkwJThFJUU3JUFCJUFGLyVFNiU5NSVCMCVFNiU4RCVBRSVFNSVCQSU5MyVFRiVCQyU4OQ=="
        dev=atob(dev)
        dev=decodeURI(dev);
        // 提示
        const warnTip = (message: string) => {
            ElMessage({
                message: message,
                type: 'warning'
            })
        }


        // 管理员输入密码后登陆
        const submit = () => {
            verifyIdentity(data.ruleForm.password).then((res: any) => {
                if (res === true) {   // 登陆成功
                    Cookies.set("token", data.ruleForm.password, { expires: 7 })
                    router.push("/all")
                    console.log('登录成功!')
                } else { // 登陆失败
                    warnTip('密码错误！')
                    console.log('登录失败!')
                }
            }).catch((error) => {
                console.log(error)
            })
        }

        const jumpToHome = () => {
            Cookies.remove("token")
            router.push('/all')
        }
        return { dev, jumpToHome, adminShow, ...toRefs(data), submit, ruleFormRef }
    }
})
</script>

<style lang="scss" scoped>
.login-box {

    /* 定义过渡效果 */
    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 2s ease;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
        /* 初始状态为透明 */
    }


    width: 100%;
    height: 100%;
    // background-color: skyblue;
    text-align: center;
    // flex布局水平垂直居中
    display: flex;
    justify-content: center;
    align-items: center;

    .demo-ruleForm {
        width: 50%;
        background-color: #f5eeee;
        // margin: 200px auto;
        padding: 60px;
        border-radius: 30px;
        color: #191970;
        user-select: none;

        .tip-btn {
            width: 8%;
        }

        .login-btn {
            width: 45%;
            height: 100%;
            margin: 2.5%;
            padding: 0;
        }

        h2 {
            margin-bottom: 10px;
            user-select: none;
        }

        .admin {
            // width: 90%;
            margin-top: 40px;
            margin-left: 5%;
            display: flex;

        }
    }

    .particle {
        position: absolute;
        bottom: 10px;
        right: 10px;
    }
}

.developer{
    position: absolute;
    right: 8px;
    bottom: 8px;
}
</style>