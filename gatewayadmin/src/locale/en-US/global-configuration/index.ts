// 基础参数语言配置
const basic = {
  'gc.basic.title': 'Basic Parameters',
  'gc.basic.baseUrl.label': 'Base Url',
  'gc.basic.requestIdKey.label': 'Request Id',

  'gc.basic.baseUrl.placeholder': 'Please Input Base Url',
  'gc.basic.requestIdKey.placeholder': 'Please Input Request Id'
}

// 下游协议语言配置
const downstream = {
  'gc.downstream.title': 'Downstream Scheme',
  'gc.downstream.scheme.label': 'Downstream Scheme',
  'gc.downstream.httpVersion.label': 'Http Version',

  'gc.downstream.scheme.placeholder': 'Please Select Downstream Scheme',
  'gc.downstream.httpVersion.placeholder': 'Please Select Http Version'
}

// 负载均衡语言配置
const loadbalancer = {
  'gc.loadbalancer.title': 'Load Balancer',
  'gc.loadbalancer.type.label': 'Load Balancer Type',
  'gc.loadbalancer.type.least.connection.label': 'Least Connection',
  'gc.loadbalancer.type.round.robin.label': 'Round Robin',
  'gc.loadbalancer.type.no.load.balance.label': 'No Load Balance',
  'gc.loadbalancer.type.cookie.sticky.sessions.label': 'Cookie Sticky Sessions',
  'gc.loadbalancer.key.label': 'Balancer Key',
  'gc.loadbalancer.expiry.label': 'Balancer Expiry',

  'gc.loadbalancer.type.placeholder': 'Please Input Load Balancer Type',
  'gc.loadbalancer.key.placeholder': 'Please Input Balancer Key',
  'gc.loadbalancer.expiry.placeholder': 'Please Input Balancer Expiry'
}

// 服务发现语言配置
const serviceDiscovery = {
  'gc.service.discovery.title': 'Service Discovery',
  'gc.service.discovery.scheme.label': 'Http Version',
  'gc.service.discovery.host.label': 'Host',
  'gc.service.discovery.port.label': 'Port',
  'gc.service.discovery.type.label': 'Type',
  'gc.service.discovery.token.label': 'Token',
  'gc.service.discovery.configuration.key.label': 'Configuration Key',
  'gc.service.discovery.namespace.label': 'Namespace',
  'gc.service.discovery.polling.interval.label': 'Polling Interval',
  'gc.service.discovery.name.label': 'Name',

  'gc.service.discovery.scheme.placeholder': 'Please Input Http Version',
  'gc.service.discovery.host.placeholder': 'Please Input Host',
  'gc.service.discovery.port.placeholder': 'Please Input Port',
  'gc.service.discovery.type.placeholder': 'Please Input Type',
  'gc.service.discovery.token.placeholder': 'Please Input Token',
  'gc.service.discovery.configuration.key.placeholder': 'Please Input Configuration Key',
  'gc.service.discovery.namespace.placeholder': 'Please Input Namespace',
  'gc.service.discovery.polling.interval.placeholder': 'Please Input Polling Interval',
  'gc.service.discovery.name.placeholder': 'Please Input Name'
}

// qos语言配置
const qos = {
  'gc.qos.title': 'Qos Options',
  'gc.qos.exceptions.allowed.before.breaking.label': 'Exceptions Allowed Before Breaking',
  'gc.qos.duration.of.break.label': 'Duration Of Break',
  'gc.qos.timeout.value.label': 'Timeout Value',

  'gc.qos.exceptions.allowed.before.breaking.placeholder': 'Please Input Allowed Before Breaking',
  'gc.qos.duration.of.break.placeholder': 'Please Input Duration Of Break',
  'gc.qos.timeout.value.placeholder': 'Please Input Timeout Value'
}

// rateLimit语言配置
const rateLimit = {
  'gc.rateLimit.title': 'Rate Limit',
  'gc.rateLimit.period.label': 'Period',
  'gc.rateLimit.periodTimespan.label': 'Period Timespan',
  'gc.rateLimit.limit.label': 'Limit',
  'gc.rateLimit.clientWhitelist.label': 'Client Whitelist',

  'gc.rateLimit.period.placeholder': 'Please Input Period',
  'gc.rateLimit.periodTimespan.placeholder': 'Please Input Period Timespan',
  'gc.rateLimit.limit.placeholder': 'Please Input Limit',
  'gc.rateLimit.clientWhitelist.placeholder': 'Please Input Client Whitelist'
}

// httpHandler语言配置
const httpHandler = {
  'gc.httpHandler.title': 'Http Handler',
  'gc.httpHandler.allow.auto.redirect.label': 'Allow Auto Rdirect',
  'gc.httpHandler.use.cookie.container.label': 'Use Cookie Container',
  'gc.httpHandler.use.tracing.label': 'Use Tracing',
  'gc.httpHandler.use.proxy.label': 'Use Proxy',
  'gc.httpHandler.max.connections.per.server.label': 'Max Connections Per Server',

  'gc.httpHandler.max.connections.per.server.placeholder': 'Please Input Max Connections Per Server'
}

export default {
  ...basic,
  ...downstream,
  ...loadbalancer,
  ...serviceDiscovery,
  ...qos,
  ...rateLimit,
  ...httpHandler
}
