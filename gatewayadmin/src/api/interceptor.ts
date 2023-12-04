import axios from 'axios'
import type { AxiosRequestConfig, AxiosResponse } from 'axios'
import { Message, Modal } from '@arco-design/web-vue'
import { useUserStore } from '@/store'
import { getToken, getRefreshToken, clearToken, setToken } from '@/utils/auth'
import { isString, isObject } from 'lodash'

const accessTokenKey = 'access-token'
const refreshAccessTokenKey = `x-${accessTokenKey}`

export interface HttpResponse<T = unknown> {
  data: T
  extras: object | null
  statusCode: number
  succeeded: boolean
  timestamp: number
  message: object | string | null
}

if (import.meta.env.VITE_API_BASE_URL) {
  axios.defaults.baseURL = import.meta.env.VITE_API_BASE_URL
}

axios.interceptors.request.use(
  (config: AxiosRequestConfig) => {
    // let each request carry token
    // this example using the JWT token
    // Authorization is a custom headers key
    // please modify it according to the actual situation
    const token = getToken()
    const refreshToken = getRefreshToken()
    if (token) {
      if (!config.headers) {
        config.headers = {}
      }
      config.headers.Authorization = `Bearer ${token}`
      config.headers['X-Authorization'] = `Bearer ${refreshToken}`
    }
    return config
  },
  (error) => {
    // do something
    return Promise.reject(error)
  }
)

// 清除 token
export const clearAccessTokens = () => {
  clearToken()

  // 刷新浏览器
  window.location.reload()
}
// add response interceptors
axios.interceptors.response.use(
  (response: AxiosResponse<HttpResponse>) => {
    // 获取状态码和返回数据
    const status = response.status
    // 处理 401
    if (status === 401) {
      clearAccessTokens()
    }
    // 读取响应报文头 token 信息
    const accessToken = response.headers[accessTokenKey]
    const refreshAccessToken = response.headers[refreshAccessTokenKey]
    // 判断是否是无效 token
    if (accessToken === 'invalid_token') {
      clearAccessTokens()
    }
    // 判断是否存在刷新 token，如果存在则存储在本地
    else if (refreshAccessToken && accessToken && accessToken !== 'invalid_token') {
      setToken(accessToken, refreshAccessToken)
    }
    const res = response.data
    if (res.statusCode === 401) {
      clearAccessTokens()
    } else if (res.statusCode === undefined) {
      return Promise.resolve(res)
    } else if (res.statusCode !== 200) {
      let errorMsg = ''
      if (isString(res.message)) {
        errorMsg = res.message
      } else if (isObject(res.message)) {
        errorMsg = JSON.stringify(res.message) || 'Error'
      }
      Message.error({
        content: errorMsg,
        duration: 5 * 1000
      })
      // 50008: Illegal token; 50012: Other clients logged in; 50014: Token expired;
      if ([50008, 50012, 50014].includes(res.statusCode) && response.config.url !== '/api/user/info') {
        Modal.error({
          title: 'Confirm logout',
          content: 'You have been logged out, you can cancel to stay on this page, or log in again',
          okText: 'Re-Login',
          async onOk() {
            const userStore = useUserStore()

            await userStore.logout()
            window.location.reload()
          }
        })
      }
      return Promise.reject(new Error(errorMsg))
    }
    return res
  },
  (error) => {
    const errorObj = JSON.parse(JSON.stringify(error))
    if (errorObj.status === 401) {
      Modal.error({
        title: 'Confirm logout',
        content: 'You have been logged out, you can cancel to stay on this page, or log in again',
        okText: 'Re-Login',
        async onOk() {
          const userStore = useUserStore()
          await userStore.logout()
          window.location.reload()
        }
      })
    } else {
      Message.error({
        content: error.message || 'Request Error',
        duration: 5 * 1000
      })
    }
    return Promise.reject(error)
  }
)
