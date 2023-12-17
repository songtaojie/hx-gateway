import axios from 'axios'
import { PageResponseModel, StatusEnum, BasePageRequest } from '../models/common'
import { LoadBalancerOptions, HttpHandlerOptions, QoSOptions, RateLimitOptions, ServiceDiscoveryProviderOptions } from '../models/ocelot-options'
const sysConfig = window['sysConfig'] || {}
const apiPrefix = sysConfig.apiPrefix || 'api'
export interface PageGlobalConfigurationRequest extends BasePageRequest {
  projectId: string | undefined
  status: StatusEnum | undefined
}
// 设置默认值
const defaultPageGlobalConfigurationRequest: PageGlobalConfigurationRequest = {
  projectId: undefined,
  status: undefined,
  page: 1,
  pageSize: 10,
  field: undefined,
  order: undefined
}
export function genQueryModel(config: Partial<PageGlobalConfigurationRequest> = {}): PageGlobalConfigurationRequest {
  return {
    ...defaultPageGlobalConfigurationRequest,
    ...config
  }
}

export interface PageGlobalConfigurationResponse {
  id: string
  projectId: string
  projectName: string
  name: string
  status: StatusEnum | undefined
  createTime: string
}

export interface GlobalConfigurationModel {
  id: string | undefined // 主键Id
  projectId: string | undefined
  name: string | undefined // 全局配置名称
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
}

// 设置默认值
const defaultGlobalConfigurationModel: GlobalConfigurationModel = {
  id: undefined, // 主键Id
  projectId: undefined,
  name: undefined,
  baseUrl: undefined, // 基础地址
  requestIdKey: undefined, // 请求ID
  downstreamScheme: undefined, // 请求的方式（http,https）
  downstreamHttpVersion: undefined, // Http版本（1.0，1.1，2.0）
  loadBalancerOptions: new LoadBalancerOptions(), //
  httpHandlerOptions: new HttpHandlerOptions(),
  qoSOptions: new QoSOptions(),
  rateLimitOptions: new RateLimitOptions(),

  serviceDiscoveryProviderOptions: new ServiceDiscoveryProviderOptions()
}

export function getPage(params: PageGlobalConfigurationRequest) {
  return axios.get<PageResponseModel<PageGlobalConfigurationResponse>>(`/${apiPrefix}/globalconfiguration/getPage`, {
    params
  })
}

// 使用对象解构和默认值语法获取参数值
export function createGlobalConfigurationModel(config: Partial<GlobalConfigurationModel> = {}): GlobalConfigurationModel {
  return {
    ...defaultGlobalConfigurationModel,
    ...config
  }
}

// 新增全局配置
export function add(data: GlobalConfigurationModel) {
  return axios.post<boolean>(`/${apiPrefix}/globalconfiguration/add`, data)
}

// 编辑全局配置
export function update(data: GlobalConfigurationModel) {
  return axios.put<boolean>(`/${apiPrefix}/globalconfiguration/update`, data)
}

// 启用或禁用项目
export function updateStatus(id: string, status: StatusEnum) {
  return axios.patch(`/${apiPrefix}/globalconfiguration/UpdateStatus`, {
    id,
    status
  })
}

// 查询全局配置信息
export function getDetail(id: string | undefined | null) {
  return axios.get<GlobalConfigurationModel>(`/${apiPrefix}/globalconfiguration/getDetail/` + id)
}

// 删除项目
export function del(id: string) {
  return axios.delete(`/${apiPrefix}/project/delete`, { data: { id: id } })
}
