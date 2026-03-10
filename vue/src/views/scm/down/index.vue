<template>
    <div class="download-page" id="top">
        <el-card class="download-card" aria-label="应用下载中心" shadow="never">
            <template #header>
                <div class="download-header">
                    <h2 class="download-title" aria-level="1">下载中心</h2>
                    <p class="download-subtitle">选择您需要的平台版本进行下载</p>
                </div>
            </template>

            <div class="platforms-container" v-for="item in platformList" :key="item.id" v-show="item.visible">
                <h3 class="platform-title">{{ item.name }}</h3>
                <div class="platforms">
                    <el-card class="platform-card" v-for="download in item.downloadList" :key="download.id"
                        v-show="download.visible">
                        <div class="platform-icon" aria-hidden="true">
                            <el-avatar :size="80" :src="download.icon" />
                        </div>
                        <h3 class="platform-name" id="windows-title">{{ download.name }}</h3>
                        <p class="platform-desc">{{ download.desc }}</p>
                        <div class="download-buttons">
                            <el-button type="primary" size="large" @click="handleDownload(download.client)"
                                :loading="isPlatform(download.client)" :disabled="isPlatform(download.client)">
                                <sc-icon name="ms-download" />{{ download.name }}版本
                            </el-button>
                            <el-button type="default" @click="showVersionInfo(download.client)">
                                <sc-icon name="ms-docs" />版本说明
                            </el-button>
                        </div>
                    </el-card>
                </div>
            </div>

            <!-- 返回顶部按钮 -->
            <el-backtop target="#top" :bottom="40" :right="140">
            </el-backtop>
        </el-card>
    </div>
    <el-dialog title="版本说明" v-model="versionVisible" width="560px" :close-on-click-modal="false"
        :close-on-press-escape="false" :show-close="false">
        <el-descriptions :column="1" :label-width="100" border>
            <el-descriptions-item label="版本号：">{{ downloadInfo.version }}</el-descriptions-item>
            <el-descriptions-item label="构建版本：">{{ downloadInfo.build }}</el-descriptions-item>
            <el-descriptions-item label="发布日期：">{{ downloadInfo.releaseDate }}</el-descriptions-item>
            <el-descriptions-item label="文件大小：">{{ downloadInfo.size }}</el-descriptions-item>
            <el-descriptions-item label="系统要求：">{{ downloadInfo.requirements }}</el-descriptions-item>
            <el-descriptions-item label="主要特性：">
                <div v-for="feature in downloadInfo.features" :key="feature">{{ feature }}</div>
            </el-descriptions-item>
            <el-descriptions-item label="下载次数：">
                {{ downloadInfo.qty }} 次
            </el-descriptions-item>
        </el-descriptions>
        <template #footer>
            <el-button type="primary" @click="versionVisible = false">关闭</el-button>
        </template>
    </el-dialog>
</template>

<script>
import { useRoute } from 'vue-router';

export default {
    name: 'scm_down',
    data() {
        return {
            downloading: 0,
            versionVisible: false,
            downloadInfo: {},
            downloadList: [],
            platformList: [
                { id: 'desktop', name: '桌面端应用', visible: false },
                { id: 'mobile', name: '移动端应用', visible: false },
                { id: 'browser', name: '网页端应用', visible: false },
                { id: 'quickapp', name: '小程序应用', visible: false }
            ],
        }
    },
    mounted() {
        //this.getVersionInfo();
        this.defaultData();
    },
    methods: {
        /**
         * 获取版本信息
         */
        async getVersionInfo() {
            var route = useRoute();
            var code = route.params.code || 'scm.net';

            try {
                var res = await this.$API.scmdevversion.list.get({ 'code': code });
                if (res == null || res.code != 200) {
                    return;
                }
                this.downloadList = res.data || [];
            } catch (error) {
                this.$message.error('获取版本信息失败，请稍后重试。');
            }
        },

        defaultData() {
            this.downloadList = this.$CONFIG.DOWNLOAD_LIST || [];
            this.downloadList.forEach(item => {
                if (item.client == 20 || item.client == 30 || item.client == 40) {
                    this.pushDownloadList('desktop', item);
                    return;
                }
                if (item.client == 50 || item.client == 60 || item.client == 70) {
                    this.pushDownloadList('mobile', item);
                    return;
                }
                if (item.client == 11 || item.client == 12 || item.client == 13) {
                    this.pushDownloadList('browser', item);
                    return;
                }
                if (item.client == 81 || item.client == 82) {
                    this.pushDownloadList('quickapp', item);
                    return;
                }
            });
        },

        pushDownloadList(platform, item) {
            if (!item.visible) {
                return;
            }

            this.platformList.forEach(plat => {
                if (plat.id == platform) {
                    if (!plat.downloadList) {
                        plat.downloadList = [];
                    }
                    plat.visible = true;
                    plat.downloadList.push(item);
                }
            });
        },

        getPlatformInfo(client) {
            return this.downloadList.find(item => item.client === client);
        },

        /**
         * 检查是否正在下载指定平台
         * @param client 平台标识
         * @returns 是否正在下载
         */
        isPlatform(client) {
            return this.downloading == client;
        },

        /**
         * 处理下载事件
         * @param {string} client - 平台标识
         */
        handleDownload(client) {
            var info = this.getPlatformInfo(client);
            if (!info) {
                this.$message.error(`当前版本不支持${this.getPlatformName(client)}下载。`);
                return;
            }

            // 设置下载状态
            this.downloading = client;

            // 显示下载提示
            this.$message.success(`正在准备${this.getPlatformName(client)}版本下载...`);

            // 模拟下载延迟
            setTimeout(() => {
                try {
                    // 创建下载链接
                    const link = document.createElement('a');
                    link.href = info.url;
                    link.download = info.file || this.getFileName(info.url);
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
                    this.recordDownload(client);

                    // 下载开始后显示成功消息
                    this.$message.success(`${this.getPlatformName(client)}版本开始下载，请注意保存文件。`);
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
            var info = this.getPlatformInfo(platform);
            if (!info) {
                this.$message.error(`当前版本不支持${this.getPlatformName(platform)}下载。`);
                return;
            }

            var url = info.url;
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
        },

        /**
         * 显示版本信息
         * @param {string} platform - 平台标识
         */
        showVersionInfo(platform) {
            var info = this.getPlatformInfo(platform);
            if (!info) {
                this.$message.error(`当前版本不支持${this.getPlatformName(platform)}下载。`);
                return;
            }

            if (!info.features) {
                info.features = [];
            }
            if (!info.qty) {
                info.qty = 0;
            }

            this.downloadInfo = info;
            this.versionVisible = true;
        },

        getFileName(url) {
            return url.split('/').pop();
        },

        /**
         * 获取平台名称
         * @param {string} client - 平台标识
         * @returns {string} 平台名称
         */
        getPlatformName(client) {
            const names = {
                10: 'Web',
                11: 'Google Chrome',
                12: 'Microsoft Edge',
                13: 'Mozilla Firefox',
                20: 'Windows',
                30: 'MacOS',
                40: 'Linux',
                50: '安卓',
                60: 'iOS',
                70: '鸿蒙',
                80: '快应用',
                81: '微信小程序',
                82: '支付宝小程序'
            };
            return names[client] || client;
        },

        /**
         * 记录下载（实际项目中应发送到服务器）
         * @param {string} client - 平台标识
         */
        recordDownload(client) {
            // 这里只是模拟，实际项目中应调用API记录下载
            console.log(`记录${this.getPlatformName(client)}下载`);
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

.platform-title {
    text-align: center;
    font-size: 24px;
    font-weight: 600;
    color: #303133;
    margin-bottom: 30px;
    position: relative;
}

.platform-title::after {
    content: '';
    display: block;
    width: 60px;
    height: 4px;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    margin: 10px auto 0;
    border-radius: 2px;
}

.platforms {
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

    .el-button {
        width: 100%;
        margin: 0px;
    }
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

    .platform-title {
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

    .platform-title {
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

    .platform-title {
        font-size: 18px;
        margin-bottom: 20px;
    }

    .platform-title::after {
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

    .platform-title::after {
        height: 3px;
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
}
</style>