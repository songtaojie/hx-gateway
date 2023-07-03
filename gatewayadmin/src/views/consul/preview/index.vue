<template>
  <div class="container">
    <Breadcrumb :items="['menu.consul', 'menu.consul.preview']" />
    <a-card class="general-card" :title="$t('menu.consul.preview')">
      <a-row>
        <a-col :flex="1">
          <a-form :model="formModel" :label-col-props="{ span: 6 }" :wrapper-col-props="{ span: 18 }" label-align="left">
            <a-row :gutter="16">
              <a-col :span="8">
                <a-form-item field="key" :label="$t('consul.form.key')">
                  <a-select v-model="formModel.key" allow-clear :options="consulSettingKeyOptions" :placeholder="$t('projectTable.form.selectDefault')" />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item field="dc" :label="$t('consul.form.dc')">
                  <a-select v-model="formModel.dc" allow-clear :options="consulDCOptions" :placeholder="$t('projectTable.form.selectDefault')" />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </a-col>
        <a-divider style="height: 32px" direction="vertical" />
        <a-col :flex="'86px'" style="text-align: right">
          <a-space direction="horizontal" :size="10">
            <a-button type="primary" @click="search">
              <template #icon>
                <icon-search />
              </template>
              {{ $t('projectTable.form.search') }}
            </a-button>
          </a-space>
        </a-col>
      </a-row>
    </a-card>
    <a-divider style="margin-top: 0" />
    <a-card>
      <json-viewer :value="consulContent" :preview-mode="true" copyable />
    </a-card>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import { getConsulKeyValue } from '@/api/consul'
import { getAllSelect } from '@/api/dictionary'
import type { SelectOptionData } from '@arco-design/web-vue/es/select/interface'

const consulContent = ref({})
const generateFormModel = () => {
  return {
    key: '',
    dc: ''
  }
}
const formModel = ref(generateFormModel())

const consulSettingKeyOptions = ref<SelectOptionData[]>([])
const consulDCOptions = ref<SelectOptionData[]>([])
const initSelect = async () => {
  const { data } = await getAllSelect()
  consulSettingKeyOptions.value = data.consulSettingKey
  consulDCOptions.value = data.consulDC
}

initSelect()

const search = async () => {
  const form = formModel.value
  const { data } = await getConsulKeyValue(form.key, form.dc)
  consulContent.value = JSON.parse(data)
}
</script>

<script lang="ts">
export default {
  name: 'ConsulPreview'
}
</script>

<style scoped lang="less">
.container {
  padding: 0 20px 20px 20px;
}
:deep(.arco-table-th) {
  &:last-child {
    .arco-table-th-item-title {
      margin-left: 16px;
    }
  }
}
</style>
