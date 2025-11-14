<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>Cat 类别</h1>
                <p>Cat 类别用于显示应用的分类。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Cat 类别默认显示应用的分类。">
                    <sc-cat appId="00000000"></sc-cat>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCat from '@/components/scCat';
import scCode from '@/components/scCode';

export default {
    name: 'scui_cat',
    components: {
        scCat,
        scCode,
    },
    data() {
        return {
            code1: '<sc-cat appId="00000000"></sc-cat>',
        };
    }
}
</script>

<style scoped>
.el-card {
    max-width: 960px;
    margin: 0 auto;
}
</style>