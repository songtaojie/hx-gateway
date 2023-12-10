import { NamedValue } from 'vue-i18n'
// 基础参数语言配置
const basic = {
  'gc.basic.title': '基础参数',
  'gc.basic.name.label': '配置名称',
  'gc.basic.baseUrl.label': '基础域名',
  'gc.basic.requestIdKey.label': '基础请求Id',

  'gc.basic.name.placeholder': '请输入配置名称',
  'gc.basic.baseUrl.placeholder': '请输入基础域名',
  'gc.basic.requestIdKey.placeholder': '请输入基础请求Id',

  'gc.form.name.required.errmsg': '配置名称不能为空',
  'gc.form.requestIdKey.required.errmsg': '基础请求Id不能为空'
}

// 下游协议语言配置
const downstream = {
  'gc.downstream.title': '下游协议',
  'gc.downstream.scheme.label': '下游协议',
  'gc.downstream.httpVersion.label': 'Http版本',

  'gc.downstream.scheme.placeholder': '请选择下游协议',
  'gc.downstream.httpVersion.placeholder': '请选择Http版本'
}

// 负载均衡语言配置
const loadbalancer = {
  'gc.loadbalancer.title': '负载均衡',
  'gc.loadbalancer.type.label': '负载均衡类型',
  'gc.loadbalancer.type.least.connection.label': '最少连接数',
  'gc.loadbalancer.type.round.robin.label': '轮询',
  'gc.loadbalancer.type.no.load.balance.label': '不使用负载均衡',
  'gc.loadbalancer.type.cookie.sticky.sessions.label': 'Cookie粘滞会话',
  'gc.loadbalancer.key.label': '负载均衡Key',
  'gc.loadbalancer.expiry.label': '负载均衡Expiry',

  'gc.loadbalancer.type.placeholder': '请输入负载均衡类型',
  'gc.loadbalancer.key.placeholder': '请输入负载均衡Key',
  'gc.loadbalancer.expiry.placeholder': '请输入负载均衡Expiry'
}

// 服务发现语言配置
const serviceDiscovery = {
  'gc.service.discovery.title': '服务发现',
  'gc.service.discovery.scheme.label': 'Http协议',
  'gc.service.discovery.host.label': '服务发现站点',
  'gc.service.discovery.port.label': '服务发现端口',
  'gc.service.discovery.type.label': '服务发现类型',
  'gc.service.discovery.token.label': 'Token',
  'gc.service.discovery.configuration.key.label': '配置Key',
  'gc.service.discovery.namespace.label': '命名空间',
  'gc.service.discovery.polling.interval.label': '轮询间隔',
  'gc.service.discovery.name.label': '服务发现名称',

  'gc.service.discovery.scheme.placeholder': '请输入Http协议',
  'gc.service.discovery.host.placeholder': '请输入服务发现站点',
  'gc.service.discovery.port.placeholder': '请输入服务发现端口',
  'gc.service.discovery.type.placeholder': '请选择服务发现类型',
  'gc.service.discovery.token.placeholder': '请输入Token',
  'gc.service.discovery.configuration.key.placeholder': '请输入配置Key',
  'gc.service.discovery.namespace.placeholder': '请输入命名空间',
  'gc.service.discovery.polling.interval.placeholder': '请输入轮询间隔',
  'gc.service.discovery.name.placeholder': '请输入服务发现名称'
}

// qos语言配置
const qos = {
  'gc.qos.title': '断路器配置',
  'gc.qos.exceptions.allowed.before.breaking.label': '允许的例外数量',
  'gc.qos.duration.of.break.label': '恢复时间间隔',
  'gc.qos.timeout.value.label': '请求超时时间',

  'gc.qos.exceptions.allowed.before.breaking.placeholder': '请输入允许的例外数量',
  'gc.qos.duration.of.break.placeholder': '请输入恢复时间间隔',
  'gc.qos.timeout.value.placeholder': '请输入请求超时时间'
}

// rateLimit语言配置
const rateLimit = {
  'gc.rateLimit.title': '限流配置',
  'gc.rateLimit.period.label': '限流时间',
  'gc.rateLimit.periodTimespan.label': '重试时间',
  'gc.rateLimit.limit.label': '最大请求数',
  'gc.rateLimit.clientWhitelist.label': '白名单',

  'gc.rateLimit.period.placeholder': '请输入限流时间',
  'gc.rateLimit.periodTimespan.placeholder': '请输入重试时间',
  'gc.rateLimit.limit.placeholder': '请输入最大请求数',
  'gc.rateLimit.clientWhitelist.placeholder': '请输入白名单'
}

// httpHandler语言配置
const httpHandler = {
  'gc.httpHandler.title': 'http处理',
  'gc.httpHandler.allow.auto.redirect.label': '允许自定重定向',
  'gc.httpHandler.use.cookie.container.label': '使用cookie容器',
  'gc.httpHandler.use.tracing.label': '使用追踪',
  'gc.httpHandler.use.proxy.label': '使用代理',
  'gc.httpHandler.max.connections.per.server.label': '服务最大连接数',

  'gc.httpHandler.max.connections.per.server.placeholder': '请输入服务最大连接数'
}
const table = {
  'gc.name.table': '配置名称'
}
const modal = {
  'gc.modal.content': ({ named }: { named: any }) => {
    let desc = '启用'
    const op = named('op')
    if (op === 3) {
      desc = '删除'
    } else if (op === 2) {
      desc = '禁用'
    }
    return `确定${desc}当前配置?`
  }
}
export default {
  ...basic,
  ...downstream,
  ...loadbalancer,
  ...serviceDiscovery,
  ...qos,
  ...rateLimit,
  ...httpHandler,
  ...table,
  ...modal
}
