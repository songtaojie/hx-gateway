const ex403 = {
  'menu.exception.403': '403',
  'exception.result.403.description': '对不起，您没有访问该资源的权限',
  'exception.result.403.back': '返回'
}
const ex404 = {
  'menu.exception.404': '404',
  'exception.result.404.description': '抱歉，页面不见了～',
  'exception.result.404.retry': '重试',
  'exception.result.404.back': '返回'
}

const ex500 = {
  'menu.exception.500': '500',
  'exception.result.500.description': '抱歉，服务器出了点问题～',
  'exception.result.500.back': '返回'
}

export default {
  ...ex403,
  ...ex404,
  ...ex500
}
