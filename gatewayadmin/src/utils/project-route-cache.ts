const PROJECT_ID_KEY = 'ProjectIdKey'
const ROUTE_ID_KEY = 'RouteIdKey'

const setProjectId = (projectId: number) => {
  sessionStorage.setItem(PROJECT_ID_KEY, projectId.toString())
}

const getProjectId = () => {
  const cache = sessionStorage.getItem(PROJECT_ID_KEY)
  if (cache) {
    return cache
  }
  return '0'
}

const setRouteId = (routeId: number) => {
  sessionStorage.setItem(ROUTE_ID_KEY, routeId.toString())
}

const getRouteId = () => {
  const cache = sessionStorage.getItem(ROUTE_ID_KEY)
  if (cache) {
    return cache
  }
  return '0'
}

export { setProjectId, getProjectId, setRouteId, getRouteId }
