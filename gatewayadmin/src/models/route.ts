import { AuthenticationOptions, LoadBalancerOptions, HttpHandlerOptions, QoSOptions, RateLimitOptions, ServiceDiscoveryProviderOptions, DownstreamHostAndPortOptions, FileCacheOptions } from './ocelot-options'
import { StatusEnum } from './common'

export interface RouteModel {
  id: number | undefined
  projectId: number | undefined
  downstreamPathTemplate: string | undefined // 下游的路由模板，即真实处理请求的路径模板
  upstreamPathTemplate: string | undefined // 上游请求的模板，即用户真实请求的链接
  upstreamHttpMethod: string[] | undefined // 上游请求的http方法（数组：GET、POST、PUT）
  downstreamHttpMethod: string | undefined // 下游请求的http方法
  downstreamHttpVersion: string | undefined // 下游Http版本
  upstreamHost: string | undefined // 上游主机
  requestIdKey: string | undefined // 请求Id
  routeIsCaseSensitive: boolean | undefined // 开启上下游路由模板大小写匹配
  useServiceDiscovery: boolean | undefined
  serviceName: string | undefined // 服务名
  serviceNamespace: string | undefined // 如果您的下游服务位于不同的名称空间中，您可以通过指定ServiceNamespace来覆盖Route级别的全局设置
  downstreamScheme: string | undefined //下游请求的协议，如：http,htttps
  downstreamHostAndPorts: DownstreamHostAndPortOptions[] | undefined
  qoSOptions: QoSOptions | undefined
  loadBalancerOptions: LoadBalancerOptions | undefined
  rateLimitOptions: RateLimitOptions | undefined
  authenticationOptions: AuthenticationOptions | undefined
  dangerousAcceptAnyServerCertificateValidator: boolean | undefined // 如果要忽略SSL警告/错误，请设置为true
  httpHandlerOptions: HttpHandlerOptions | undefined // HttpHandler配置
  delegatingHandlers: string[] //委托处理程序
  fileCacheOptions: FileCacheOptions | undefined
  serviceDiscoveryProviderOptions: ServiceDiscoveryProviderOptions | undefined
  priority: number | undefined // 你可以通过在ocelot.json中包含“Priority”属性来定义你想要的路由匹配Upstream HttpRequest的顺序,0是最低优先级
  sort: number | undefined // 排序
  status: StatusEnum | undefined // 状态
}

export interface PageRouteModel {
  id: number | undefined
  projectId: number | undefined
  downstreamPathTemplate: string | undefined // 下游的路由模板，即真实处理请求的路径模板
  upstreamPathTemplate: string | undefined // 上游请求的模板，即用户真实请求的链接
  upstreamHttpMethod: string[] | undefined // 上游请求的http方法（数组：GET、POST、PUT）
  downstreamHttpMethod: string | undefined // 下游请求的http方法
  downstreamHttpVersion: string | undefined // 下游Http版本
  upstreamHost: string | undefined // 上游主机
  requestIdKey: string | undefined // 请求Id
  routeIsCaseSensitive: boolean | undefined // 开启上下游路由模板大小写匹配
  useServiceDiscovery: boolean | undefined
  serviceName: string | undefined // 服务名
  serviceNamespace: string | undefined // 如果您的下游服务位于不同的名称空间中，您可以通过指定ServiceNamespace来覆盖Route级别的全局设置
  downstreamScheme: string | undefined //下游请求的协议，如：http,htttps
  sort: number | undefined // 排序
  status: StatusEnum | undefined // 状态
  status_V: string | undefined
}
