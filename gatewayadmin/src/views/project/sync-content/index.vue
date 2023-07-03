<template>
  <div class="container">
    <Breadcrumb :items="['menu.project', 'menu.project.sync']" />
    <a-card class="general-card" :title="$t('menu.project.sync')">
      <div class="frame-bg">
        <div class="frame-body">
          <div class="frame-aside">
            <a-steps :current="current" direction="vertical">
              <a-step>路由预览</a-step>
              <a-step>同步路由</a-step>
            </a-steps>
          </div>
          <div class="frame-main">
            <div v-if="current === 1">
              <json-viewer :value="consulContent" :expand-depth="2" :preview-mode="true" copyable />
            </div>
            <div v-if="current === 2">
              <a-card style="border: none">
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
                      <a-button type="outline" @click="sysnToConsul">
                        <template #icon>
                          <icon-sync />
                        </template>
                        {{ $t('consul.form.sync') }}
                      </a-button>
                    </a-space>
                  </a-col>
                </a-row>
              </a-card>
              <a-card style="border: none">
                <ul v-for="(item, index) in messages" :key="index">
                  <li v-if="!isJSON(item)">{{ item }}</li>
                  <json-viewer v-else :value="JSON.parse(item)" :preview-mode="true" copyable boxed />
                </ul>
              </a-card>
            </div>
            <div class="main-bottom">
              <a-button :disabled="current === 1" @click="onPrev">
                <icon-left />
                {{ $t('back') }}
              </a-button>
              <a-button :disabled="current === 2" @click="onNext">
                {{ $t('next') }}
                <icon-right />
              </a-button>
            </div>
          </div>
        </div>
      </div>
    </a-card>
  </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
import { getAllSelect } from '@/api/dictionary'
import type { SelectOptionData } from '@arco-design/web-vue/es/select/interface'
import { getRoutePreview } from '@/api/route'
import isJSON from '@/utils/string-extend'
import * as signalR from '@microsoft/signalr'
import {} from '@arco-design/web-vue'

const messages = ref<string[]>([])
const current = ref(1)

const onPrev = () => {
  current.value = Math.max(1, current.value - 1)
}

const onNext = () => {
  current.value = Math.min(3, current.value + 1)
}

const consulContent = ref({})
const formModel = ref({ key: '', dc: '' })

const consulSettingKeyOptions = ref<SelectOptionData[]>([])
const consulDCOptions = ref<SelectOptionData[]>([])
const initSelect = async () => {
  const { data } = await getAllSelect()
  consulSettingKeyOptions.value = data.consulSettingKey
  consulDCOptions.value = data.consulDC
  if (data.consulSettingKey !== null && data.consulSettingKey.length > 0) {
    formModel.value.key = data.consulSettingKey[0].value
  }
  if (data.consulDC !== null && data.consulDC.length > 0) {
    formModel.value.dc = data.consulDC[0].value
  }
}

initSelect()

const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${import.meta.env.VITE_API_BASE_URL}/consulhub`, {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  })
  .withAutomaticReconnect()
  .configureLogging(signalR.LogLevel.Information)
  .build()

connection.on('SysnToConsulMessage', (message) => {
  messages.value.push(message)
})

connection.start()

const sysnToConsul = () => {
  messages.value = []
  if (connection.state !== 'Connected') {
    connection.start()
  }
  connection.invoke('SysnToConsulAsync', formModel.value.key, formModel.value.dc)
}

const preview = async () => {
  const { data } = await getRoutePreview()
  consulContent.value = JSON.parse(data)
}
preview()
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
.frame-bg {
  padding: 10px;
}

.frame-body {
  display: flex;
  background: var(--color-bg-2);
}

.frame-aside {
  padding: 24px;
  height: 272px;
  border-right: 1px solid var(--color-border);
}

.frame-main {
  width: 100%;
}

.main-bottom {
  display: flex;
  justify-content: center;

  button {
    margin: 0 20px;
  }
}
</style>
