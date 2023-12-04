import axios from 'axios'
import { StatusEnum } from '../models/common'
import { LoadBalancerOptions, HttpHandlerOptions, QoSOptions, RateLimitOptions, ServiceDiscoveryProviderOptions } from '../models/ocelot-options'

export interface GlobalConfigModel {
  id: number | undefined // 主键Id
  baseUrl: string | undefined // 基础地址
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
  status: StatusEnum | undefined // 配置启动
}

// 设置默认值
const defaultGlobalConfigModel: GlobalConfigModel = {
  id: undefined, // 主键Id
  baseUrl: undefined, // 基础地址
  requestIdKey: undefined, // 请求ID
  downstreamScheme: undefined, // 请求的方式（http,https）
  downstreamHttpVersion: undefined, // Http版本（1.0，1.1，2.0）
  loadBalancerOptions: new LoadBalancerOptions(), //
  httpHandlerOptions: new HttpHandlerOptions(),
  qoSOptions: new QoSOptions(),
  rateLimitOptions: new RateLimitOptions(),

  serviceDiscoveryProviderOptions: new ServiceDiscoveryProviderOptions(),
  status: StatusEnum.Disable // 配置启动
}

// 使用对象解构和默认值语法获取参数值
export function createGlobalConfigModel(config: Partial<GlobalConfigModel> = {}): GlobalConfigModel {
  return {
    ...defaultGlobalConfigModel,
    ...config
  }
}

// 新增全局配置
export function addGlobalConfig(data: GlobalConfigModel) {
  return axios.post<number>('/api/globalconfiguration/addGlobalConfiguration', data)
}

// 编辑全局配置
export function updateGlobalConfig(data: GlobalConfigModel) {
  return axios.put<string>('/api/globalconfiguration/updateGlobalConfiguration', data)
}

// 查询全局配置信息
export function getGlobalConfig() {
  return axios.get<GlobalConfigModel>('/api/globalconfiguration/getGlobalConfiguration')
}
