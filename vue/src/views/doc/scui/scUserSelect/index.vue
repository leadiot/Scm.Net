<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Select User 人员选择器组件</h1>
                    <p>Select User 组件用于选择人员的展示。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Select User 组件默认使用示例。">
                        <sc-user-select ref="selectUser" v-model:selectOpen="example1.isOpenUser"
                            @onSelect="selectUserRes"></sc-user-select>
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
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scUserSelect from '@/components/scUserSelect';

export default {
    name: 'scui_select_user',
    components: {
        scCode,
        scUserSelect,
    },
    data() {
        return {
            example1: {
                isOpenUser: false,
                code: `<sc-user-select ref="selectUser" v-model:selectOpen="isOpenUser"
                        @onSelect="selectUserRes"></sc-user-select>`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'Array', desc: '选中的人员', val: '', def: '' },
                { name: 'multiple', type: 'Boolean', desc: '是否支持多选', val: '', def: 'true' },
                { name: 'multipleLimit', type: 'Number', desc: '多选限制数量', val: '', def: '0' },
                { name: 'placeholder', type: 'String', desc: '占位符', val: '', def: '请选择' },
                { name: 'width', type: 'Number', desc: '选择器宽度', val: '', def: '750' },
                { name: 'hideInput', type: 'Boolean', desc: '是否隐藏输入框', val: '', def: 'false' },
                { name: 'selectOpen', type: 'Boolean', desc: '选择器是否打开', val: '', def: 'false' },
                { name: 'defaultValue', type: 'Array', desc: '默认选中的人员', val: '', def: '[]' },
                { name: 'ignore', type: 'Array', desc: '忽略的人员', val: '', def: '[]' },
            ],
            eventTable: [
                { name: 'onSelect', type: 'Function', desc: '选择人员事件', val: '', def: '' },
            ]
        };
    },
    methods: {
        selectUserRes(users) {
            this.isOpenUser = false;
            if (!users || users.length < 1) {
                return;
            }
        }
    }
}
</script>