import localeMessageBox from '@/components/message-box/locale/en-US'
import localeLogin from '@/views/login/locale/en-US'

import localeWorkplace from '@/views/dashboard/workplace/locale/en-US'

import localeConsulPreview from '@/views/consul/preview/locale/en-US'
import localeConsul from '@/views/consul/search-table/locale/en-US'

import localeSuccess from '@/views/result/success/locale/en-US'
import localeError from '@/views/result/error/locale/en-US'

import localeEx from './en-US/exception'

import localeSettings from './en-US/settings'

import localeCommon from './en-US/common'
import localeMenu from './en-US/menu'
import localeglobalconfiguration from './en-US/global-configuration'
import localeProject from './en-US/project'
import localeRoute from './en-US/route'
export default {
  ...localeCommon,
  ...localeMenu,
  ...localeglobalconfiguration,
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
