<template>
    <el-container>
        <el-main class="nopadding">
            <sc-editor ref="scEditor" :content="html" />
        </el-main>
    </el-container>
</template>

<script>
import scEditor from '@/components/scEditor/rich.vue';

export default {
    name: 'scui_editor',
    components: {
        scEditor,
    },
    data() {
        return {
            html: '123',
        };
    },
    mounted() {
    },
    methods: {
    }
}
</script>