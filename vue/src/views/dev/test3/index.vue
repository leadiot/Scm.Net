<template>
    <div class="download-page" id="top">
        <el-card class="download-card" aria-label="应用下载中心" shadow="never">
            <template #header>
                <div class="download-header">
                    <h2 class="download-title" aria-level="1">下载中心</h2>
                    <p class="download-subtitle">选择您需要的平台版本进行下载</p>
                </div>
            </template>

            <div class="platforms-container">
                <!-- 桌面端平台 -->
                <h3 class="section-title">桌面端应用</h3>
                <div class="desktop-platforms">
                    <!-- Windows平台 -->
                    <el-card class="platform-card" aria-labelledby="windows-title">
                        <div class="platform-icon windows-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="windows-title">Windows</h3>
                        <p class="platform-desc">适用于Windows 7/8/10/11</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('windows')"
                                :loading="downloading === 'windows'" :disabled="downloading === 'windows'"
                                aria-label="下载Windows版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                Windows版本 (.exe)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('windows')"
                                aria-label="查看Windows版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                    <!-- MacOS平台 -->
                    <el-card class="platform-card" aria-labelledby="mac-title">
                        <div class="platform-icon mac-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="mac-title">Mac</h3>
                        <p class="platform-desc">适用于macOS 10.13及以上版本</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('macos')"
                                :loading="downloading === 'macos'" :disabled="downloading === 'macos'"
                                aria-label="下载MacOS版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                MacOS版本 (.dmg)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('macos')"
                                aria-label="查看MacOS版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                    <!-- Linux平台 -->
                    <el-card class="platform-card" aria-labelledby="linux-title">
                        <div class="platform-icon linux-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="linux-title">Linux</h3>
                        <p class="platform-desc">适用于大多数Linux发行版</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('linux')"
                                :loading="downloading === 'linux'" :disabled="downloading === 'linux'"
                                aria-label="下载Linux版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                Linux版本 (.deb/.rpm)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('linux')"
                                aria-label="查看Linux版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                </div>
            </div>

            <div class="platforms-container">
                <!-- 移动端平台 -->
                <h3 class="section-title">移动端应用</h3>
                <div class="mobile-platforms">
                    <!-- Android平台 -->
                    <el-card class="platform-card" aria-labelledby="android-title">
                        <div class="platform-icon android-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="android-title">Android</h3>
                        <p class="platform-desc">适用于Android 5.0及以上版本</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('android')"
                                :loading="downloading === 'android'" :disabled="downloading === 'android'"
                                aria-label="下载Android版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                Android版本 (.apk)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('android')"
                                aria-label="查看Android版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                    <!-- iOS平台 -->
                    <el-card class="platform-card" aria-labelledby="ios-title">
                        <div class="platform-icon ios-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="ios-title">iOS</h3>
                        <p class="platform-desc">适用于iOS 13及以上版本</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('ios')"
                                :loading="downloading === 'ios'" :disabled="downloading === 'ios'" aria-label="下载iOS版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                iOS版本 (.ipa)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('ios')"
                                aria-label="查看iOS版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                    <!-- HarmonyOS平台 -->
                    <el-card class="platform-card" aria-labelledby="harmonyos-title">
                        <div class="platform-icon harmonyos-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="harmonyos-title">HarmonyOS</h3>
                        <p class="platform-desc">适用于HarmonyOS 2.0及以上版本</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('harmonyos')"
                                :loading="downloading === 'harmonyos'" :disabled="downloading === 'harmonyos'"
                                aria-label="下载HarmonyOS版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                HarmonyOS版本 (.hap)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('harmonyos')"
                                aria-label="查看HarmonyOS版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                </div>
            </div>

            <div class="platforms-container">
                <!-- 网页端平台 -->
                <h3 class="section-title">网页端应用</h3>
                <div class="mobile-platforms">
                    <!-- Chrome浏览器 -->
                    <el-card class="platform-card" aria-labelledby="chrome-title">
                        <div class="platform-icon chrome-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="chrome-title">Chrome浏览器</h3>
                        <p class="platform-desc">适用于Chrome浏览器 80及以上版本</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('chrome')"
                                :loading="downloading === 'chrome'" :disabled="downloading === 'chrome'"
                                aria-label="下载Chrome版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                Chrome版本 (.crx)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('chrome')"
                                aria-label="查看Chrome版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                    <!-- Edge浏览器 -->
                    <el-card class="platform-card" aria-labelledby="edge-title">
                        <div class="platform-icon edge-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="edge-title">Edge浏览器</h3>
                        <p class="platform-desc">适用于Edge浏览器 80及以上版本</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('edge')"
                                :loading="downloading === 'edge'" :disabled="downloading === 'edge'"
                                aria-label="下载Edge版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                Edge版本 (.crx)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('edge')"
                                aria-label="查看Edge版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                    <!-- Firefox浏览器 -->
                    <el-card class="platform-card" aria-labelledby="firefox-title">
                        <div class="platform-icon firefox-icon" aria-hidden="true"></div>
                        <h3 class="platform-name" id="firefox-title">Firefox浏览器</h3>
                        <p class="platform-desc">适用于Firefox浏览器 80及以上版本</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload('firefox')"
                                :loading="downloading === 'firefox'" :disabled="downloading === 'firefox'"
                                aria-label="下载Firefox版本">
                                <el-icon>
                                    <Download />
                                </el-icon>
                                Firefox版本 (.crx)
                            </el-button>
                            <el-button type="default" size="large" @click="showVersionInfo('firefox')"
                                aria-label="查看Firefox版本说明">
                                <el-icon>
                                    <Document />
                                </el-icon>
                                版本说明
                            </el-button>
                        </div>
                    </el-card>
                </div>
            </div>

            <!-- 返回顶部按钮 -->
            <el-backtop target="#top" :bottom="40" :right="40" class="back-to-top">
                <el-icon class="arrow-icon">
                    <ArrowUp />
                </el-icon>
            </el-backtop>
        </el-card>
    </div>
</template>

<script>
import { Download, Document, Smartphone, ChatDotRound, Phone, Mail, ArrowUp } from '@element-plus/icons-vue'

export default {
    name: 'DownloadPage',
    components: {
        Download,
        Document,
        Smartphone,
        ChatDotRound,
        Phone,
        Mail,
        ArrowUp
    },
    data() {
        return {
            // 当前正在下载的平台
            downloading: null,
            // 下载链接数据（实际项目中应替换为真实链接）
            downloadLinks: {
                windows: '/downloads/app-windows-v1.0.0.exe',
                macos: '/downloads/app-macos-v1.0.0.dmg',
                linux: '/downloads/app-linux-v1.0.0.AppImage',
                android: '/downloads/app-android-v1.0.0.apk',
                harmony: '/downloads/app-harmony-v1.0.0.hap'
            },
            // 应用商店链接
            appStoreLinks: {
                android: 'https://play.google.com/store/apps',
                ios: 'https://apps.apple.com/cn/app/id1668832000',
                harmony: 'https://appgallery.huawei.com/'
            },
            // 版本信息
            versionInfo: {
                currentVersion: 'v1.0.0',
                updateTime: '2023-12-01',
                updateContent: '修复已知问题，优化用户体验',
                // 各平台特定的版本信息
                platforms: {
                    windows: {
                        version: 'v1.0.0',
                        size: '128MB',
                        requirements: 'Windows 7/8/10/11，4GB RAM，200MB 磁盘空间',
                        features: ['桌面快捷方式', '自动更新', '托盘图标']
                    },
                    macos: {
                        version: 'v1.0.0',
                        size: '112MB',
                        requirements: 'macOS 10.14+，4GB RAM，200MB 磁盘空间',
                        features: ['触控栏支持', '自动更新', '菜单栏图标']
                    },
                    linux: {
                        version: 'v1.0.0',
                        size: '95MB',
                        requirements: 'Ubuntu 18.04+ / Fedora 30+，4GB RAM，200MB 磁盘空间',
                        features: ['AppImage格式', '无依赖安装', '托盘图标']
                    },
                    android: {
                        version: 'v1.0.0',
                        size: '45MB',
                        requirements: 'Android 7.0+，2GB RAM，100MB 存储空间',
                        features: ['离线模式', '推送通知', '深色模式']
                    },
                    harmony: {
                        version: 'v1.0.0',
                        size: '38MB',
                        requirements: 'HarmonyOS 2.0+，2GB RAM，100MB 存储空间',
                        features: ['原子化服务', '推送通知', '深色模式']
                    }
                }
            },
            // 下载状态
            downloadStats: {
                windows: 12456,
                macos: 8765,
                linux: 5432,
                android: 23456,
                harmony: 4321
            }
        }
    },
    mounted() {
        // 组件挂载后可以添加一些初始化逻辑
        console.log('下载页面已加载');
    },
    computed: {
        /**
         * 计算总下载量
         */
        totalDownloads() {
            return this.formatNumber(Object.values(this.downloadStats).reduce((sum, count) => sum + count, 0));
        }
    },
    methods: {
        /**
         * 处理下载事件
         * @param {string} platform - 平台标识
         */
        handleDownload(platform) {
            // 设置下载状态
            this.downloading = platform;

            // 显示下载提示
            this.$message.success(`正在准备${this.getPlatformName(platform)}版本下载...`);

            // 模拟下载延迟
            setTimeout(() => {
                try {
                    // 创建下载链接
                    const link = document.createElement('a');
                    link.href = this.downloadLinks[platform];
                    link.download = `app-${platform}-${this.versionInfo.currentVersion}${this.getFileExtension(platform)}`;
                    document.body.appendChild(link);

                    // 使用 MouseEvent 增强可访问性
                    const clickEvent = new MouseEvent('click', {
                        view: window,
                        bubbles: true,
                        cancelable: true
                    });
                    link.dispatchEvent(clickEvent);

                    // 延迟移除元素以确保下载开始
                    setTimeout(() => {
                        document.body.removeChild(link);
                    }, 100);

                    // 记录下载
                    this.recordDownload(platform);

                    // 下载开始后显示成功消息
                    this.$message.success(`${this.getPlatformName(platform)}版本开始下载，请注意保存文件。`);
                } catch (error) {
                    console.error('下载失败:', error);
                    this.$message.error(`下载失败，请稍后重试。`);
                } finally {
                    // 重置下载状态
                    this.downloading = null;
                }
            }, 1200);
        },

        /**
         * 打开应用商店
         * @param {string} platform - 平台标识
         */
        openAppStore(platform) {
            const url = this.appStoreLinks[platform];
            if (url) {
                try {
                    // 添加延迟和反馈，提升用户体验
                    const storeName = platform === 'android' ? 'Google Play' : '华为应用市场';
                    this.$message.info(`正在打开${storeName}...`);

                    // 使用更安全的方式打开新窗口
                    setTimeout(() => {
                        const newWindow = window.open('', '_blank');
                        if (newWindow) {
                            newWindow.opener = null; // 防止新窗口访问 opener 属性
                            newWindow.location.href = url;
                        } else {
                            // 如果弹窗被阻止，提供替代方案
                            this.$message.warning(`无法自动打开新窗口，请手动访问: ${url}`);
                            // 复制URL到剪贴板作为备选
                            navigator.clipboard.writeText(url).then(() => {
                                this.$message.success('链接已复制到剪贴板');
                            });
                        }
                    }, 500);
                } catch (error) {
                    console.error('打开应用商店失败:', error);
                    this.$message.error('打开应用商店失败，请稍后重试。');
                }
            }
        },

        /**
         * 显示版本信息
         * @param {string} platform - 平台标识
         */
        showVersionInfo(platform) {
            const info = this.versionInfo.platforms[platform];
            if (info) {
                this.$dialog.alert({
                    title: `${this.getPlatformName(platform)} 版本信息`,
                    message: `
            <div class="version-dialog-content">
              <p><strong>版本号：</strong>${info.version}</p>
              <p><strong>文件大小：</strong>${info.size}</p>
              <p><strong>系统要求：</strong>${info.requirements}</p>
              <p><strong>主要特性：</strong></p>
              <ul>
                ${info.features.map(feature => `<li>${feature}</li>`).join('')}
              </ul>
              <p><strong>下载次数：</strong>${this.formatNumber(this.downloadStats[platform])} 次</p>
            </div>
          `,
                    dangerouslyUseHTMLString: true,
                    confirmButtonText: '确定'
                });
            }
        },

        /**
         * 获取平台名称
         * @param {string} platform - 平台标识
         * @returns {string} 平台名称
         */
        getPlatformName(platform) {
            const names = {
                windows: 'Windows',
                macos: 'macOS',
                linux: 'Linux',
                android: 'Android',
                harmony: '鸿蒙'
            };
            return names[platform] || platform;
        },

        /**
         * 获取文件扩展名
         * @param {string} platform - 平台标识
         * @returns {string} 文件扩展名
         */
        getFileExtension(platform) {
            const extensions = {
                windows: '.exe',
                macos: '.dmg',
                linux: '.AppImage',
                android: '.apk',
                harmony: '.hap'
            };
            return extensions[platform] || '';
        },

        /**
         * 格式化数字（添加千位分隔符）
         * @param {number} num - 数字
         * @returns {string} 格式化后的字符串
         */
        formatNumber(num) {
            return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        },

        /**
         * 记录下载（实际项目中应发送到服务器）
         * @param {string} platform - 平台标识
         */
        recordDownload(platform) {
            // 这里只是模拟，实际项目中应调用API记录下载
            console.log(`记录${platform}下载`);
            // 模拟更新下载次数
            this.downloadStats[platform]++;
        }
    }
}
</script>

<style scoped>
.download-page {
    padding: 20px;
    background: linear-gradient(0, #FFFFFF 0%, #FAFAFA 100%);
    min-height: 100vh;
    display: flex;
    justify-content: center;
    align-items: flex-start;
}

.download-card {
    max-width: 1200px;
    width: 100%;
    background: white;
}

.download-header {
    text-align: center;
    padding: 40px 20px;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
}

.download-title {
    font-size: 36px;
    font-weight: 700;
    margin-bottom: 12px;
    text-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}

.download-subtitle {
    font-size: 18px;
    color: rgba(255, 255, 255, 0.9);
    margin: 0;
    font-weight: 300;
}

.platforms-container {
    padding: 40px 20px;
}

.section-title {
    text-align: center;
    font-size: 24px;
    font-weight: 600;
    color: #303133;
    margin-bottom: 30px;
    position: relative;
}

.section-title::after {
    content: '';
    display: block;
    width: 60px;
    height: 4px;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    margin: 10px auto 0;
    border-radius: 2px;
}

.desktop-platforms,
.mobile-platforms {
    display: flex;
    gap: 30px;
    margin-bottom: 50px;
    justify-content: center;
    flex-wrap: wrap;
}

.platform-card {
    width: 180px;
    text-align: center;
    border-radius: 16px;
    border: none;
    box-shadow: 0 6px 20px rgba(0, 0, 0, 0.1);
    transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
    overflow: hidden;
}

.platform-card:hover {
    transform: translateY(-10px);
    box-shadow: 0 12px 28px rgba(0, 0, 0, 0.18);
}

.platform-icon {
    width: 100px;
    height: 100px;
    margin: 30px auto 20px;
    border-radius: 50%;
    background-size: 60%;
    background-repeat: no-repeat;
    background-position: center;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 48px;
    color: white;
}

.windows-icon {
    background-color: #0078d4;
    background-image: url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAgMTAwIj48cGF0aCBkPSJNODguNDUgMjkuMzNMMC4zMiAxOS44NGEzLjM3IDMuMzcgMCAwIDEgMC01Ljk0bDk5Ljc0LTI0Ljk1YTQuNCA0LjQgMCAwIDEgNi4xIDBsMjkuODggMjkuODhhNC4zNiA0LjM2IDAgMCAxIDAgNi4xTDEuMjcgMTUuNzNhMy4zNyAzLjM3IDAgMCAxIDAtNS45NGw5OS43NC0yNC45NUE0LjQgNC40IDAgMCAxIDk2LjU1IDBsMjkuODggMjkuODhhNC4zNiA0LjM2IDAgMCAxIDAgNi4xTDguNDUgMjkuMzNhMy4zNyAzLjM3IDAgMCAxIDAtNS45NHoiIGZpbGw9Im5vbmUiIHN0cm9rZT0iI2ZmZmZmZiIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz48cGF0aCBkPSJNNDkuODggMzIuMDlhNCA0IDAgMCAwLTUuNzIgMCBMOS40NSAzOC42M2ExLjY3IDEuNjcgMCAwIDEtMS4xOCAyLjgyTDQwLjEyIDUxLjQxYTEuNjcgMS42NyAwIDAgMSAwIDIuODJsLTQuMzcgNC4zN2EyLjE0IDIuMTQgMCAwIDEtMy4wMyAwTDUuNCA1NC4yM2EyLjEyIDIuMTIgMCAwIDEgMC0zLjAxTDM1LjI1IDM4LjZhMS42NyAxLjY3IDAgMCAxIDIuODItMS4xOGw0MC45Ny0xMC4yYTQuNCA0IDAgMCAwIDAgLTguNjZsLTM1LjM0IDguODNhMS42NyAxLjY3IDAgMCAxLTIuODItMS4xOEw1LjQgMzMuNjNBMi4xMyAyLjEzIDAgMCAxIDUuNCAzMCAxLjY3IDEuNjcgMCAwIDEgNi41OCAyN0wzNC45OCA5Ljk3YTEuNjggMS42OCAwIDAgMSAyLjg0IDBsNDkuOTYgMTIuNDlhNC40IDQuNCAwIDAgMCAwIDguNjZsLTM1LjM0LTguODNhMS42NyAxLjY3IDAgMCAxLTIuODIgMS4xOE00OS44OCA0OC43M2E0IDQgMCAwIDAtNS43MiAwTDkuNDUgNTUuMmExLjY3IDEuNjcgMCAwIDEtMS4xOCAyLjgyTDQwLjEyIDY4LjNhMS42NyAxLjY3IDAgMCAxIDAgMi44MmwtNC4zNyA0LjM3YTIuMTQgMi4xNCAwIDAgMS0zLjAzIDBMMTUuNDQgNzEuOWEyLjEyIDIuMTIgMCAwIDEgMC0zLjAxTDM1LjI1IDU2LjZhMS42NyAxLjY3IDAgMCAxIDIuODItMS4xOGw0MC45Ny0xMC4yYTQuNCA0IDAgMCAwIDAtOC42Nkw4MC43NyA0OC43M2ExLjY3IDEuNjcgMCAwIDEtMS4xOCAyLjgybC0zMC4xNyAzMC4xN2ExLjY3IDEuNjcgMCAwIDEtMi44MiAwTDQ5Ljg4IDQ4LjczWiIgZmlsbD0ibm9uZSIgc3Ryb2tlPSIjZmZmZmZmIiBzdHJva2Utd2lkdGg9IjIiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIvPjxwYXRoIGQ9Ik00OS44OCA2NS4zOWE0IDQgMCAwIDAtNS43MiAwTDkuNDUgNzEuODNhMS42NyAxLjY3IDAgMCAxLTEuMTggMi44Mkw0MC4xMiA4NC40MWExLjY3IDEuNjcgMCAwIDEgMCAyLjgybC00LjM3IDQuMzdhMi4xNCAyLjE0IDAgMCAxLTMuMDMgMEwxNS40NCA5MC4yM2EyLjEyIDIuMTIgMCAwIDEgMC0zLjAxTDM1LjI1IDc0LjZhMS42NyAxLjY3IDAgMCAxIDIuODItMS4xOGw0MC45Ny0xMC4yYTQuNCA0IDAgMCAwIDAtOC42Nkw4MC43NyA2NS4zOWExLjY3IDEuNjcgMCAwIDEtMS4xOCAyLjgybC0zMC4xNyAzMC4xN2ExLjY3IDEuNjcgMCAwIDEtMi44MiAwTDQ5Ljg4IDY1LjM5WiIgZmlsbD0ibm9uZSIgc3Ryb2tlPSIjZmZmZmZmIiBzdHJva2Utd2lkdGg9IjIiIHN0cm9rZS1saW5lY2FwPSJyb3VuZCIgc3Ryb2tlLWxpbmVqb2luPSJyb3VuZCIvPjwvc3ZnPg==');
}

.mac-icon {
    background-color: #ffffff;
    background-image: url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAgMTAwIj48cGF0aCBkPSJNODUuNDQgMTYuMjZDODIuNDQgMTQuNjMgNzguNTcgMTMuNCA3NC42NyAxMy40QzcwLjggMTMuNCA2Ny4wNyAxNC42MyA2NC4zMyAxNi4yNkM2NC4xOCAxMC41OCA1OS44NiA2LjI2IDU0LjE3IDYuMjZDNDguNDcgNi4yNiA0NC4xNSAxMC41OCA0NC4wIDU0LjE3QzcwLjY1IDUyLjYgODUuNDQgMzcuOCA4NS40NCAxNi4yNnptLTI3LjIzIDI1LjQxYzQuNDQgMCA4LjEtMy42NiA4LjEtOC4xcy0zLjY2LTguMS04LjEtOC4xcy04LjEgMy42Ni04LjEgOC4xczMuNjYgOC4xIDguMSA4LjF6IiBmaWxsPSIj0v9w4fA0P81IyIvPjwvc3ZnPg==');
    border: 2px solid #e0e0e0;
}

.linux-icon {
    background-color: #fcc624;
    background-image: url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAgMTAwIj48cGF0aCBkPSJNMTggMTJjNi42MyAwIDEyIDUuMzcgMTIgMTJoNmMtNi42MyAwLTEyLTUuMzctMTItMTJoLTZ6bTAgMjhjNi42MyAwIDEyIDUuMzcgMTIgMTJoNmMtNi42MyAwLTEyLTUuMzctMTItMTJoLTZ6bTAgMjhjNi42MyAwIDEyIDUuMzcgMTIgMTJoNmMtNi42MyAwLTEyLTUuMzctMTItMTJoLTZ6bTQyLTY4YzUuNTIgMCAxMCA0LjQ4IDEwIDEwaDYtNS41MiAwLTEwLTQuNDgtMTAtMTBoLTZ6bTAgMjhjNS41MiAwIDEwIDQuNDggMTAgMTBoNi01LjUyIDAtMTAtNC40OC0xMC0xMGgtNnoiIGZpbGw9Im5vbmUiIHN0cm9rZT0iIzg4NGI3NSIgc3Ryb2tlLXdpZHRoPSI0Ii8+PHBhdGggZD0iTTQ4IDEyYzYuNjMgMCAxMiA1LjM3IDEyIDEyaDYtNi42MyAwLTEyLTUuMzctMTItMTJoLTZ6bTAgMjhjNi42MyAwIDEyIDUuMzcgMTIgMTJoNi02LjYzIDAtMTItNS4zNy0xMi0xMmgtNnoiIGZpbGw9Im5vbmUiIHN0cm9rZT0iIzg4NGI3NSIgc3Ryb2tlLXdpZHRoPSI0Ii8+PHBhdGggZD0iTTQ4IDY0YzYuNjMgMCAxMiA1LjM3IDEyIDEyaDYtNi42MyAwLTEyLTUuMzctMTItMTJoLTZ6IiBmaWxsPSJub25lIiBzdHJva2U9IiM4ODRiNzUiIHN0cm9rZS13aWR0aD0iNCIvPjwvc3ZnPg==');
}

.android-icon {
    background-color: #3ddc84;
    background-image: url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAgMTAwIj48cGF0aCBkPSJNODkuMTIgNDcuNjFjMi4yMi0xLjUgNC4xMi0zLjUyIDUuNjQtNS43NUw3Ni44OSAyMi4yQzc1LjMxIDIwLjczIDczLjUgMTkuNzcgNzEuNzYgMTkuMzdDNjYuNDQgMTcuOTUgNjEuMTIgMTcuOTUgNTUuOCAxOS4zN0w0Mi42OSAyNS44N0MxOC45OSA0NC41Mi0yLjA2IDc4LjcyIDAgODBjMi4xNCAxLjc4IDQuOTkgMi42NyA3LjkgMi42N2MxLjI4IDAgMi41NS0uMjUgMy43NS0uNzRsMS40OS0uNDljMy45Ni0xLjUgNy43NS0zLjczIDEuMy0yMy4xMy0zLjQ2IDcuNjQtNi45MiAxNS4yOS0xMC4zNyAyMi45My0zLjQ2IDcuNjQtNi45MiAxNS4yOS0xMC4zOCAyMi45My00LjY0IDIwLjQtOC4yIDE3LjkyLTEyLjE4IDE3LjkyLTIuNTQgMC0zLjggMS41Mi0zLjggMy43NHYzNy4zN2MwIDIuMjIgMS4xNCA0LjIgMy4wMyA1LjQ0TDQwLjE4IDc0Ljc1YzAuNTcuNDQgMS4yNS42NSAxLjk2LjY1IDEuNzYgMCAzLjItMS40NCAzLjItMy4yVjU4Ljg0YzAgMS4xMi0uOSAyLjAyLTEuOTYgMi4wMmgtMi42NmMtMS4wNyAwLTIuMDItLjktMi4wMi0yLjAydjE1Ljk0Yy0xLjc2IDAtMy4yIDEuNDQtMy4yIDMuMlY1NC44MmMzLjAyIDE2LjUgOS45MyAyNi41OCAxNi45OSAyOC4zMiAxMS4xOCAyLjgzIDIwLjA2LTE0LjQzIDIwLjA2LTE0LjQzQzkzLjgzIDY5LjY5IDk3LjI3IDU5LjQ1IDg5LjEyIDQ3LjYxeiIgZmlsbD0ibm9uZSIgc3Ryb2tlPSIjZmZmZmZmIiBzdHJva2Utd2lkdGg9IjIiLz48cGF0aCBkPSJNMzkuMDUgMTcuMTFjMS4zNi0uNDUgMi43OC0uMDkgMy45Ni44N0w1MC4wOSAyNS43Yy43OC41NiAxLjM3IDEuNDQgMS4zNyAyLjM2IDAgMS43Ni0xLjQ0IDMuMi0zLjIgMy4ycy0zLjItMS40NC0zLjItMy4yYzAtLjI4LjA1LS41Ni4xMy0uODJsLTYuMTctNS4zN2MtLjY0LS41Ni0uNDMtMS42NS41Ny0yLjA0YzEuMDItLjM5IDIuMTEuMTcgMi41NyAxLjE5bC42OSAxLjM4YzMuMzYgNi43Mi0yLjMgMTIuODktNy41NiAxMi44OS0xLjUgMC0yLjctMS4yMi0yLjctMi43N3MxLjItMi43NyAyLjctMi43N2M1LjI2IDAgMTAuOTktNi4xOCA3LjU2LTEyLjg5bC42OS0xLjM4eiIgZmlsbD0ibm9uZSIgc3Ryb2tlPSIjZmZmZmZmIiBzdHJva2Utd2lkdGg9IjIiLz48L3N2Zz4=');
}

.harmony-icon {
    background-color: #ff3b30;
    background-image: url('data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAxMDAgMTAwIj48cGF0aCBkPSJNODIuMjIgMzAuNjNhMy42NiAzLjY2IDAgMCAwLTIuNi0uOTRsLTQuMTMtNC4xM2MtLjgtLjgtMi4wNi0xLjIzLTMuMzYtMS4yM2MtMS4zIDAtMi41Ni40My0zLjM2IDEuMjNsLTQuMTMgNC4xM2MtLjguOC0xLjIzIDIuMDYtMS4yMyAzLjM2IDAgMS4zLjQzIDIuNTYgMS4yMyAzLjM2bDEyLjQwIDEyLjQwYzAuODkuODkgMi4xNSAxLjMyIDMuNDEgMS4zMnMxLjUyLS40MyAyLjQxLTEuMjNsNC4xMy00LjEzYy44LS44IDEuMjMtMi4wNiAxLjIzLTMuMzYgMC0xLjMtLjQzLTIuNTYtMS4yMy0zLjM2bC0xMi40LTEyLjQwYy0uODktLjg5LTIuMTUtMS4zMi0zLjQxLTEuMzJ6TTE3Ljc4IDMwLjYzYTMuNjYgMy42NiAwIDAgMCAtMi42LS45NGwtNC4xMy00LjEzYy0uOC0uOC0yLjA2LTEuMjMtMy4zNi0xLjIzYy0xLjMgMC0yLjU2LjQzLTMuMzYuMjNsLTQuMTMgNC4xM2MtLjguOC0xLjIzIDIuMDYtMS4yMyAzLjM2IDAgMS4zLjQzIDIuNTYgMS4yMyAzLjM2bDEyLjQgMTIuNDBjLjg5Ljg5IDIuMTUgMS4zMiAzLjQxIDEuMzJzMS41Mi0uNDMgMi40MS0xLjIzTDI0LjkgMzQuOTFjLjgtLjggMS4yMy0yLjA2IDEuMjMtMy4zNiAwLTEuMy0uNDQtMi41Ni0xLjI0LTMuMzZsLTEyLjQtMTIuNDBjLS44OS0uODktMi4xNS0xLjMyLTMuNDEtMS4zMnptNDAgNGEzLjY2IDMuNjYgMCAwIDEtMi42LS45NGwtNC4xMy00LjEzYy0uOC0uOC0yLjA2LTEuMjMtMy4zNi0xLjIzYy0xLjMgMC0yLjU2LjQzLTMuMzYuMjNsLTQuMTMgNC4xM2MtLjguOC0xLjIzIDIuMDYtMS4yMyAzLjM2IDAgMS4zLjQzIDIuNTYgMS4yMyAzLjM2bDEyLjQgMTIuNDBjLjg5Ljg5IDIuMTUgMS4zMiAzLjQxIDEuMzJzMS41Mi0uNDMgMi40MS0xLjIzTDY0LjkgNzQuOTFjLjgtLjggMS4yMy0yLjA2IDEuMjMtMy4zNiAwLTEuMy0uNDQtMi41Ni0xLjI0LTMuMzZsLTEyLjQtMTIuNDBjLS44OS0uODktMi4xNS0xLjMyLTMuNDEtMS4zMnptNDAgMGEzLjY2IDMuNjYgMCAwIDEtMi42LS45NGwtNC4xMy00LjEzYy0uOC0uOC0yLjA2LTEuMjMtMy4zNi0xLjIzYy0xLjMgMC0yLjU2LjQzLTMuMzYuMjNsLTQuMTMgNC4xM2MtLjguOC0xLjIzIDIuMDYtMS4yMyAzLjM2IDAgMS4zLjQzIDIuNTYgMS4yMyAzLjM2bDEyLjQgMTIuNDBjLjg5Ljg5IDIuMTUgMS4zMiAzLjQxIDEuMzJzMS41Mi0uNDMgMi40MS0xLjIzTDg0LjkgMzQuOTFjLjgtLjggMS4yMy0yLjA2IDEuMjMtMy4zNiAwLTEuMy0uNDQtMi41Ni0xLjI0LTMuMzZsLTEyLjQtMTIuNDBjLS44OS0uODktMi4xNS0xLjMyLTMuNDEtMS4zMnptLTQwIDQwYTMuNjYgMy42NiAwIDAgMS0yLjYtLjk0bC00LjEzLTQuMTNjLS44LS44LTIuMDYtMS4yMy0zLjM2LTEuMjNjLTEuMyAwLTIuNTYuNDMtMy4zNi4yM2wtNC4xMyA0LjEzYy0uOC44LTEuMjMgMi4wNi0xLjIzIDMuMzYgMCAxLjMuNDMgMi41NiAxLjIzIDMuMzZsMTIuNCAxMi40MGMuODkuODkgMi4xNSAxLjMyIDMuNDEgMS4zMnMxLjUyLS40MyAyLjQxLTEuMjNMNDIuOSA3NC45MWMuOC0uOCAxLjIzLTIuMDYgMS4yMy0zLjM2IDAtMS4zLS40NC0yLjU2LTEuMjQtMy4zNmwtMTIuNC0xMi40Yy0uODktLjg5LTIuMTUtMS4zMi0zLjQxLTEuMzJ6IiBmaWxsPSJub25lIiBzdHJva2U9IiNmZmZmZmYiIHN0cm9rZS13aWR0aD0iMiIvPjwvc3ZnPg==');
}

.platform-name {
    font-size: 22px;
    font-weight: 600;
    margin-bottom: 8px;
    color: #303133;
}

.platform-desc {
    font-size: 14px;
    color: #606266;
    margin-bottom: 25px;
    padding: 0 20px;
}

.download-buttons {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 12px;
    padding: 20px 30px;
}

.version-info,
.system-requirements {
    padding: 0 40px 40px;
}

.version-details {
    display: flex;
    flex-wrap: wrap;
    gap: 30px;
    margin-top: 20px;
}

.version-item {
    display: flex;
    align-items: center;
    min-width: 200px;
    background: #f8f9fa;
    padding: 15px 20px;
    border-radius: 8px;
    border-left: 4px solid #667eea;
}

.version-label {
    font-weight: 500;
    color: #606266;
    margin-right: 12px;
    min-width: 80px;
}

.version-value {
    color: #303133;
    font-weight: 500;
}

.requirements-content {
    line-height: 1.8;
    color: #606266;
    background: #f8f9fa;
    padding: 20px;
    border-radius: 8px;
    border-left: 4px solid #667eea;
}

/* 响应式设计 - 大屏幕 (1400px+) */
@media (min-width: 1400px) {
    .download-card {
        max-width: 1400px;
    }

    .desktop-platforms {
        gap: 40px;
    }

    .platform-card {
        width: 280px;
    }
}

/* 响应式设计 - 中大屏幕 (1200px - 1399px) */
@media (min-width: 1200px) and (max-width: 1399px) {
    .desktop-platforms {
        gap: 30px;
    }

    .platform-card {
        width: 350px;
    }
}

/* 响应式设计 - 中等屏幕 (1024px - 1199px) */
@media (min-width: 1024px) and (max-width: 1199px) {

    .desktop-platforms,
    .mobile-platforms {
        gap: 20px;
    }

    .platform-card {
        width: 280px;
    }

    .download-title {
        font-size: 32px;
    }

    .section-title {
        font-size: 22px;
    }
}

/* 响应式设计 - 平板设备 (768px - 1023px) */
@media (min-width: 768px) and (max-width: 1023px) {
    .download-card {
        margin: 30px 20px;
    }

    .download-header {
        padding: 35px 20px;
    }

    .download-title {
        font-size: 28px;
    }

    .download-subtitle {
        font-size: 16px;
    }

    .desktop-platforms,
    .mobile-platforms {
        gap: 20px;
        justify-content: center;
    }

    .platform-card {
        width: 260px;
    }

    .platform-name {
        font-size: 20px;
    }

    .platform-icon {
        width: 90px;
        height: 90px;
        margin: 25px auto 15px;
    }

    .version-details {
        gap: 20px;
    }

    .version-item {
        min-width: calc(50% - 10px);
        padding: 12px 15px;
    }

    .version-info,
    .system-requirements {
        padding: 0 30px 30px;
    }
}

/* 响应式设计 - 小型平板和大型手机 (600px - 767px) */
@media (min-width: 600px) and (max-width: 767px) {
    .download-page {
        padding: 15px;
    }

    .download-card {
        margin: 20px 15px;
    }

    .download-header {
        padding: 30px 15px;
    }

    .download-title {
        font-size: 26px;
    }

    .download-subtitle {
        font-size: 15px;
    }

    .platforms-container {
        padding: 30px 15px;
    }

    .section-title {
        font-size: 20px;
        margin-bottom: 25px;
    }

    .desktop-platforms,
    .mobile-platforms {
        flex-direction: column;
        align-items: center;
        gap: 20px;
    }

    .platform-card {
        width: 100%;
        max-width: 320px;
    }

    .platform-name {
        font-size: 20px;
    }

    .platform-icon {
        width: 85px;
        height: 85px;
        margin: 25px auto 15px;
    }

    .version-details {
        flex-direction: column;
        gap: 15px;
    }

    .version-item {
        min-width: auto;
        padding: 12px 15px;
    }

    .version-info,
    .system-requirements {
        padding: 0 20px 25px;
    }
}

/* 响应式设计 - 移动设备 (小于 600px) */
@media (max-width: 599px) {
    .download-page {
        padding: 10px;
    }

    .download-card {
        margin: 15px 10px;
        border-radius: 12px;
        overflow: hidden;
    }

    .download-header {
        padding: 25px 15px;
    }

    .download-title {
        font-size: 24px;
        margin-bottom: 10px;
        line-height: 1.3;
    }

    .download-subtitle {
        font-size: 14px;
    }

    .platforms-container {
        padding: 25px 15px;
    }

    .section-title {
        font-size: 18px;
        margin-bottom: 20px;
    }

    .section-title::after {
        width: 40px;
        height: 3px;
        margin: 8px auto 0;
    }

    .desktop-platforms,
    .mobile-platforms {
        gap: 15px;
        margin-bottom: 35px;
    }

    .platform-card {
        width: 100%;
        border-radius: 12px;
    }

    .platform-icon {
        width: 75px;
        height: 75px;
        margin: 20px auto 15px;
    }

    .platform-name {
        font-size: 18px;
    }

    .platform-desc {
        font-size: 13px;
        padding: 0 15px;
        margin-bottom: 20px;
    }

    .download-buttons {
        padding: 0 15px 25px;
        gap: 10px;
    }

    .download-btn {
        height: 44px;
        font-size: 14px;
    }

    .version-details {
        flex-direction: column;
        gap: 12px;
        margin-top: 15px;
    }

    .version-item {
        flex-direction: column;
        align-items: flex-start;
        padding: 12px;
        gap: 5px;
    }

    .version-label {
        margin-right: 0;
        font-size: 14px;
    }

    .version-value {
        font-size: 14px;
    }

    .version-info,
    .system-requirements {
        padding: 0 15px 20px;
    }

    .requirements-content {
        padding: 15px;
        font-size: 14px;
    }
}

/* 响应式设计 - 超小型设备 (小于 360px) */
@media (max-width: 359px) {
    .download-title {
        font-size: 22px;
    }

    .platform-icon {
        width: 70px;
        height: 70px;
    }

    .download-btn {
        height: 42px;
        font-size: 13px;
    }

    .platform-desc {
        font-size: 12px;
    }
}

/* 高对比度模式支持 */
@media (prefers-contrast: high) {
    .download-card {
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
    }

    .platform-card {
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        border: 1px solid #dcdfe6;
    }

    .section-title::after {
        height: 3px;
    }

    .version-item,
    .requirements-content {
        border-left-width: 3px;
    }
}

/* 减少动画模式支持 */
@media (prefers-reduced-motion: reduce) {
    * {
        animation-duration: 0.01ms !important;
        animation-iteration-count: 1 !important;
        transition-duration: 0.01ms !important;
        scroll-behavior: auto !important;
    }

    .download-card {
        animation: none;
    }

    .platform-card:hover,
    .download-btn:hover {
        transform: none;
    }

    .support-item:hover {
        transform: none;
    }
}
</style>