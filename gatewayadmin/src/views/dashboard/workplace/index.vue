<template>
  <div class="container">
    <div class="left-side">
      <div class="panel">
        <Banner />
        <DataPanel />
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import Banner from './components/banner.vue'
import DataPanel from './components/data-panel.vue'
</script>

<script lang="ts">
export default {
  name: 'Dashboard' // If you want the include property of keep-alive to take effect, you must name the component
}
</script>

<style lang="less" scoped>
.container {
  background-color: var(--color-fill-2);
  padding: 16px 20px;
  padding-bottom: 0;
  display: flex;
}

.left-side {
  flex: 1;
  overflow: auto;
}

.panel {
  background-color: var(--color-bg-2);
  border-radius: 4px;
  overflow: auto;
}
:deep(.panel-border) {
  margin-bottom: 0;
  border-bottom: 1px solid rgb(var(--gray-2));
}
</style>
