import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd())
  
  return {
    plugins: [vue()],
    
    resolve: {
      alias: {
        '@': path.resolve(__dirname, 'src')
      },
      extensions: ['.mjs', '.js', '.ts', '.jsx', '.tsx', '.json', '.vue']
    },
    
    server: {
      port: parseInt(env.VITE_WEB_PORT || '2800'),
      open: false,
      proxy: {
        '/api': {
          target: env.VITE_API_BASE || 'http://localhost:5000',
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/api/, '/')
        }
      }
    },
    
    build: {
      outDir: 'dist',
      assetsDir: 'static',
      sourcemap: false,
      minify: mode === 'production' ? 'esbuild' : false,
      chunkSizeWarningLimit: 1500,
      rolldownOptions: {
        onwarn(warning, warn) {
          if (warning.message.includes('eval') || warning.message.includes('PLUGIN_TIMINGS')) {
            return
          }
          warn(warning)
        },
        output: {
          codeSplitting: {
            minSize: 20000,
            groups: [
              {
                name: 'vue-vendor',
                test: /node_modules[\\/](@?vue|vue-router|vuex|vue-i18n)/,
                priority: 30
              },
              {
                name: 'element-plus',
                test: /node_modules[\\/](@?element-plus)/,
                priority: 25
              },
              {
                name: 'echarts',
                test: /node_modules[\\/]echarts/,
                priority: 20
              },
              {
                name: 'xgplayer',
                test: /node_modules[\\/]xgplayer/,
                priority: 20
              },
              {
                name: 'xlsx',
                test: /node_modules[\\/]xlsx/,
                priority: 20
              },
              {
                name: 'highlight',
                test: /node_modules[\\/](@?highlight)/,
                priority: 15
              },
              {
                name: 'crypto',
                test: /node_modules[\\/]crypto-js/,
                priority: 15
              },
              {
                name: 'pinyin',
                test: /node_modules[\\/]pinyin-pro/,
                priority: 15
              },
              {
                name: 'vendor',
                test: /node_modules/,
                minSize: 30000,
                priority: 10
              }
            ]
          }
        }
      }
    },

    optimizeDeps: {
      include: ['vue', 'vue-router', 'vuex', 'axios', 'element-plus']
    }
  }
})
