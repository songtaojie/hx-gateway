import axios from 'axios'

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
  return axios.post<LoginRes>('/api/system/login', data)
}
