import { DEFAULT_LAYOUT } from '../base'
import { AppRouteRecordRaw } from '../types'

const PROJECT: AppRouteRecordRaw = {
  path: '/project',
  name: 'project',
  component: DEFAULT_LAYOUT,
  meta: {
    locale: 'menu.project',
    requiresAuth: false,
    icon: 'icon-list',
    order: 1
  },
  children: [
    {
      path: 'search-project',
      name: 'SearchProject',
      component: () => import('@/views/project/search-table/index.vue'),
      meta: {
        locale: 'menu.project.search',
        requiresAuth: true,
        roles: ['*']
      }
    },
    {
      path: 'search-route',
      name: 'SearchRoute',
      component: () => import('@/views/route/search-table/index.vue'),
      meta: {
        locale: 'menu.route.search',
        requiresAuth: false,
        roles: ['*'],
        hideInMenu: true
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
    },
    {
      path: 'sync-route',
      name: 'SysnRoute',
      component: () => import('@/views/project/sync-content/index.vue'),
      meta: {
        locale: 'menu.project.sync',
        requiresAuth: true,
        roles: ['*']
      }
    }
  ]
}

export default PROJECT
