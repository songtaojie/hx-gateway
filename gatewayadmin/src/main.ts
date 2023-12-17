import { createApp } from 'vue'
import ArcoVue from '@arco-design/web-vue'
import ArcoVueIcon from '@arco-design/web-vue/es/icon'
import globalComponents from './components/index'
import JsonViewer from 'vue-json-viewer'
import router from './router'
import store from './store'
import i18n from './locale'
import directive from './directive'
import App from './App.vue'
import '@arco-design/web-vue/dist/arco.css'
import '@/api/interceptor'
import '@/theme/index.less'

const app = createApp(App)
app.use(ArcoVue, {})
app.use(ArcoVueIcon)

app.use(router)
app.use(store)
app.use(i18n)
app.use(globalComponents)
app.use(directive)
app.use(JsonViewer)
app.mount('#app')
