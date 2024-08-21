import { createRouter, createWebHistory, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/login',
    name: 'login',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "login" */ '../views/LoginView.vue')
  },
  {
    path: '/',
    name: 'home',
    component: HomeView,
    redirect: 'login',
    children: [
      {
        path: '/all',
        name: 'all',
        meta: {
          "title": '全部',
          isShow: true,
        },
        component: () => import(/* webpackChunkName: "all" */ '../views/AllView.vue')
      },
      {
        path: '/paper',
        name: 'paper',
        meta: {
          "title": '论文',
          isShow: true
        },
        component: () => import(/* webpackChunkName: "paper" */ '../views/PaperView.vue')
      },
      {
        path: '/patent',
        name: 'patent',
        meta: {
          "title": '专利',
          isShow: true
        },
        component: () => import(/* webpackChunkName: "patent" */ '../views/PatentView.vue')
      },
      {
        path: '/software',
        name: 'software',
        meta: {
          "title": '软著',
          isShow: true
        },
        component: () => import(/* webpackChunkName: "fund" */ '../views/SoftwareView.vue')
      },
      {
        path: '/prize',
        name: 'prize',
        meta: {
          "title": '奖项',
          isShow: true
        },
        component: () => import(/* webpackChunkName: "fund" */ '../views/PrizeView.vue')
      },
      {
        path: '/project',
        name: 'project',
        meta: {
          "title": '项目',
          isShow: true
        },
        component: () => import(/* webpackChunkName: "fund" */ '../views/ProjectView.vue')
      },
    ]
  },
]

const router = createRouter({
  // history: createWebHistory(process.env.BASE_URL),
  history: createWebHashHistory(),
  routes
})



export default router
