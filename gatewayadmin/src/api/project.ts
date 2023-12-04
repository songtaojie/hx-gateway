import axios from 'axios'
import qs from 'query-string'
import { PageResponseModel, BasePageRequest, StatusEnum } from '../models/common'

export interface ProjectResponse {
  id: number
  code:string
  name: string
  sortIndex: number
  enabled: boolean
  createTime: number
  updateTime: number
}

export interface PageProjectRequest extends BasePageRequest {
  status: StatusEnum | undefined
}

export interface EditProjectModel {
  id: number | undefined // 项目Id
  code:string,
  name: string // 项目名称
  sortIndex: number // 排序字段
}

export function getPage(params: PageProjectRequest) {
  return axios.get<PageResponseModel<ProjectResponse>>('/api/project/getPage', {
    params,
    paramsSerializer: (obj) => {
      return qs.stringify(obj)
    }
  })
}

export function addProject(data: EditProjectModel) {
  return axios.post('/api/project/add', data)
}

export function updateProject(data: EditProjectModel) {
  return axios.put('/api/project/Update', data)
}

// 启用或禁用项目
export function patchProject(projectId: number, status: number) {
  return axios.patch(`/api/project/PatchProject/${projectId}/${status}`)
}

// 删除项目
export function deleteProject(projectId: number) {
  return axios.delete(`/api/project/project/${projectId}`)
}
