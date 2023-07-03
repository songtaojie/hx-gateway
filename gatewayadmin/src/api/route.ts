import axios from 'axios'
import qs from 'query-string'
import { PageResponseModel, BasePageRequest, StatusEnum } from '@/models/common'
import { RouteModel, PageRouteModel } from '../models/route'

export interface PageQueryRouteParams extends BasePageRequest {
  enabled: boolean | undefined
  projectId: number | undefined
  downstreamPathTemplate: string | undefined
  upstreamPathTemplate: string | undefined
  requestIdKey: string | undefined
}

export function getPageRoute(params: PageQueryRouteParams) {
  return axios.get<PageResponseModel<PageRouteModel>>('/api/route/GetPageRoute', {
    params,
    paramsSerializer: (obj) => {
      return qs.stringify(obj)
    }
  })
}

export interface RoutePropertieModel {
  id: number
  key: string
  value: string
  routeId: number
  type: number
}

// 查询路由具体信息
export function getRoute(routeId: number) {
  return axios.get<RouteModel>(`/api/route/getRoute/${routeId}`)
}

export function addRoute(data: RouteModel) {
  return axios.post('/api/route/addRoute', data)
}

export function updateRoute(data: RouteModel) {
  return axios.put('/api/route/updateRoute', data)
}

// 启用或禁用路由
export function patchRoute(routeId: number, status: StatusEnum) {
  return axios.patch(`/api/route/patchRouteStatus/${routeId}/${status}`)
}

// 删除路由
export function deleteRoute(routeId: number) {
  return axios.delete(`/api/route/route/${routeId}`)
}

// 获取预览
export function getRoutePreview() {
  return axios.get<string>('/api/route/GetRoutePreview')
}
