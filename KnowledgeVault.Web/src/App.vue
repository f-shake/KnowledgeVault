<template>
    <el-config-provider :locale="zhCn">
      <router-view />
    </el-config-provider>
</template>

<script lang="ts" setup>
import { ElConfigProvider } from "element-plus";
import zhCn from 'element-plus/es/locale/lang/zh-cn';

// 解决 ResizeObserver loop completed with undelivered notifications. 问题
const debounce = (callback: (...args: any[]) => void, delay: number) => {
  let tid: any;
  return function (...args: any[]) {
    const ctx = self;
    tid && clearTimeout(tid);
    tid = setTimeout(() => {
      callback.apply(ctx, args);
    }, delay);
  };
};

const _ = (window as any).ResizeObserver;
(window as any).ResizeObserver = class ResizeObserver extends _ {
  constructor(callback: (...args: any[]) => void) {
    callback = debounce(callback, 20);
    super(callback);
  }
};
</script>


<style lang="scss">
* {
  padding: 0;
  margin: 0;
}

html,
body,
#app {
  width: 100%;
  height: 100%;
}
</style>
