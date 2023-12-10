const create = {
  'route.host.port': 'Host Port',
  'route.title.basic': 'Route Basic Settings',
  'route.requestIdKey': 'Request Id Key',
  'route.service.name': 'Service Name',
  'route.service.namespace': 'Service Namespace',
  'route.port': 'Port',
  'route.cacheTtlseconds': 'Cache Ttl Seconds',
  'route.routeIsCaseSensitive': 'Is Case Sensitive',
  'route.cache.region': 'Cache Region',
  'route.dangerousAcceptAnyServerCertificateValidator': 'Dangerous Accept Any Server Certificate Validator',
  'route.delegating.handlers': 'Delegating Handlers'
}
const table = {
  'route.downstream.scheme': 'Downstream Scheme',
  'route.downstream.http.version': 'Downstream Http Version',
  'route.downstream.http.method': 'Downstream Http Method',
  'route.downstream.path.template': 'Downstream Path Template',
  'route.upstream.http.method': 'Upstream Http Method',
  'route.upstream.path.template': 'Upstream Path Template',
  'route.upstream.host': 'Upstream Host',
  'authentication.manager': 'Authentication',
  'authentication.provider.key': 'Authentication Provider Key',
  'authentication.allowed.scopes': 'Authentication Allowed Scopes',
  'authentication.add.scope': 'Add'
}
export default {
  'menu.route.search': 'Search Routes',
  'routeTable.form.search': 'Search',
  'routeTable.form.reset': 'Reset',

  'routeTable.placeholder.downstream.path.template': 'Please Input Downstream Path Template',
  'routeTable.placeholder.upstream.path.template': 'Please Input Upstream Path Template',
  'routeTable.placeholder.route.requestIdKey': 'Please Input RequestIdKey',

  'routeTable.columns.status': 'Status',
  'routeTable.columns.operations': 'Operations',
  'routeTable.columns.operations.view': 'View',
  'routeTable.columns.operations.edit': 'Edit',
  'routeTable.columns.operations.del': 'Delete',
  'routeTable.operation.create.route': 'Create Route',
  'routeTable.operation.update.route': 'Update Route',
  'routeTable.columns.sort': 'Sort',
  ...create,
  ...table
}
