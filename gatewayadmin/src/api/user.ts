import axios from 'axios'
const sysConfig = window['sysConfig'] || {}
const apiPrefix = sysConfig.apiPrefix || 'api'
export interface LoginData {
  account: string
  password: string
}

export interface LoginRes {
  accessToken: string
  refreshToken: string
  fullName: string
  account: string
}
export function login(data: LoginData) {
  return axios.post<LoginRes>(`/${apiPrefix}/system/login`, data)
}
