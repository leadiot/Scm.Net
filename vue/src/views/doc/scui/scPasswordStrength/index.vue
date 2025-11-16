<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>PasswordStrength 密码强度组件</h1>
                <p>PasswordStrength 组件用于密码强度的展示。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="PasswordStrength 组件默认使用示例。">
                    <el-form>
                        <el-form-item label="登录密码" prop="password">
                            <el-input v-model="example1.password" type="password" placeholder="请输入登录密码"></el-input>
                            <sc-password-strength v-model="example1.password"></sc-password-strength>
                        </el-form-item>
                    </el-form>
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
import scPasswordStrength from '@/components/scPasswordStrength';

export default {
    name: 'scui_password_strength',
    components: {
        scCode,
        scPasswordStrength,
    },
    data() {
        return {
            example1: {
                password: '',
                code: `<el-form>
    <el-form-item label="登录密码" prop="password">
        <el-input v-model="password" type="password" placeholder="请输入登录密码"></el-input>
        <sc-password-strength v-model="password"></sc-password-strength>
    </el-form-item>
</el-form>`,
            },
            fieldTable: [
                { name: 'modelValue', type: 'String', desc: '密码', val: '', def: '' },
            ],
            eventTable: [
            ]
        };
    },
    methods: {
    }
}
</script>