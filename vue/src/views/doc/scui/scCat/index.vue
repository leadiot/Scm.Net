<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>Cat 类别组件</h1>
                <p>Cat 类别组件用于显示应用的分类。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="Cat 类别组件默认显示应用的分类。">
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
            example1: {
                code: '<sc-cat appId="00000000"></sc-cat>',
            },
        };
    }
}
</script>