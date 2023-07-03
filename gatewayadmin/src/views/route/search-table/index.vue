<template>
  <div class="container">
    <Breadcrumb :items="['menu.project', 'menu.route.search']" />
    <a-card class="general-card" :title="$t('menu.route.search')">
      <a-row>
        <a-col :flex="1">
          <a-form :model="formModel" :label-col-props="{ span: 6 }" :wrapper-col-props="{ span: 18 }" label-align="left">
            <a-row :gutter="16">
              <a-col :span="8">
                <a-form-item field="downstreamPathTemplate" :label="$t('downstream.path.template')">
                  <a-input v-model="formModel.downstreamPathTemplate" :placeholder="$t('routeTable.placeholder.downstream.path.template')" />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item field="upstreamPathTemplate" :label="$t('upstream.path.template')">
                  <a-input v-model="formModel.upstreamPathTemplate" :placeholder="$t('routeTable.placeholder.upstream.path.template')" />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item field="RequestIdKey" :label="$t('route.requestIdKey')">
                  <a-input v-model="formModel.requestIdKey" :placeholder="$t('routeTable.placeholder.route.requestIdKey')" />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item field="status" :label="$t('routeTable.form.status')">
                  <a-select v-model="formModel.enabled" allow-clear :options="statusOptions" :placeholder="$t('routeTable.form.selectDefault')" />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </a-col>
        <a-divider style="height: 84px" direction="vertical" />
        <a-col :flex="'86px'" style="text-align: right">
          <a-space direction="vertical" :size="18">
            <a-button type="primary" @click="search">
              <template #icon>
                <icon-search />
              </template>
              {{ $t('routeTable.form.search') }}
            </a-button>
            <a-button @click="reset">
              <template #icon>
                <icon-refresh />
              </template>
              {{ $t('routeTable.form.reset') }}
            </a-button>
          </a-space>
        </a-col>
      </a-row>
      <a-divider style="margin-top: 0" />
      <a-row style="margin-bottom: 16px">
        <a-col :span="16">
          <a-space>
            <a-button type="primary" @click="toEditRoutePage(undefined)">
              <template #icon>
                <icon-plus />
              </template>
              {{ $t('routeTable.operation.create') }}
            </a-button>
          </a-space>
        </a-col>
      </a-row>
      <a-table row-key="id" :loading="loading" :pagination="pagination" :data="renderData" :bordered="false" @page-change="onPageChange">
        <template #columns>
          <a-table-column :title="$t('downstream.path.template')" data-index="downstreamPathTemplate" />
          <a-table-column :title="$t('downstream.http.version')" data-index="downstreamHttpVersion" />
          <a-table-column :title="$t('downstream.scheme')" data-index="downstreamScheme" />
          <a-table-column :title="$t('upstream.path.template')" data-index="upstreamPathTemplate" />
          <a-table-column :title="$t('upstream.http.method')" data-index="upstreamHttpMethod" />
          <a-table-column :title="$t('route.requestIdKey')" data-index="requestIdKey" />
          <a-table-column :title="$t('downstream.http.method')" data-index="downstreamHttpMethod" />
          <a-table-column :title="$t('routeTable.columns.sort')" data-index="sort" />
          <a-table-column :title="$t('routeTable.columns.status')" data-index="status">
            <template #cell="{ record }">
              <a-tag :color="record.status === 1 ? 'blue' : 'orange'">
                <template #icon>
                  <icon-check v-if="record.status === 1" />
                  <icon-close v-else />
                </template>
                {{ $t(`${record.status === 1 ? 'radio.enabled.label' : 'radio.disabled.label'}`) }}
              </a-tag>
            </template>
          </a-table-column>
          <a-table-column :title="$t('operations.table')" data-index="operations">
            <template #cell="{ record }">
              <a-space>
                <a-button type="primary" :status="record.status === 1 ? 'warning' : 'success'" size="small" @click="patchRouteStatus(record.id, record.status === 1 ? StatusEnum.Disable : StatusEnum.Enable)">
                  {{ $t(`${record.status === 1 ? 'radio.disabled.label' : 'radio.enabled.label'}`) }}
                </a-button>
                <a-button type="outline" size="small" @click="toEditRoutePage(record.id)">
                  {{ $t('edit.operations.table') }}
                </a-button>
                <a-button type="primary" size="small" status="danger" @click="handleDel(record.id)">
                  {{ $t('del.operations.table') }}
                </a-button>
              </a-space>
            </template>
          </a-table-column>
        </template>
      </a-table>
    </a-card>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive } from 'vue'
import { useI18n } from 'vue-i18n'
import useLoading from '@/hooks/loading'
import { getPageRoute, patchRoute, PageQueryRouteParams, deleteRoute } from '@/api/route'
import { Pagination } from '@/models/global'
import { StatusEnum } from '@/models/common'
import { PageRouteModel } from '/@/models/route'
import type { SelectOptionData } from '@arco-design/web-vue/es/select/interface'
import { Message, Modal } from '@arco-design/web-vue'
import { useRouteStore } from '@/store'
import { useRouter } from 'vue-router'

const router = useRouter()
const routeStore = useRouteStore()
const projectId = computed(() => routeStore.getProjectIdCache)

const generateFormModel = () => {
  return {
    downstreamPathTemplate: '',
    upstreamPathTemplate: '',
    requestIdKey: '',
    enabled: undefined,
    projectId: projectId.value
  }
}
const { loading, setLoading } = useLoading(true)
const { t } = useI18n()
const renderData = ref<PageRouteModel[]>([])
const formModel = ref(generateFormModel())
const basePagination: Pagination = {
  page: 1,
  pageSize: 10
}
const pagination = reactive({
  ...basePagination
})
const statusOptions = computed<SelectOptionData[]>(() => [
  {
    label: t('radio.enabled.label'),
    value: 1
  },
  {
    label: t('radio.disabled.label'),
    value: 0
  }
])
const fetchData = async (
  params: PageQueryRouteParams = {
    page: 1,
    pageSize: 10,
    enabled: undefined,
    projectId: projectId.value,
    downstreamPathTemplate: undefined,
    upstreamPathTemplate: undefined,
    requestIdKey: undefined,
    field: undefined,
    order: undefined
  }
) => {
  setLoading(true)
  try {
    const { data } = await getPageRoute(params)
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
  } as unknown as PageQueryRouteParams)
}
const onPageChange = (current: number) => {
  basePagination.page = current
  fetchData({
    ...basePagination,
    ...formModel.value
  } as unknown as PageQueryRouteParams)
}

fetchData()
const reset = () => {
  formModel.value = generateFormModel()
}

// 启用或禁用路由
const patchRouteStatus = async (routeId: number, status: StatusEnum) => {
  await patchRoute(routeId, status)
  Message.success({
    content: t('submit.success'),
    duration: 5 * 1000
  })
  fetchData()
}

// 创建或编辑路由
const toEditRoutePage = (routeId: number | undefined) => {
  const thisRouteState = useRouteStore()
  thisRouteState.toggleRouteId(routeId ?? 0)
  router.push({
    name: 'EditRoute'
  })
}
// 删除路由
const handleDel = (routeId: number) => {
  Modal.warning({
    title: 'Warning Notification',
    content: 'This is a warning description which directly indicates a warning that might need attention.',
    okText: t('confirm'),
    cancelText: t('cancel'),
    hideCancel: false,
    onOk: async () => {
      await deleteRoute(routeId)
      const msg = t('detailForm.submitSuccess')
      fetchData()
      Message.success({
        content: msg,
        duration: 5 * 1000
      })
    }
  })
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
