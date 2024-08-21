<template>
    <div class="login-box">
        <Transition name="fade">
            <ParticleBox v-if="particleShow"/>
        </Transition>
        <Transition name="fade">
            <div v-if="!particleShow"
                style="position: absolute;width: 100%; height: 100%; background-color: #7299df; z-index: -1;"></div>
        </Transition>

        <el-form ref="ruleFormRef" style="max-width: 600px" :model="ruleForm" status-icon :rules="rules"
            label-width="auto" class="demo-ruleForm">
            <h2>海岸地理环境智能感知与建模团队<br />成果管理系统</h2>
            <el-form-item>
                <!-- <el-tooltip class="box-item" effect="dark" content="账号：admin &emsp;密码：123456" placement="bottom-start">
                  <el-button class="tip-btn" type="info">提示</el-button>
              </el-tooltip> -->
                <el-button class="login-btn" type="primary" @click="submitForm()">管理员登录</el-button>
                <el-button class="login-btn login-btn-right" @click="resetForm()">游客访问</el-button>
            </el-form-item>
            <el-form-item label="管理员密码" prop="password" class="admin" v-if="adminShow" >
                <el-input type="password" v-model="ruleForm.password" 
                @keydown.enter="submit" autocomplete="on"
                    style="width: 60%; margin-right: 10px;" />
                <el-button type="primary" @click="submit">登录</el-button>
            </el-form-item>
        </el-form>
        <el-button size="small" class="particle" @click="particleShow = !particleShow">
            {{ particleShow ? '关闭动画' : '开启动画' }}
        </el-button>

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



export default defineComponent({
    conmponents: {
        ParticleBox
    },
    setup() {
        const ruleFormRef = ref<FormInstance>();
        const router = useRouter();
        const data = reactive(new LoginData());
        const adminShow = ref(false);   // 管理员登陆密码默认不显示
        const particleShow = ref(true);  // 默认在登录界面显示粒子效果



        const rules = {
            username: [
                { required: true, message: '请输入账户', trigger: 'blur' },
                { min: 3, max: 10, message: '账号长度在3到10个字符之间' }
            ],
            password: [
                { required: true, message: '请输入密码', trigger: 'blur' },
                { min: 6, max: 10, message: '密码长度在6到10个字符之间' }
            ]
        };
        // 提示
        const warnTip = (message: string) => {
            ElMessage({
                message: message,
                type: 'warning'
            })
        }

        // 登录
        // const submitForm = (formEl: FormInstance | undefined) => {
        //     if (!formEl) return
        //     formEl.validate((valid) => {
        //         if (valid) {
        //             // 验证成功处理登录逻辑
        //             login(data.ruleForm).then((res) => {
        //                 if (res.data.status === 200) {
        //                     console.log('登陆成功!')
        //                     // 保存token
        //                     localStorage.setItem("token", res.data.token)
        //                     // 跳转页面
        //                     router.push('/')
        //                 } else {
        //                     console.log('登录失败!')
        //                     warnTip('账户或密码错误！')
        //                 }
        //             }).catch((error) => {
        //                 console.log(error)
        //             })
        //         } else {
        //             console.log('error submit!')
        //             warnTip('账户或密码不符合规范！')
        //             return false
        //         }
        //     })
        // console.log(formEl)
        // } 
        // 管理员登陆
        const submitForm = () => {
            adminShow.value = true
        }
        // 游客访问
        const resetForm = () => {
            // data.ruleForm.username = "";
            // data.ruleForm.password = "";
            localStorage.removeItem("NbuAchievementManagementSystemAdministrator")// loaclStorage 存储身份信息
            // sessionStorage.setItem("NbuAchievementManagementSystemAdministrator", "true")   // 路由守卫，跳转登陆页面，有这个sessionStorage信息才能进入其他页面
            router.push("/all")
        }
        // 管理员输入密码后登陆
        const submit = () => {
            verifyIdentity(data.ruleForm.password).then((res: any) => {
                console.log(1)
                if (res === true) {   // 登陆成功
                    console.log(2)
                    localStorage.setItem("NbuAchievementManagementSystemAdministrator", "admin")    // loaclStorage 存储身份信息
                    // sessionStorage.setItem("NbuAchievementManagementSystemAdministrator", "true")   // 路由守卫，跳转登陆页面，有这个sessionStorage信息才能进入其他页面
                    router.push("/all")
                    console.log('登录成功!')
                } else { // 登陆失败
                    console.log('登录失败!')
                    warnTip('密码错误！')
                }
            }).catch((error) => {
                console.log(error)
            })
        }
        return { particleShow, adminShow, ...toRefs(data), rules, submitForm, resetForm, submit, ruleFormRef }
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
</style>