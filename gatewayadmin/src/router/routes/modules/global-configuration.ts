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
      path: 'search-global',
      name: 'global-search',
      component: () => import('@/views/global-configuration/search/index.vue'),
      meta: {
        locale: 'menu.globalconfiguration.search',
        requiresAuth: true,
        roles: ['*']
      }
    }
  ]
}

export default GLOBALCONFIG
