import { DEFAULT_LAYOUT } from '../base'
import { AppRouteRecordRaw } from '../types'

const GLOBALCONFIG: AppRouteRecordRaw = {
  path: '/globalconfiguration',
  name: 'globalconfiguration',
  component: DEFAULT_LAYOUT,
  meta: {
    locale: 'menu.globalconfiguration',
    icon: 'icon-settings',
    requiresAuth: false,
    order: 2
  },
  children: [
    {
      path: 'detail',
      name: 'Detail',
      component: () => import('@/views/global-configuration/detail/index.vue'),
      meta: {
        locale: 'menu.globalconfiguration.detail',
        requiresAuth: true,
        roles: ['*']
      }
    }
  ]
}

export default GLOBALCONFIG
