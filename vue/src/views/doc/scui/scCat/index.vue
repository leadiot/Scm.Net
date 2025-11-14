<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <el-card>
                <h1>Cat 类别</h1>
                <p>Cat 类别用于显示应用的分类。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Cat 类别默认显示应用的分类。">
                    <sc-cat appId="00000000"></sc-cat>
                </sc-code>
            </el-card>
        </div>
    </div>
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