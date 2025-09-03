import { createRouter, createWebHistory, createWebHashHistory, RouteRecordRaw } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import UnifiedView from '../views/ListView.vue' // 引入统一组件

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'home',
    component: HomeView,
    children: [
      {
        path: '/',
        name: 'all',
        meta: {
          "title": '全部',
          isShow: true,
        },
        component: UnifiedView,
        props: { type: 0 } // 传递type参数表示全部视图
      },
      {
        path: '/paper',
        name: 'paper',
        meta: {
          "title": '论文',
          isShow: true
        },
        component: UnifiedView,
        props: { type: 1 } // 传递type参数表示论文视图
      },
      {
        path: '/patent',
        name: 'patent',
        meta: {
          "title": '专利',
          isShow: true
        },
        component: UnifiedView,
        props: { type: 2 } // 传递type参数表示专利视图
      },
      {
        path: '/software',
        name: 'software',
        meta: {
          "title": '软著',
          isShow: true
        },
        component: UnifiedView,
        props: { type: 3 } // 传递type参数表示软著视图
      },
      {
        path: '/prize',
        name: 'prize',
        meta: {
          "title": '奖项',
          isShow: true
        },
        component: UnifiedView,
        props: { type: 4 } // 传递type参数表示奖项视图
      },
      {
        path: '/project',
        name: 'project',
        meta: {
          "title": '项目',
          isShow: true
        },
        component: UnifiedView,
        props: { type: 5 } // 传递type参数表示项目视图
      },
    ]
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router