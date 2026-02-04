const { defineConfig } = require('@vue/cli-service')

module.exports = defineConfig({
	//设置为空打包后不分更目录还是多级目录
	publicPath: '',
	//build编译后存放静态文件的目录
	//assetsDir: "static",

	// build编译后不生成资源MAP文件
	productionSourceMap: false,

	//开发服务,build后的生产模式还需nginx代理
	devServer: {
		open: false, //运行后自动打开浏览器
		port: process.env.VUE_APP_WEB_PORT, //挂载端口
		proxy: {
			'/api': {
				target: process.env.VUE_APP_API_BASE,
				ws: true,
				pathRewrite: {
					'^/api': '/'
				}
			}
		},
		client: {
			overlay: false,
		}
	},

	chainWebpack: config => {
		// 移除 prefetch 插件
		config.plugins.delete('preload');
		config.plugins.delete('prefetch');
		config.resolve.alias.set('vue-i18n', 'vue-i18n/dist/vue-i18n.cjs.js');
		config.plugin('define').tap((definitions) => {
			Object.assign(definitions[0], {
				__VUE_OPTIONS_API__: 'true',
				__VUE_PROD_DEVTOOLS__: 'false',
				__VUE_PROD_HYDRATION_MISMATCH_DETAILS__: 'false'
			})
			return definitions
		});
	},

	configureWebpack: {
		//devtool: 'source-map',
		devtool: process.env.NODE_ENV === 'development' ? 'eval-cheap-module-source-map' : false,
		//性能提示
		performance: {
			hints: false
		},
		optimization: {
			splitChunks: {
				chunks: "all",
				maxInitialRequests: 20,
				maxAsyncRequests: 20,
				maxSize: 244000,
				automaticNameDelimiter: '~',
				name: "scmChunks",
				cacheGroups: {
					//第三方库抽离
					vendor: {
						name: "modules",
						test: /[\\/]node_modules[\\/]/,
						priority: -10
					},
					elicons: {
						name: "elicons",
						test: /[\\/]node_modules[\\/]@element-plus[\\/]icons-vue[\\/]/
					},
					echarts: {
						name: "echarts",
						test: /[\\/]node_modules[\\/]echarts[\\/]/
					},
					xgplayer: {
						name: "xgplayer",
						test: /[\\/]node_modules[\\/]xgplayer.*[\\/]/
					},
					codemirror: {
						name: "codemirror",
						test: /[\\/]node_modules[\\/]codemirror[\\/]/
					}
				}
			}
		},
		externals: {
			'vue': 'Vue',
			'vue-router': 'VueRouter',
			'vuex': 'Vuex',
			'axios': 'axios',
			'element-plus': 'ElementPlus',
			'element-icons': 'ElementPlusIconsVue',
			'echarts': 'echarts',
			'xlsx': 'XLSX',
			'highlight.js': 'highlight.js',
			'wang-editor': 'wangEditor',
			'editorjs': 'editorjs'
		}
	}
})
