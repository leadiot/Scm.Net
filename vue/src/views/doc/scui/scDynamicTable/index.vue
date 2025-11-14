<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>FormTable 动态表格</h1>
                <p>FormTable 组件用于动态构建表格。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="FormTable 组件默认使用示例。">
                    <sc-dynamic-table ref="formTable" v-model="list" :addTemplate="addTemplate">
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
            list: [],
            addTemplate:
            {
                name: '',
                age: 0,
                score: 'A',
            },
            code1: `<sc-dynamic-table ref="formTable" v-model="list" :addTemplate="addTemplate">
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
        };
    },
    methods: {
        handleClick() {
            this.visible = true;
        }
    }
}
</script>