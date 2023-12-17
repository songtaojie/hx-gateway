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
      component: () => import('@/views/project/index.vue'),
      meta: {
        locale: 'menu.project.search',
        requiresAuth: true,
        roles: ['*']
      }
    }
  ]
}

export default PROJECT
