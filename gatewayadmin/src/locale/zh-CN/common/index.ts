const radio = {
  'radio.enabled.label': '启用',
  'radio.disabled.label': '禁用',
  'radio.use.label': '使用',
  'radio.not.use.label': '不使用',
  'radio.allow.label': '允许',
  'radio.not.allow.label': '不允许',
  'radio.label': '启用/禁用'
}
const status = {
  'label.status': '状态',
  'label.status.enabled': '启用',
  'label.status.disabled': '禁用'
}

const select = {
  'select.options.default': '全部'
}
const search = {
  'search.query': '查询',
  'search.reset': '重置'
}

const table = {
  'table.operations': '操作',
  'table.operations.add': '新建',
  'table.operations.edit': '编辑',
  'table.operations.del': '删除',
  'table.column.sort.index': '排序',
  'table.column.status': '状态',
  'table.column.createtime': '创建时间',
  'table.column.updatetime': '更新时间'
}

const modalOperations = {
  'modal.title': ({ named }: { named: any }) => `${named('op') === 1 ? '警告' : '提示'}`,
  'modal.delete.content': '确定删除当前数据?'
}
const formOperations = {
  'submit.success': '提交成功',
  'submit.fail': '提交失败',
  'submit.update.success': '更新成功',
  'submit.update.fail': '更新失败'
}
export default {
  'unit.ms': '毫秒',
  'unit.second': '秒',
  'navbar.action.locale': '切换为中文',
  'delete.success': '删除成功',
  'delete.fail': '删除失败',
  submit: '提交',
  confirm: '确认',
  exit: '退出',
  cancel: '取消',
  ...formOperations,
  ...radio,
  ...status,
  ...select,
  ...search,
  ...table,
  ...modalOperations
}
