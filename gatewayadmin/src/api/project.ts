import axios from 'axios'
import qs from 'query-string'
import { PageResponseModel, BasePageRequest, StatusEnum } from '../models/common'
const sysConfig = window['sysConfig'] || {}
const apiPrefix = sysConfig.apiPrefix || 'api'
export interface ProjectResponse {
  id: string | undefined
  code: string | undefined
  name: string | undefined
  sortIndex: number
  status: StatusEnum | undefined
  createTime: number
  updateTime: number
}

export interface PageProjectRequest extends BasePageRequest {
  status: StatusEnum | undefined
}

export interface EditProjectModel {
  id: string | undefined // 项目Id
  code: string
  name: string // 项目名称
  status: StatusEnum | undefined
  sortIndex: number // 排序字段
}

export function CreateEditProjectModel(): EditProjectModel {
  return {
    id: undefined,
    code: '',
    name: '',
    status: StatusEnum.Disable,
    sortIndex: 0
  }
}

export function getPage(params: PageProjectRequest) {
  return axios.get<PageResponseModel<ProjectResponse>>(`/${apiPrefix}/project/getPage`, {
    params,
    paramsSerializer: (obj) => {
      return qs.stringify(obj)
    }
  })
}

export function getList() {
  return axios.get<Array<ProjectResponse>>(`/${apiPrefix}/project/getList`)
}

export function addProject(data: EditProjectModel) {
  return axios.post(`/${apiPrefix}/project/add`, data)
}

export function updateProject(data: EditProjectModel) {
  return axios.put(`/${apiPrefix}/project/Update`, data)
}

// 启用或禁用项目
export function patchProject(projectId: number, status: number) {
  return axios.patch(`/${apiPrefix}/project/PatchProject/${projectId}/${status}`)
}

// 删除项目
export function deleteProject(projectId: string) {
  return axios.delete(`/${apiPrefix}/project/delete`, { data: { id: projectId } })
}
