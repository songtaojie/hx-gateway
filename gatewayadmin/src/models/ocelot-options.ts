export class AuthenticationOptions {
  authenticationProviderKey: string | undefined // 当Ocelot运行时，它会查看这个AuthenticationProviderKey并检查是否有一个使用给定密钥注册的身份验证提供程序。
  AllowedScopes: Array<string> | undefined // 允许的范围
}
export class LoadBalancerOptions {
  type: string | undefined // 负载均衡方式（LeastConnection，RoundRobin，NoLoadBalance）
  key: string | undefined // 负载均衡关键字
  expiry: number | undefined //负载均衡结束时间
}

export class HttpHandlerOptions {
  allowAutoRedirect: boolean | undefined // 允许自动跳转
  useCookieContainer: boolean | undefined // 使用Cookie容器
  useTracing: boolean | undefined // 使用链路追踪
  useProxy: boolean | undefined // 使用代理
  maxConnectionsPerServer: number | undefined // 这将控制内部HttpClient将打开多少个连接。这可以在Route或全局级别设置。
}

export class QoSOptions {
  enabled: boolean | undefined // 流量调控开启
  exceptionsAllowedBeforeBreaking: number | undefined // 打开断路器之前允许的例外数量,要实现此规则，必须对ExceptionsAllowedBeforeBreaking设置一个大于0的数字
  durationOfBreak: number | undefined // 断路器复位之前打开的时间（ms）
  timeoutValue: number | undefined // 请求超时时间（ms）
}

export class RateLimitOptions {
  enableRateLimiting: boolean | undefined // 此值指定启用端点速率限制
  clientWhitelistStr: string | undefined
  clientWhitelist: Array<string> | undefined // 这是一个包含客户端白名单的数组。这意味着该数组中的客户端将不受速率限制的影响
  period: number | undefined //如果在此时间段内发出的请求超过限制，则需要等待PeriodTimespan结束后再发出另一个请求
  periodTimespan: number | undefined // 这个值指定我们可以在特定的秒数后重试
  limit: number | undefined // 此值指定客户端在指定时间段内可以发出的最大请求数
  SetcCientWhitelist() {
    if (this.clientWhitelistStr != undefined && this.clientWhitelistStr != '') {
      this.clientWhitelist = this.clientWhitelistStr.split(/,|，/)
    } else {
      this.clientWhitelist = []
    }
  }
}

export class ServiceDiscoveryProviderOptions {
  enabled: boolean | undefined // 是否启用
  scheme: string | undefined // Http协议（http,https）
  host: string | undefined //主机
  port: number | undefined // 端口
  type: string | undefined // 此值指定客户端在指定时间段内可以发出的最大请求数
  token: string | undefined // 如果您正在使用ACL与Consul Ocelot支持添加X-Consul-Token头。为了使其工作，您必须添加下面的附加属性
  // Ocelot将把这个令牌添加到Consul客户端，用于发出请求，然后用于每个请求
  configurationKey: string | undefined //配置key
  pollingInterval: number | undefined //轮询间隔以毫秒为单位，告诉Ocelot多久调用Consul更改一次服务配置
  namespace: string | undefined // 命名空间
}

export class FileCacheOptions {
  ttlSeconds: number | undefined // 如设置为15，这意味着缓存将在xx秒后过期
  region: string | undefined
}

// 下游服务的主机和端口
export class DownstreamHostAndPortOptions {
  host: string | undefined // 主机
  port: number | undefined // 端口
  routeId: number | undefined // 路由id
}
