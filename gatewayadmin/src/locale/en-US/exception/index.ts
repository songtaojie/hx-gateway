const ex403 = {
  'menu.exception.403': '403',
  'exception.result.403.description': 'Access to this resource on the server is denied.',
  'exception.result.403.back': 'Back'
}
const ex404 = {
  'menu.exception.404': '404',
  'exception.result.404.description': 'Whoops, this page is gone.',
  'exception.result.404.retry': 'Retry',
  'exception.result.404.back': 'Back'
}

const ex500 = {
  'menu.exception.500': '500',
  'exception.result.500.description': 'Internal server error',
  'exception.result.500.back': 'Back'
}

export default {
  ...ex403,
  ...ex404,
  ...ex500
}
