import localeMessageBox from '@/components/message-box/locale/zh-CN'
import localeLogin from '@/views/login/locale/zh-CN'

import localeWorkplace from '@/views/dashboard/workplace/locale/zh-CN'

import localeConsulPreview from '@/views/consul/preview/locale/zh-CN'
import localeConsul from '@/views/consul/search-table/locale/zh-CN'

import localeSuccess from '@/views/result/success/locale/zh-CN'
import localeError from '@/views/result/error/locale/zh-CN'

import localeEx from './zh-CN/exception'

import localeSettings from './zh-CN/settings'

import localeCommon from './zh-CN/common'
import localeMenu from './zh-CN/menu'
import localeGlobalconfiguration from './zh-CN/global-configuration'
import localeProject from './zh-CN/project'
import localeRoute from './zh-CN/route'
export default {
  ...localeCommon,
  ...localeMenu,
  ...localeGlobalconfiguration,
  ...localeProject,

  ...localeSettings,
  ...localeMessageBox,
  ...localeLogin,
  ...localeWorkplace,

  ...localeConsulPreview,
  ...localeConsul,

  ...localeSuccess,
  ...localeError,

  ...localeEx,
  ...localeRoute
}
