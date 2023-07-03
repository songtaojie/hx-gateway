import localeMessageBox from '@/components/message-box/locale/en-US'
import localeLogin from '@/views/login/locale/en-US'

import localeWorkplace from '@/views/dashboard/workplace/locale/en-US'

import localeSearchProject from '@/views/project/search-table/locale/en-US'
import localeSearchRoute from '@/views/route/search-table/locale/en-US'
import localeCreateRoute from '@/views/route/create-route/locale/en-US'
import localeConsulPreview from '@/views/consul/preview/locale/en-US'
import localeSyncProject from '@/views/project/sync-content/locale/en-US'
import localeConsul from '@/views/consul/search-table/locale/en-US'

import localeSuccess from '@/views/result/success/locale/en-US'
import localeError from '@/views/result/error/locale/en-US'

import locale403 from '@/views/exception/403/locale/en-US'
import locale404 from '@/views/exception/404/locale/en-US'
import locale500 from '@/views/exception/500/locale/en-US'

import localeSettings from './en-US/settings'

import localeDownstream from './en-US/downstream'
import localeUpstream from './en-US/upstream'
import localeAuthentication from './en-US/authentication'

import localeCommon from './en-US/common'
import localeMenu from './en-US/menu'
import localeglobalconfiguration from './en-US/global-configuration'
import localeProject from './en-US/project'

export default {
  ...localeCommon,
  ...localeMenu,
  ...localeglobalconfiguration,
  ...localeProject,

  ...localeSettings,
  ...localeMessageBox,
  ...localeLogin,
  ...localeWorkplace,

  ...localeSyncProject,
  ...localeSearchProject,
  ...localeSearchRoute,
  ...localeCreateRoute,
  ...localeConsulPreview,
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
