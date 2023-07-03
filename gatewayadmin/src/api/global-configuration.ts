import axios from 'axios'

import { LoadBalancerOptions, HttpHandlerOptions, QoSOptions, RateLimitOptions, ServiceDiscoveryProviderOptions } from '../models/ocelot-options'

export interface GlobalConfigurationModel {
  id: number | undefined // 主键Id
  baseUrl: string // 基础地址
  requestIdKey: string | undefined // 请求ID
  downstreamScheme: string | undefined // 请求的方式（http,https）
  downstreamHttpVersion: string | undefined // Http版本（1.0，1.1，2.0）
  loadBalancerOptions: LoadBalancerOptions | undefined //
  httpHandlerOptions: HttpHandlerOptions | undefined
  qoSOptions: QoSOptions | undefined
  rateLimitOptions: RateLimitOptions | undefined

  // ratelimitQuotaexceededmessage: string | undefined // 超过限制提示语
  // ratelimitRatelimitcounterprefix: string | undefined // 计数前缀
  // ratelimitDisableratelimitheaders: boolean | undefined // 包含X-Rate-Limit和Rety-After
  // ratelimitHttpstatuscode: number | undefined // 超过限制Http状态码
  // ratelimitClientidheader: string | undefined // 客户Header
  serviceDiscoveryProviderOptions: ServiceDiscoveryProviderOptions | undefined
  status: number | undefined // 配置启动
}

// 新增全局配置
export function addGlobalConfiguration(data: GlobalConfigurationModel) {
  return axios.post<number>('/api/globalconfiguration/addGlobalConfiguration', data)
}

// 编辑全局配置
export function updateGlobalConfiguration(data: GlobalConfigurationModel) {
  return axios.put<string>('/api/globalconfiguration/updateGlobalConfiguration', data)
}

// 查询全局配置信息
export function getGlobalConfiguration() {
  return axios.get<GlobalConfigurationModel>('/api/globalconfiguration/getGlobalConfiguration')
}
