import { defineStore } from 'pinia'
import { setProjectId, getProjectId, setRouteId, getRouteId } from '@/utils/project-route-cache'
import { RouteState } from './types'

const useRouteStore = defineStore('route', {
  state: (): RouteState => ({
    projectId: undefined,
    routeId: undefined
  }),

  getters: {
    getProjectIdCache(): number {
      if (this.projectId) {
        return this.projectId
      }
      return getProjectId() as unknown as number
    },
    getRouteIdCache(): number {
      if (this.routeId) {
        return this.routeId
      }
      return getRouteId() as unknown as number
    }
  },

  actions: {
    toggleProjectId(value: number) {
      this.projectId = value
      setProjectId(value)
    },
    toggleRouteId(value: number) {
      this.routeId = value
      setRouteId(value)
    }
  }
})

export default useRouteStore
