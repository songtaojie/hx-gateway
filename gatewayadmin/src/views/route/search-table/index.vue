<template>
  <div class="container">
    <Breadcrumb :items="['menu.route', 'menu.route.search']" />
    <a-card hoverable>
      <a-form :model="queryModel" layout="horizontal" auto-label-width>
        <a-row :gutter="{ md: 8, lg: 24, xl: 32 }">
          <a-col :sm="24" :md="12" :lg="8" :xl="8" :xxl="6">
            <a-form-item field="downstreamPathTemplate" :label="$t('route.downstream.path.template')">
              <a-input v-model="queryModel.downstreamPathTemplate" :placeholder="$t('routeTable.placeholder.downstream.path.template')" />
            </a-form-item>
          </a-col>
          <a-col :sm="24" :md="12" :lg="8" :xl="8" :xxl="6">
            <a-form-item field="upstreamPathTemplate" :label="$t('route.upstream.path.template')">
              <a-input v-model="queryModel.upstreamPathTemplate" :placeholder="$t('routeTable.placeholder.upstream.path.template')" />
            </a-form-item>
          </a-col>
          <a-col :sm="24" :md="12" :lg="8" :xl="8" :xxl="6">
            <a-form-item field="RequestIdKey" :label="$t('route.requestIdKey')">
              <a-input v-model="queryModel.requestIdKey" :placeholder="$t('routeTable.placeholder.route.requestIdKey')" />
            </a-form-item>
          </a-col>
          <a-col :sm="24" :md="12" :lg="8" :xl="8" :xxl="6">
            <a-form-item field="status" :label="$t('label.status')">
              <a-select v-model="queryModel.status" allow-clear :options="statusOptions" :placeholder="$t('select.options.default')" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="{ md: 8, lg: 24, xl: 32 }">
          <a-col :sm="24" :md="12" :lg="8" :xl="8" :xxl="6">
            <a-space size="medium">
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
              <a-button type="primary" @click="onEditRoute(undefined)">
                <template #icon>
                  <icon-plus />
                </template>
                {{ $t('table.operations.add') }}
              </a-button>
            </a-space>
          </a-col>
        </a-row>
      </a-form>
    </a-card>
    <a-card style="margin-top: 8px" size="medium">
      <a-table row-key="id" size="small" :loading="loading" :pagination="pagination" :data="renderData" column-resizable class="arco-table-border-cell" @page-change="onPageChange">
        <template #columns>
          <a-table-column :title="$t('route.downstream.path.template')" data-index="downstreamPathTemplate" />
          <a-table-column :title="$t('route.downstream.http.version')" data-index="downstreamHttpVersion" />
          <a-table-column :title="$t('route.downstream.scheme')" data-index="downstreamScheme" />
          <a-table-column :title="$t('route.upstream.path.template')" data-index="upstreamPathTemplate" />
          <a-table-column :title="$t('route.upstream.http.method')" data-index="upstreamHttpMethod" />
          <a-table-column :title="$t('route.requestIdKey')" data-index="requestIdKey" />
          <a-table-column :title="$t('route.downstream.http.method')" data-index="downstreamHttpMethod" />
          <a-table-column :title="$t('routeTable.columns.sort')" data-index="sort" />
          <a-table-column :title="$t('routeTable.columns.status')" data-index="status">
            <template #cell="{ record }">
              <a-tag :color="record.status === 1 ? 'blue' : 'orange'">
                {{ $t(`${record.status === 1 ? 'radio.enabled.label' : 'radio.disabled.label'}`) }}
              </a-tag>
            </template>
          </a-table-column>
          <a-table-column :title="$t('table.operations')" data-index="operations">
            <template #cell="{ record }">
              <a-space>
                <a-button type="text" size="mini" :status="record.status === 1 ? 'warning' : 'success'" @click="onupdateStatus(record.id, record.status)">
                  <template #icon>
                    <icon-close v-if="record.status === 1" />
                    <icon-check v-else />
                  </template>
                  {{ $t(`${record.status === 1 ? 'radio.disabled.label' : 'radio.enabled.label'}`) }}
                </a-button>
                <a-button type="text" size="mini" @click="onEditRoute(record.id)">
                  <template #icon><icon-edit /></template>
                  {{ $t('table.operations.edit') }}
                </a-button>
                <a-button type="text" size="mini" status="danger" @click="handleDel(record.id)">
                  <template #icon><icon-delete /></template>
                  {{ $t('table.operations.del') }}
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
import { getPage, genPageRouteInput, updateStatus, PageRouteInput, deleteRoute } from '@/api/route'
import { Pagination } from '@/models/global'
import { PageRouteModel } from '/@/models/route'
import type { SelectOptionData } from '@arco-design/web-vue/es/select/interface'
import { Message, Modal } from '@arco-design/web-vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const { loading, setLoading } = useLoading(true)
const { t } = useI18n()
const renderData = ref<PageRouteModel[]>([])
const queryModel = ref(genPageRouteInput())
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
    value: 2
  }
])
const fetchData = async (params: PageRouteInput = { ...genPageRouteInput(), page: 1, pageSize: 10, field: undefined, order: undefined }) => {
  setLoading(true)
  try {
    const { data } = await getPage(params)
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
    ...queryModel.value
  } as unknown as PageRouteInput)
}
const onPageChange = (current: number) => {
  basePagination.page = current
  fetchData({
    ...basePagination,
    ...queryModel.value
  } as unknown as PageRouteInput)
}

fetchData()
const reset = () => {
  queryModel.value = genPageRouteInput()
  onPageChange(basePagination.page)
}

const onupdateStatus = (id: string, status: number) => {
  var newStatus = status === 1 ? 2 : 1
  Modal.warning({
    titleAlign: 'start',
    title: t('modal.title'),
    content: t('gc.modal.content', { op: newStatus }),
    okText: t('confirm'),
    cancelText: t('cancel'),
    hideCancel: false,
    onOk: async () => {
      var res = await updateStatus(id, newStatus)
      if (res && res.data) {
        Message.success({
          content: t('submit.update.success'),
          duration: 5 * 1000
        })
        fetchData()
      } else {
        Message.error({
          content: t('submit.update.fail'),
          duration: 5 * 1000
        })
      }
    }
  })
}
// 创建或编辑路由
const onEditRoute = (id: string | undefined) => {
  router.push({
    name: 'EditRoute',
    query: { id }
  })
}

const handleDel = (routeId: string) => {
  Modal.warning({
    titleAlign: 'start',
    title: t('modal.title', { op: 1 }),
    content: t('modal.delete.content'),
    okText: t('confirm'),
    cancelText: t('cancel'),
    hideCancel: false,
    onOk: async () => {
      var res = await deleteRoute(routeId)
      if (res && res.data) {
        fetchData()
        Message.success({
          content: t('delete.success'),
          duration: 5 * 1000
        })
      }
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
