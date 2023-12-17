import { BasePageRequest, PageResponseModel } from '@/models/common'
import qs from 'query-string'
import axios from 'axios'

export interface SyncProjectModel {
  key: string
  dc: string
}

export interface PageSettingBakModel extends BasePageRequest {
  consulKey: string | undefined
  consulDc: string | undefined
  bakTime: string | undefined
}

export interface SettingBakModel {
  id: number
  bakTime: string
  bakJson: string
  remark: string
  consulKey: string
  consulDc: string
}

// 获取所有下拉框
export function getConsulKeyValue(key: string, dc: string) {
  key = key.replace('/', '%2F')
  return axios.get<string>(`/api/consul/consul-key-value/${key}/${dc}`)
}

export interface EditProjectModel {
  id: number // 项目Id
  projectName: string // 项目名称
  orderIndex: number // 排序字段
}

export function sysnToConsul(data: SyncProjectModel) {
  return axios.post('/api/consul/sysn-to-consul', data)
}

export function getPageSettingBak(params: PageSettingBakModel) {
  return axios.get<PageResponseModel<SettingBakModel>>('/api/consul/page-setting-baks', {
    params,
    paramsSerializer: (obj) => {
      return qs.stringify(obj)
    }
  })
}

export function rollback(id: number) {
  return axios.post<string>(`/api/consul/roll-back/${id}`)
}
