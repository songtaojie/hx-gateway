<template>
  <div class="container">
    <Breadcrumb :items="['menu.project', 'menu.project.search']" />
    <a-card class="general-card" :title="$t('menu.project.search')">
      <a-row>
        <a-col :flex="1">
          <a-form :model="queryModel" :label-col-props="{ span: 6 }" :wrapper-col-props="{ span: 18 }" label-align="left">
            <a-row :gutter="16">
              <a-col :span="8">
                <a-form-item field="number" :label="$t('project.label.name')">
                  <a-input v-model="queryModel.searchKey" :placeholder="$t('project.name.placeholder')" />
                </a-form-item>
              </a-col>
              <a-col :span="8">
                <a-form-item field="status" :label="$t('label.status')">
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

    <a-card class="general-card">
      <a-table row-key="id" :loading="loading" :pagination="pagination" :data="renderData" :bordered="false" @page-change="onPageChange">
        <template #columns>
          <a-table-column :title="$t('project.code.table')" data-index="code" align="center" />
          <a-table-column :title="$t('project.name.table')" data-index="name" align="center" />
          <a-table-column :title="$t('table.column.sort.index')" data-index="sortIndex" align="center" />
          <a-table-column :title="$t('table.column.createtime')" data-index="createTime" align="center" />

          <a-table-column :title="$t('table.column.status')" data-index="status" align="center">
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
          <a-table-column :title="$t('table.operations')" data-index="operations" align="center">
            <template #cell="{ record }">
              <a-space>
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
                  {{ $t('table.operations.edit') }}
                </a-button>
                <a-button type="primary" size="small" status="danger" @click="handleDel(record.id)">
                  {{ $t('table.operations.del') }}
                </a-button>
              </a-space>
            </template>
          </a-table-column>
        </template>
      </a-table>
    </a-card>
    <a-modal v-model:visible="visible" title-align="start" :title="$t(`${isCreate ? 'project.create.modal' : 'project.update.modal'}`)" :mask-closable="false" @cancel="handleCancel" :on-before-ok="handleOk">
      <a-form :model="form" ref="formRef">
        <a-form-item field="code" :label="$t('project.label.code')" :rules="[{ required: true, message: $t('project.form.code.required.errmsg') }]" :validate-trigger="['blur']">
          <a-input v-model="form.code" :placeholder="$t('project.code.placeholder')" />
        </a-form-item>
        <a-form-item field="name" :label="$t('project.label.name')" :rules="[{ required: true, message: $t('project.form.name.required.errmsg') }]" :validate-trigger="['blur']">
          <a-input v-model="form.name" :placeholder="$t('project.name.placeholder')" />
        </a-form-item>
        <a-form-item field="sortIndex" :label="$t('project.label.sort.index')">
          <a-input-number v-model="form.sortIndex" :placeholder="$t('project.sort.index.placeholder')" />
        </a-form-item>
        <a-form-item field="status" :label="$t('label.status')">
          <a-switch v-model="form.status" :checked-value="1" :unchecked-value="2">
            <template #checked>启用</template>
            <template #unchecked>禁用</template>
          </a-switch>
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script lang="ts" setup>
import { computed, ref, reactive } from 'vue'
import { useI18n } from 'vue-i18n'
import useLoading from '@/hooks/loading'
import { getPage, addProject, updateProject, EditProjectModel, CreateEditProjectModel, ProjectResponse, PageProjectRequest, deleteProject } from '@/api/project'
import { Pagination } from '@/models/global'
import type { SelectOptionData } from '@arco-design/web-vue/es/select/interface'
import { Message, Modal } from '@arco-design/web-vue'
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
          const msg = t('submit.success')
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
const handleDel = (projectId: string) => {
  Modal.warning({
    titleAlign: 'start',
    title: t('modal.delete.title'),
    content: t('modal.delete.content'),
    okText: t('confirm'),
    cancelText: t('cancel'),
    hideCancel: false,
    onOk: async () => {
      var res = await deleteProject(projectId)
      if (res && res.data) {
        Message.success({
          content: t('delete.success'),
          duration: 5 * 1000
        })
        fetchData()
      }
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
    label: t('label.status.enabled'),
    value: 1
  },
  {
    label: t('label.status.disabled'),
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
