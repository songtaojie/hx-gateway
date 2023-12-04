import { defineStore } from 'pinia'
import { login as userLogin, LoginData } from '@/api/user'
import { setToken, clearToken, getFullName, setFullName } from '@/utils/auth'
import { removeRouteListener } from '@/utils/route-listener'
import startImage from '@/assets/images/start.8e0e4855ee346a46ccff8ff3e24db27b.png'
import { UserState } from './types'
import useAppStore from '../app'

const useUserStore = defineStore('user', {
  state: (): UserState => ({
    name: undefined,
    avatar: undefined,
    job: undefined,
    organization: undefined,
    location: undefined,
    email: undefined,
    introduction: undefined,
    personalWebsite: undefined,
    jobName: undefined,
    organizationName: undefined,
    locationName: undefined,
    phone: undefined,
    registrationDate: undefined,
    accountId: undefined,
    certification: undefined,
    role: ''
  }),

  getters: {
    userInfo(state: UserState): UserState {
      return { ...state }
    }
  },

  actions: {
    switchRoles() {
      return new Promise((resolve) => {
        this.role = this.role === 'user' ? 'admin' : 'user'
        resolve(this.role)
      })
    },
    // Set user's information
    setInfo(partial: Partial<UserState>) {
      this.$patch(partial)
    },

    // Reset user's information
    resetInfo() {
      this.$reset()
    },

    // Get user's information
    async info() {
      const data: UserState = {
        name: getFullName() || '',
        avatar: startImage,
        role: 'admin'
      }

      this.setInfo(data)
    },

    // Login
    async login(loginForm: LoginData) {
      try {
        const res = await userLogin(loginForm)
        console.log(res, 'login')
        setToken(res.data.accessToken, res.data.refreshToken)
        setFullName(res.data.fullName)
      } catch (err) {
        clearToken()
        throw err
      }
    },
    logoutCallBack() {
      const appStore = useAppStore()
      this.resetInfo()
      clearToken()
      removeRouteListener()
      appStore.clearServerMenu()
    },
    // Logout
    async logout() {
      this.logoutCallBack()
    }
  }
})

export default useUserStore
