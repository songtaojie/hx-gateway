import axios from 'axios'
import qs from 'query-string'
import { PageResponseModel, BasePageRequest, StatusEnum } from '@/models/common'
import { RouteModel, PageRouteModel } from '../models/route'
import { AuthenticationOptions, LoadBalancerOptions, HttpHandlerOptions, QoSOptions, RateLimitOptions, ServiceDiscoveryProviderOptions, DownstreamHostAndPortOptions, FileCacheOptions } from '../models/ocelot-options'

export interface PageRouteInput extends BasePageRequest {
  status: StatusEnum | undefined
  projectId: string | undefined
  downstreamPathTemplate: string | undefined
  upstreamPathTemplate: string | undefined
  requestIdKey: string | undefined
}

export function genPageRouteInput() {
  return {
    projectId: undefined,
    downstreamPathTemplate: undefined,
    upstreamPathTemplate: undefined,
    status: undefined,
    requestIdKey: undefined
  }
}
// 设置默认值
const defaultRouteModel: RouteModel = {
  id: undefined,
  projectId: undefined,
  downstreamPathTemplate: '/{url}',
  upstreamPathTemplate: '/{url}',
  upstreamHttpMethod: ['Get', 'Post', 'Put', 'Patch', 'Delete', 'Option'],
  downstreamHttpMethod: undefined,
  downstreamHttpVersion: '1.1',
  upstreamHost: undefined,
  requestIdKey: undefined,
  routeIsCaseSensitive: false, // 开启上下游路由模板大小写匹配
  useServiceDiscovery: false, // 是否使用服务发现
  serviceName: undefined, // 服务名
  serviceNamespace: undefined, // 如果您的下游服务位于不同的名称空间中，您可以通过指定ServiceNamespace来覆盖Route级别的全局设置
  downstreamScheme: undefined, //下游请求的协议，如：http,htttps
  downstreamHostAndPorts: [],
  qoSOptions: new QoSOptions(),
  loadBalancerOptions: new LoadBalancerOptions(),
  rateLimitOptions: new RateLimitOptions(),
  authenticationOptions: new AuthenticationOptions(),
  dangerousAcceptAnyServerCertificateValidator: false, // 如果要忽略SSL警告/错误，请设置为true
  httpHandlerOptions: new HttpHandlerOptions(), // HttpHandler配置
  delegatingHandlers: [], //委托处理程序
  fileCacheOptions: new FileCacheOptions(),
  serviceDiscoveryProviderOptions: new ServiceDiscoveryProviderOptions(),
  priority: undefined,
  sort: undefined,
  status: StatusEnum.Enable
}

export function createRouteModel(config: Partial<RouteModel> = {}): RouteModel {
  return {
    ...defaultRouteModel,
    ...config
  }
}

export function getPage(params: PageRouteInput) {
  return axios.get<PageResponseModel<PageRouteModel>>('/api/route/getPage', {
    params,
    paramsSerializer: (obj) => {
      return qs.stringify(obj)
    }
  })
}

export interface RoutePropertieModel {
  id: string
  key: string
  value: string
  routeId: string
  type: number
}

// 查询路由具体信息
export function getDetail(routeId: string) {
  return axios.get<RouteModel>(`/api/route/getDetail/${routeId}`)
}

export function addRoute(data: RouteModel) {
  return axios.post('/api/route/add', data)
}

export function updateRoute(data: RouteModel) {
  return axios.put('/api/route/update', data)
}

// 启用或禁用路由
export function updateStatus(id: string, status: StatusEnum) {
  return axios.patch(`/api/route/updateStatus`, {
    id,
    status
  })
}

// 删除路由
export function deleteRoute(id: string) {
  return axios.delete(`/api/route/delete`, {
    data: { id }
  })
}

// 获取预览
export function getRoutePreview() {
  return axios.get<string>('/api/route/GetRoutePreview')
}
