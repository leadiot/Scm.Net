<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Statistic 统计组件</h1>
                <p>Statistic 组件用于展示统计数据。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Statistic 组件默认使用示例。">
                    <sc-statistic ref="statistic" title="统计" value="100" description="这是一个统计组件" prefix="人" suffix="万"
                        tips="当前的人流量统计"></sc-statistic>
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
import scStatistic from '@/components/scStatistic';

export default {
    name: 'scui_statistic',
    components: {
        scCode,
        scStatistic,
    },
    data() {
        return {
            example1: {
                code: `<sc-statistic ref="statistic" title="统计" value="100" description="这是一个统计组件" prefix="人" suffix="万"
 tips="当前的人流量统计"></sc-statistic>`,
            },
            fieldTable: [
                { name: 'title', type: 'String', desc: '统计标题', val: '', def: '' },
                { name: 'value', type: 'String', desc: '统计数据', val: '', def: '' },
                { name: 'prefix', type: 'String', desc: '统计值前缀', val: '', def: '' },
                { name: 'suffix', type: 'String', desc: '统计值后缀', val: '', def: '' },
                { name: 'description', type: 'String', desc: '统计描述', val: '', def: '' },
                { name: 'tips', type: 'String', desc: '统计提示', val: '', def: '' },
                { name: 'groupSeparator', type: 'Boolean', desc: '是否显示组分隔符', val: '', def: 'false' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>