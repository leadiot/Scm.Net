<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>FormTable 动态表格组件</h1>
                <p>FormTable 组件用于动态构建表格。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="FormTable 组件默认使用示例。">
                    <sc-dynamic-table ref="formTable" v-model="example1.list" :addTemplate="example1.addTemplate">
                        <el-table-column label="姓名" prop="name">
                            <template #default="scope">
                                <el-input v-model="scope.row.name" placeholder="请输入姓名"></el-input>
                            </template>
                        </el-table-column>
                        <el-table-column label="年龄" prop="age">
                            <template #default="scope">
                                <el-input-number v-model="scope.row.age" placeholder="请输入年龄"></el-input-number>
                            </template>
                        </el-table-column>
                        <el-table-column label="分数" prop="score">
                            <template #default="scope">
                                <el-select v-model="scope.row.score" placeholder="请输入分数">
                                    <el-option label="A" value="A"></el-option>
                                    <el-option label="B" value="B"></el-option>
                                    <el-option label="C" value="C"></el-option>
                                </el-select>
                            </template>
                        </el-table-column>
                    </sc-dynamic-table>
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
import scDynamicTable from "@/components/scDynamicTable";

export default {
    name: 'scui_form_table',
    components: {
        scCode,
        scDynamicTable,
    },
    data() {
        return {
            example1: {
                list: [],
                addTemplate:
                {
                    name: '',
                    age: 0,
                    score: 'A',
                },
                code: `<sc-dynamic-table ref="formTable" v-model="list" :addTemplate="addTemplate">
    <el-table-column label="姓名" prop="name">
        <template #default="scope">
            <el-input v-model="scope.row.name" placeholder="请输入姓名"></el-input>
        </template>
    </el-table-column>
    <el-table-column label="年龄" prop="age">
        <template #default="scope">
            <el-input-number v-model="scope.row.age" placeholder="请输入年龄"></el-input-number>
        </template>
    </el-table-column>
    <el-table-column label="分数" prop="score">
        <template #default="scope">
            <el-select v-model="scope.row.score" placeholder="请输入分数">
                <el-option label="A" value="A"></el-option>
                <el-option label="B" value="B"></el-option>
                <el-option label="C" value="C"></el-option>
            </el-select>
        </template>
    </el-table-column>
</sc-dynamic-table>`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'Array', desc: '动态表格数据', val: '', def: '[]' },
                { name: 'isAdd', type: 'Boolean', desc: '是否显示新增按钮', val: '', def: 'true' },
                { name: 'addTemplate', type: 'Object', desc: '动态表格新增行模板', val: '', def: '{}' },
                { name: 'placeholder', type: 'String', desc: '无数据时提示信息', val: '', def: '暂无数据' },
                { name: 'dragSort', type: 'Boolean', desc: '是否开启拖拽排序', val: '', def: 'false' },
                { name: 'tableHeight', type: 'Object', desc: '动态表格高度', val: '', def: '300px' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>