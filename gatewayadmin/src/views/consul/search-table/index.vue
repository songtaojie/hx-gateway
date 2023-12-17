<template>
  <div class="container">
    <Breadcrumb :items="['menu.consul', 'menu.consul.search']" />
    <a-card class="general-card" :title="$t('menu.consul.search')">
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
              <a-col :span="8">
                <a-form-item field="updateTime" :label="$t('consul.form.bakTime')">
                  <a-date-picker v-model="formModel.bakTime" style="width: 100%" />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </a-col>
        <a-divider style="height: 32px" direction="vertical" />
        <a-col :flex="'86px'" style="text-align: right">
          <a-space direction="horizontal" :size="18">
            <a-button type="primary" @click="search">
              <template #icon>
                <icon-search />
              </template>
              {{ $t('projectTable.form.search') }}
            </a-button>
            <a-button @click="reset">
              <template #icon>
                <icon-refresh />
              </template>
              {{ $t('projectTable.form.reset') }}
            </a-button>
          </a-space>
        </a-col>
      </a-row>
      <a-divider style="margin-top: 0" />
      <a-table row-key="id" :loading="loading" :pagination="pagination" :data="renderData" :bordered="false" @page-change="onPageChange">
        <template #columns>
          <a-table-column :title="$t('consul.bakTime')" data-index="bakTime" />
          <a-table-column :title="$t('consul.consulKey')" data-index="consulKey" />
          <a-table-column :title="$t('consul.consulDc')" data-index="consulDc" />
          <a-table-column :title="$t('consul.bakJson')" ellipsis data-index="bakJson">
            <template #cell="{ record }">
              <a-button @click="handleDetail(record.bakJson)">查看详情</a-button>
            </template>
          </a-table-column>
          <a-table-column :title="$t('consul.remark')" data-index="remark"></a-table-column>
          <a-table-column :title="$t('projectTable.columns.operations')" data-index="operations">
            <template #cell="{ record }">
              <a-space>
                <a-button type="outline" size="small" @click="Rollback(record.id)">
                  {{ $t('consul.columns.operations.rollback') }}
                </a-button>
              </a-space>
            </template>
          </a-table-column>
        </template>
      </a-table>
    </a-card>
    <a-modal v-model:visible="visible" fullscreen :footer="false">
      <template #title></template>
      <json-viewer :value="consulContent" :preview-mode="true" />
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
import { ref, reactive } from 'vue'
import useLoading from '@/hooks/loading'
import { getPageSettingBak, PageSettingBakModel, rollback, SettingBakModel } from '@/api/consul'
import { Pagination } from '@/models/global'
import type { SelectOptionData } from '@arco-design/web-vue/es/select/interface'
import { getAllSelect } from '@/api/dictionary'
import { Message } from '@arco-design/web-vue'

const visible = ref(false)
const consulContent = ref({})
const consulSettingKeyOptions = ref<SelectOptionData[]>([])
const consulDCOptions = ref<SelectOptionData[]>([])
const initSelect = async () => {
  const { data } = await getAllSelect()
  consulSettingKeyOptions.value = data.consulSettingKey
  consulDCOptions.value = data.consulDC
}
initSelect()

const formModel = ref({
  key: undefined,
  dc: undefined,
  bakTime: undefined
})

const { loading, setLoading } = useLoading(true)
const renderData = ref<SettingBakModel[]>([])
const basePagination: Pagination = {
  page: 1,
  pageSize: 10
}
const pagination = reactive({
  ...basePagination
})

const fetchData = async (
  params: PageSettingBakModel = {
    page: 1,
    pageSize: 10,
    field: undefined,
    order: undefined,
    consulDc: undefined,
    consulKey: undefined,
    bakTime: undefined
  }
) => {
  setLoading(true)
  try {
    const { data } = await getPageSettingBak(params)
    renderData.value = data.items
    pagination.page = params.page
    pagination.total = data.total
  } catch (err) {
    // you can report use errorHandler or other
  } finally {
    setLoading(false)
  }
}

const search = () => {
  basePagination.page = 1
  fetchData({
    ...basePagination,
    ...formModel.value
  } as unknown as PageSettingBakModel)
}
const onPageChange = (currentPage: number) => {
  basePagination.page = currentPage
  fetchData({
    ...basePagination,
    ...formModel.value
  } as unknown as PageSettingBakModel)
}

fetchData()
const reset = () => {
  formModel.value = {
    key: undefined,
    dc: undefined,
    bakTime: undefined
  }
}
const handleDetail = (jsonStr: string) => {
  visible.value = true
  consulContent.value = JSON.parse(jsonStr)
}

const Rollback = async (id: number) => {
  const { data } = await rollback(id)
  Message.success(data)
}
</script>

<script lang="ts">
export default {
  name: 'SearchConsul'
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
