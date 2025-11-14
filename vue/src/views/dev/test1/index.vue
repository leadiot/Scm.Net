<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>WaterMark 水印组件</h1>
                    <p>WaterMark 组件用于添加水印。</p>
                    <sc-code lang="html" :code="code1" title="基础用法" desc="WaterMark 组件默认使用示例。">
                        <sc-watermark ref="watermark" text="水印文字">
                            <div style="width: 100%; height: 400px;"></div>
                        </sc-watermark>
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scWatermark from '@/components/scWaterMark';

export default {
    name: 'scui_watermark',
    components: {
        scCode,
        scWatermark,
    },
    data() {
        return {
            src: '',
            code1: `<sc-watermark ref="watermark" text="水印文字">
    <div style="width: 100%; height: 400px;"></div>
</sc-watermark>`,
        };
    },
    methods: {
    }
}
</script>