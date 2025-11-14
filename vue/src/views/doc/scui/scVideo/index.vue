<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Vedio 视频组件</h1>
                    <p>Vedio 组件用于播放视频。</p>
                    <sc-code lang="html" :code="code1" title="基础用法" desc="Vedio 组件默认使用示例。">
                        <sc-video ref="video" :src="src" />
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scVideo from '@/components/scVideo';

export default {
    name: 'scui_video',
    components: {
        scCode,
        scVideo,
    },
    data() {
        return {
            src: '',
            code1: `<sc-video ref="video" :src="src" />`,
        };
    },
    methods: {
    }
}
</script>