import { DEFAULT_LAYOUT } from '../base'
import { AppRouteRecordRaw } from '../types'

const PROJECT: AppRouteRecordRaw = {
  path: '/route',
  name: 'route',
  component: DEFAULT_LAYOUT,
  meta: {
    locale: 'menu.route',
    requiresAuth: false,
    icon: 'icon-list',
    order: 3
  },
  children: [
    {
      path: 'search-route',
      name: 'SearchRoute',
      component: () => import('@/views/route/search-table/index.vue'),
      meta: {
        locale: 'menu.route.search',
        requiresAuth: false,
        roles: ['*']
      }
    },
    {
      path: 'edit-route',
      name: 'EditRoute',
      component: () => import('@/views/route/create-route/index.vue'),
      meta: {
        locale: 'menu.route.edit',
        requiresAuth: false,
        roles: ['*'],
        hideInMenu: true
      }
    }
  ]
}

export default PROJECT
