import axios from 'axios'
import { BaseSelectModel } from '@/models/common'

export interface AllSelectResponse {
  consulSettingKey: BaseSelectModel<string>[]
  consulDC: BaseSelectModel<string>[]
}

// 获取所有下拉框
export function getAllSelect() {
  return axios.get<AllSelectResponse>('/api/dict/getAllSelect')
}
