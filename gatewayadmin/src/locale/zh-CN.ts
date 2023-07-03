import localeMessageBox from '@/components/message-box/locale/zh-CN'
import localeLogin from '@/views/login/locale/zh-CN'

import localeWorkplace from '@/views/dashboard/workplace/locale/zh-CN'

import localeSearchRoute from '@/views/route/search-table/locale/zh-CN'
import localeCreateRoute from '@/views/route/create-route/locale/zh-CN'
import localeConsulPreview from '@/views/consul/preview/locale/zh-CN'
import localeSyncProject from '@/views/project/sync-content/locale/zh-CN'
import localeConsul from '@/views/consul/search-table/locale/zh-CN'

import localeSuccess from '@/views/result/success/locale/zh-CN'
import localeError from '@/views/result/error/locale/zh-CN'

import locale403 from '@/views/exception/403/locale/zh-CN'
import locale404 from '@/views/exception/404/locale/zh-CN'
import locale500 from '@/views/exception/500/locale/zh-CN'

import localeSettings from './zh-CN/settings'

import localeDownstream from './zh-CN/downstream'
import localeUpstream from './zh-CN/upstream'
import localeAuthentication from './zh-CN/authentication'

import localeCommon from './zh-CN/common'
import localeMenu from './zh-CN/menu'
import localeGlobalconfiguration from './zh-CN/global-configuration'
import localeProject from './zh-CN/project'
export default {
  ...localeCommon,
  ...localeMenu,
  ...localeGlobalconfiguration,
  ...localeProject,

  ...localeSettings,
  ...localeMessageBox,
  ...localeLogin,
  ...localeWorkplace,

  ...localeSearchRoute,
  ...localeCreateRoute,
  ...localeConsulPreview,
  ...localeSyncProject,
  ...localeConsul,

  ...localeSuccess,
  ...localeError,
  ...locale403,
  ...locale404,
  ...locale500,

  ...localeDownstream,
  ...localeUpstream,
  ...localeAuthentication
}
