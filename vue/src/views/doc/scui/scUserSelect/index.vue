<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Select User 人员选择器组件</h1>
                <p>Select User 组件用于选择人员的展示。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Select User 组件默认使用示例。">
                    <sc-select-user ref="selectUser" v-model:selectOpen="example1.isOpenUser"
                        @onSelect="selectUserRes"></sc-select-user>
                </sc-code>
            </el-card>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scSelectUser from '@/components/scSelectUser';

export default {
    name: 'scui_select_user',
    components: {
        scCode,
        scSelectUser,
    },
    data() {
        return {
            example1: {
                isOpenUser: false,
                code: `<sc-select-user ref="selectUser" v-model:selectOpen="isOpenUser"
                        @onSelect="selectUserRes"></sc-select-user>`,
            },
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