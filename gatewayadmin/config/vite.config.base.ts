import { resolve } from 'path'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueJsx from '@vitejs/plugin-vue-jsx'
import svgLoader from 'vite-svg-loader'

export default defineConfig({
  plugins: [vue(), vueJsx(), svgLoader({ svgoConfig: {} })],
  resolve: {
    // alias: {
    //   '@': resolve(__dirname, '../src'),
    //   '@api': resolve(__dirname, '../src/api'),
    //   '@views': resolve(__dirname, '../src/views'),
    //   '@utils': resolve(__dirname, '../src/utils'),
    //   assets: resolve(__dirname, '../src/assets'),
    //   'vue-i18n': 'vue-i18n/dist/vue-i18n.cjs.js',
    //   vue: 'vue/dist/vue.esm-bundler.js'
    // },
    alias: [
      {
        find: '@',
        replacement: resolve(__dirname, '../src')
      },
      {
        find: 'assets',
        replacement: resolve(__dirname, '../src/assets')
      },
      {
        find: 'vue-i18n',
        replacement: 'vue-i18n/dist/vue-i18n.cjs.js' // Resolve the i18n warning issue
      },
      {
        find: 'vue',
        replacement: 'vue/dist/vue.esm-bundler.js' // compile template
      }
    ],
    extensions: ['.ts', '.js']
  },
  define: {
    'process.env': {}
  },
  css: {
    preprocessorOptions: {
      less: {
        modifyVars: {
          hack: `true; @import (reference) "${resolve('src/theme/arcovars.less')}";`
        },
        javascriptEnabled: true
      }
    }
  }
})
