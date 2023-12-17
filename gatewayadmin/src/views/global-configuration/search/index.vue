<template>
  <div class="container">
    <Breadcrumb :items="['menu.globalconfiguration', 'menu.globalconfiguration.search']" />
    <a-card hoverable size="medium">
      <a-form :model="queryModel" layout="inline">
        <a-form-item field="projectId" :label="$t('project.label.name')">
          <a-select v-model="queryModel.projectId" allow-clear :options="projectOptions" :field-names="{ value: 'id', label: 'name' }" :placeholder="$t('select.options.default')" />
        </a-form-item>
        <a-form-item field="status" :label="$t('label.status')">
          <a-select v-model="queryModel.status" allow-clear :options="statusOptions" :placeholder="$t('select.options.default')" />
        </a-form-item>
        <a-form-item>
          <a-space size="medium">
            <a-button type="primary" @click="search">
              <template #icon>
                <icon-search />
              </template>
              {{ $t('search.query') }}
            </a-button>
            <a-button @click="reset">
              <template #icon>
                <icon-refresh />
              </template>
              {{ $t('search.reset') }}
            </a-button>
            <a-button type="primary" @click="handleEdit('')">
              <template #icon>
                <icon-plus />
              </template>
              {{ $t('project.name.table.operation') }}
            </a-button>
          </a-space>
        </a-form-item>
      </a-form>
    </a-card>

    <a-card style="margin-top: 8px" size="medium">
      <a-table row-key="id" size="small" :loading="loading" :pagination="pagination" :data="renderData" column-resizable class="arco-table-border-cell" @page-change="onPageChange">
        <template #columns>
          <a-table-column :title="$t('project.name.table')" data-index="projectName" align="center" />
          <a-table-column :title="$t('gc.name.table')" data-index="name" align="center" />
          <a-table-column :title="$t('table.column.status')" data-index="status" align="center">
            <template #cell="{ record }">
              <a-tag :color="record.status === 1 ? 'blue' : 'orange'">
                {{ $t(`${record.status === 1 ? 'radio.enabled.label' : 'radio.disabled.label'}`) }}
              </a-tag>
            </template>
          </a-table-column>
          <a-table-column :title="$t('table.column.createtime')" data-index="createTime" align="center" />

          <a-table-column :title="$t('table.operations')" data-index="operations" align="center">
            <template #cell="{ record }">
              <a-space>
                <a-button type="text" size="mini" :status="record.status === 1 ? 'warning' : 'success'" @click="onupdateStatus(record.id, record.status)">
                  <template #icon>
                    <icon-close v-if="record.status === 1" />
                    <icon-check v-else />
                  </template>
                  {{ $t(`${record.status === 1 ? 'radio.disabled.label' : 'radio.enabled.label'}`) }}
                </a-button>
                <a-button type="text" size="mini" @click="handleEdit(record.id)">
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
import { computed, ref, reactive, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import useLoading from '@/hooks/loading'
import { getList as getProjectList, ProjectResponse } from '@/api/project'
import { getPage, del, genQueryModel, PageGlobalConfigurationRequest, PageGlobalConfigurationResponse, updateStatus } from '@/api/global-configuration'

import { Pagination } from '@/models/global'
import type { SelectOptionData } from '@arco-design/web-vue/es/select/interface'
import { Message, Modal } from '@arco-design/web-vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const projectOptions = ref<ProjectResponse[]>([])
const handleEdit = (id: string) => {
  router.push({ name: 'global-edit', query: { id } })
}
const handleDel = (id: string) => {
  Modal.warning({
    titleAlign: 'start',
    title: t('modal.title', { op: 1 }),
    content: t('gc.modal.contentt', { op: 3 }),
    okText: t('confirm'),
    cancelText: t('cancel'),
    hideCancel: false,
    onOk: async () => {
      var res = await del(id)
      if (res && res.data) {
        Message.success({
          content: t('delete.success'),
          duration: 5 * 1000
        })
        fetchData()
      } else {
        Message.error({
          content: t('delete.fail')
        })
      }
    }
  })
}

const { loading, setLoading } = useLoading(true)
const { t } = useI18n()
const queryModel = ref(genQueryModel())
const renderData = ref<PageGlobalConfigurationResponse[]>([])
const basePagination: Pagination = {
  page: 1,
  pageSize: 10
}
const pagination = reactive({
  ...basePagination
})
const statusOptions = computed<SelectOptionData[]>(() => [
  {
    label: t('label.status.enabled'),
    value: 1
  },
  {
    label: t('label.status.disabled'),
    value: 2
  }
])
const fetchData = async (params: PageGlobalConfigurationRequest = genQueryModel()) => {
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
  } as unknown as PageGlobalConfigurationRequest)
}

const onPageChange = (page: number) => {
  basePagination.page = page
  fetchData({
    ...basePagination,
    ...queryModel.value
  } as unknown as PageGlobalConfigurationRequest)
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
const reset = () => {
  queryModel.value = genQueryModel()
  fetchData(queryModel.value)
}
onMounted(async () => {
  fetchData()
  const { data } = await getProjectList()
  if (data) {
    projectOptions.value = data
  }
})
</script>

<script lang="ts">
export default {
  name: 'SearchProject'
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
