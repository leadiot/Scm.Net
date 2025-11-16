<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Icon 组件</h1>
                <p>Icon 组件用于显示图标。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Icon 组件默认使用示例。">
                    <sc-icon name="sc-user-line"></sc-icon>
                    <sc-icon name="sc-user-line" color="red"></sc-icon>
                    <sc-icon name="sc-user-line" color="red" :size="24"></sc-icon>
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

export default {
    name: 'scui_icon',
    components: {
        scCode,
    },
    data() {
        return {
            example1: {
                code: `<sc-icon name="sc-user-line"></sc-icon>
<sc-icon name="sc-user-line" color="red"></sc-icon>
<sc-icon name="sc-user-line" color="red" :size="24"></sc-icon>`,
            },
            fieldTable: [
                { name: 'name', type: 'String', desc: '图标名称', val: '', def: '' },
                { name: 'size', type: 'Number', desc: '图标大小', val: '', def: '24' },
                { name: 'color', type: 'String', desc: '图标颜色', val: '', def: '' },
                { name: 'title', type: 'String', desc: '图标提示', val: '', def: '' },
                { name: 'type', type: 'String', desc: '图标风格', val: 'Filled、Outlined、Round、Sharp', def: 'Outlined' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>