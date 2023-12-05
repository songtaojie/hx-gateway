<template>
  <div class="container">
    <Breadcrumb :items="['menu.project', 'menu.project.search']" />
    <a-card class="general-card" :title="$t('menu.project.search')">
      <a-row>
        <a-col :flex="1">
          <a-form :model="queryModel" :label-col-props="{ span: 6 }" :wrapper-col-props="{ span: 18 }" label-align="left">
            <a-row :gutter="16">
              <a-col :span="8">
                <a-form-item field="number" :label="$t('project.name.label')">
                  <a-input v-model="queryModel.searchKey" :placeholder="$t('project.name.placeholder')" />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item field="status" :label="$t('status.name.label')">
                  <a-select v-model="queryModel.status" allow-clear :options="statusOptions" :placeholder="$t('select.options.default')" />
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
              {{ $t('search.query') }}
            </a-button>
            <a-button @click="reset">
              <template #icon>
                <icon-refresh />
              </template>
              {{ $t('search.reset') }}
            </a-button>
          </a-space>
        </a-col>
      </a-row>
      <a-divider style="margin-top: 0" />
      <a-row style="margin-bottom: 16px" :gutter="16">
        <a-col>
          <a-space>
            <a-button type="primary" @click="handleEdit(true, undefined)">
              <template #icon>
                <icon-plus />
              </template>
              {{ $t('project.name.table.operation') }}
            </a-button>
            <!-- <a-button type="outline" @click="syncProjects">
              <template #icon>
                <icon-sync />
              </template>
              {{ $t('consul.form.sync') }}
            </a-button> -->
          </a-space>
        </a-col>
        <a-col :span="6"></a-col>
      </a-row>
    </a-card>

    <a-card class="general-card" :title="$t('menu.project.search')">
      <a-table row-key="id" :loading="loading" :pagination="pagination" :data="renderData" :bordered="false" @page-change="onPageChange">
        <template #columns>
          <a-table-column :title="$t('project.name.table')" data-index="name" align="center" />
          <a-table-column :title="$t('project.sort.index.table')" data-index="sortIndex" align="center" />
          <a-table-column :title="$t('project.createdTime.table')" data-index="createTime" align="center" />

          <a-table-column :title="$t('project.status.table')" data-index="status" align="center">
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
          <a-table-column :title="$t('operations.table')" data-index="operations" align="center">
            <template #cell="{ record }">
              <a-space>
                <a-button type="primary" :status="record.status === 1 ? 'warning' : 'success'" size="small" @click="patchProjectEnabled(record.id, !(record.status === 1))">
                  {{ $t(`${record.status === 1 ? 'radio.disabled.label' : 'radio.enabled.label'}`) }}
                </a-button>
                <a-button
                  type="outline"
                  size="small"
                  @click="
                    handleEdit(false, {
                      code: record.code,
                      name: record.name,
                      id: record.id,
                      status: record.status,
                      sortIndex: record.sortIndex
                    })
                  "
                >
                  {{ $t('edit.operations.table') }}
                </a-button>
                <a-button type="primary" size="small" @click="toRoutePageSearch(record.id)">
                  {{ $t('project.set.route.operations.table') }}
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
    <a-modal v-model:visible="visible" title-align="start" :title="$t(`${isCreate ? 'project.create.modal' : 'project.update.modal'}`)" :mask-closable="false" @cancel="handleCancel" :on-before-ok="handleOk">
      <a-form :model="form" ref="formRef">
        <a-form-item field="code" :label="$t('project.code.label')" :rules="[{ required: true, message: $t('project.form.code.required.errmsg') }]" :validate-trigger="['change', 'blur']">
          <a-input v-model="form.code" :placeholder="$t('project.code.placeholder')" />
        </a-form-item>
        <a-form-item field="name" :label="$t('project.name.label')" :rules="[{ required: true, message: $t('project.form.name.required.errmsg') }]" :validate-trigger="['change', 'blur']">
          <a-input v-model="form.name" :placeholder="$t('project.name.placeholder')" />
        </a-form-item>
        <a-form-item field="sortIndex" :label="$t('project.sort.index.label')">
          <a-input-number v-model="form.sortIndex" :placeholder="$t('project.sort.index.placeholder')" />
        </a-form-item>
        <a-form-item field="status" :label="$t('project.status.label')">
          <a-switch v-model="form.status" :checked-value="1" :unchecked-value="2" checked-text="启用" unchecked-text="禁用" />
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive } from 'vue'
import { useI18n } from 'vue-i18n'
import useLoading from '@/hooks/loading'
import { getPage, patchProject, addProject, updateProject, EditProjectModel, CreateEditProjectModel, ProjectResponse, PageProjectRequest, deleteProject } from '@/api/project'
import { Pagination } from '@/types/global'
import type { SelectOptionData } from '@arco-design/web-vue/es/select/interface'
import { Message, Modal } from '@arco-design/web-vue'
import { useRouter } from 'vue-router'
import { useRouteStore } from '@/store'

const router = useRouter()
const formRef = ref()
const visible = ref(false)
const isCreate = ref(true)
const form = ref<EditProjectModel>(CreateEditProjectModel())

const handleCancel = () => {
  visible.value = false
}
const handleOk = async (): Promise<boolean> => {
  var result = await new Promise<boolean>((resolve) => {
    formRef.value &&
      formRef.value.validate(async (r: any, v: any) => {
        if (r == void 0) {
          visible.value = false
          if (isCreate.value) {
            await addProject(form.value)
          } else {
            await updateProject(form.value)
          }
          const msg = t('detailForm.submitSuccess')
          Message.success({
            content: msg,
            duration: 5 * 1000
          })
          fetchData()
        } else {
          resolve(false)
        }
      })
  })
  return result
}
const handleEdit = (useCreate: boolean, editProjectParams: EditProjectModel | undefined) => {
  if (useCreate) {
    form.value = CreateEditProjectModel()
  } else {
    form.value = editProjectParams as EditProjectModel
  }
  visible.value = true
  isCreate.value = useCreate
}
const handleDel = (projectId: number) => {
  Modal.warning({
    title: 'Warning Notification',
    content: 'This is a warning description which directly indicates a warning that might need attention.',
    okText: t('confirm'),
    cancelText: t('cancel'),
    hideCancel: false,
    onOk: () => {
      deleteProject(projectId)
      const msg = t('detailForm.submitSuccess')
      Message.success({
        content: msg,
        duration: 5 * 1000
      })
      fetchData()
    }
  })
}
const generateQueryModel = () => {
  return {
    searchKey: '',
    status: undefined,
    orderIndex: undefined,
    createTime: []
  }
}
const { loading, setLoading } = useLoading(true)
const { t } = useI18n()
const renderData = ref<ProjectResponse[]>([])
const queryModel = ref(generateQueryModel())
const basePagination: Pagination = {
  page: 1,
  pageSize: 10
}
const pagination = reactive({
  ...basePagination
})
const statusOptions = computed<SelectOptionData[]>(() => [
  {
    label: t('status.enabled.label'),
    value: 1
  },
  {
    label: t('status.disabled.label'),
    value: 2
  }
])
const fetchData = async (
  params: PageProjectRequest = {
    page: 1,
    pageSize: 10,
    status: undefined,
    field: undefined,
    order: undefined
  }
) => {
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
  } as unknown as PageProjectRequest)
}

const onPageChange = (page: number) => {
  basePagination.page = page
  fetchData({
    ...basePagination,
    ...queryModel.value
  } as unknown as PageProjectRequest)
}

fetchData()
const reset = () => {
  queryModel.value = generateQueryModel()
}

const patchProjectEnabled = async (projectId: number, enabled: boolean) => {
  await patchProject(projectId, enabled ? 1 : 2)
  const msg = t('detailForm.submitSuccess')
  Message.success({
    content: msg,
    duration: 5 * 1000
  })
  fetchData()
}

// const formatDate = (value: any) => {
//   const date = new Date(value)
//   const y = date.getFullYear()
//   const MM = date.getMonth() + 1
//   const MMstr = MM < 10 ? `0${MM}` : MM
//   const d = date.getDate()
//   const dstr = d < 10 ? `0${d}` : d
//   const h = date.getHours()
//   const hstr = h < 10 ? `0${h}` : h
//   const m = date.getMinutes()
//   const mstr = m < 10 ? `0${m}` : m
//   const s = date.getSeconds()
//   const sstr = s < 10 ? `0${s}` : s
//   return `${y}-${MMstr}-${dstr} ${hstr}:${mstr}:${sstr}`
// }

const toRoutePageSearch = (projectId: number) => {
  const routeStore = useRouteStore()
  routeStore.toggleProjectId(projectId)
  router.push({
    name: 'SearchRoute'
  })
}
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
