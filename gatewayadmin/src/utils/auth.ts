const TOKEN_KEY = 'accessToken'
const REFRESH_TOKEN_KEY = 'refreshToken'
const FULL_NAME_KEY = 'fullName'

const isLogin = () => {
  return !!localStorage.getItem(TOKEN_KEY)
}

const getToken = () => {
  return localStorage.getItem(TOKEN_KEY)
}

const getRefreshToken = () => {
  return localStorage.getItem(REFRESH_TOKEN_KEY)
}

const getFullName = () => {
  return localStorage.getItem(FULL_NAME_KEY)
}

const setToken = (accessToken: string, refreshToken: string) => {
  localStorage.setItem(TOKEN_KEY, accessToken)
  localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken)
}

const setFullName = (fullName: string) => {
  localStorage.setItem(FULL_NAME_KEY, fullName)
}

const clearToken = () => {
  localStorage.removeItem(TOKEN_KEY)
  localStorage.removeItem(REFRESH_TOKEN_KEY)
  localStorage.removeItem(FULL_NAME_KEY)
}

export { isLogin, getToken, setToken, clearToken, getFullName, getRefreshToken, setFullName }
