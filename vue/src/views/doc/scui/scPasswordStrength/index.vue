<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>PasswordStrength 密码强度组件</h1>
                <p>PasswordStrength 组件用于密码强度的展示。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="PasswordStrength 组件默认使用示例。">
                    <el-form>
                        <el-form-item label="登录密码" prop="password">
                            <el-input v-model="password" type="password" placeholder="请输入登录密码"></el-input>
                            <sc-password-strength v-model="password"></sc-password-strength>
                        </el-form-item>
                    </el-form>
                </sc-code>
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
            password: '',
            code1: `<el-form>
    <el-form-item label="登录密码" prop="password">
        <el-input v-model="password" type="password" placeholder="请输入登录密码"></el-input>
        <sc-password-strength v-model="password"></sc-password-strength>
    </el-form-item>
</el-form>`,
        };
    },
    methods: {
    }
}
</script>