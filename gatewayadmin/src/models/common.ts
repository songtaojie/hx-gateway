// 分页列表数据
export interface PageResponseModel<T> {
  page: number
  pageSize: number
  total: number
  totalPages: number
  items: T[]
  hasPrevPage: boolean
  hasNextPage: boolean
}

//分页请求数据
export interface BasePageRequest {
  page: number
  pageSize: number
  field: string | undefined // 排序字段
  order: string | undefined // 排序方向
}

export interface BaseSelectModel<T> {
  value: T
  label: string
}

export enum StatusEnum {
  //启用
  Enable = 1,
  Disable = 2 //禁用
}
