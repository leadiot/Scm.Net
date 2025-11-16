<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Trend 趋势组件</h1>
                <p>Trend 组件用于显示趋势。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Trend 组件默认使用示例。">
                    <sc-trend ref="trend" v-model="example1.value" prefix="前缀" suffix="后缀" />
                </sc-code>

                <h2>属性</h2>
                <el-table :data="fieldTable" style="width: 100%">
                    <el-table-column prop="name" label="属性名" width="120" />
                    <el-table-column prop="desc" label="说明" />
                    <el-table-column prop="type" label="类型" width="80" />
                    <el-table-column prop="val" label="可选值" width="120" />
                    <el-table-column prop="def" label="默认值" width="120" />
                </el-table>
                <h2>事件</h2>
                <el-table :data="eventTable" style="width: 100%">
                    <el-table-column prop="name" label="事件名" width="120" />
                    <el-table-column prop="desc" label="说明" />
                    <el-table-column prop="args" label="回调参数" width="120" />
                </el-table>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scTrend from '@/components/scTrend';

export default {
    name: 'scui_trend',
    components: {
        scCode,
        scTrend,
    },
    data() {
        return {
            example1: {
                value: 1,
                code: `<sc-trend ref="trend" v-model="value" prefix="前缀" suffix="后缀" />`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'Number', desc: '趋势值', val: '', def: '' },
                { name: 'prefix', type: 'String', desc: '前缀文本', val: '', def: '' },
                { name: 'suffix', type: 'String', desc: '后缀文本', val: '', def: '' },
                { name: 'reverse', type: 'Boolean', desc: '是否反向', val: 'true/false', def: 'false' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>