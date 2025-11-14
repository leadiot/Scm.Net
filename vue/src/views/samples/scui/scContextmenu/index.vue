<template>
    <el-container>
        <el-main>
            <el-card>
                <h1>Contextmenu 上下文菜单</h1>
                <p>Contextmenu 上下文菜单用于显示上下文菜单示例。</p>
                <sc-code lang="html" :code="code1" title="基础用法" desc="Contextmenu 组件默认显示上下文菜单示例。">
                    <el-button type="primary" @click="cm">点击显示弹出菜单</el-button>
                    <sc-contextmenu ref="contextmenu">
                        <sc-contextmenu-item title="全选"></sc-contextmenu-item>
                        <sc-contextmenu-item title="复制"></sc-contextmenu-item>
                        <sc-contextmenu-item title="粘贴"></sc-contextmenu-item>
                        <sc-contextmenu-item title="多级菜单" divided>
                            <sc-contextmenu-item title="子菜单1"></sc-contextmenu-item>
                            <sc-contextmenu-item title="子菜单2"></sc-contextmenu-item>
                        </sc-contextmenu-item>
                        <sc-contextmenu-item title="关闭" divided></sc-contextmenu-item>
                    </sc-contextmenu>
                </sc-code>
            </el-card>
        </el-main>
    </el-container>
</template>
<script>
import scCode from '@/components/scCode';
import scContextmenu from "@/components/scContextmenu";
import scContextmenuItem from "@/components/scContextmenu/item";

export default {
    name: 'scui_contextmenu',
    components: {
        scCode,
        scContextmenu,
        scContextmenuItem,
    },
    data() {
        return {
            visible: false,
            top: 0,
            left: 0,
            code1: `<sc-contextmenu ref="contextmenu">
    <sc-contextmenu-item title="全选"></sc-contextmenu-item>
    <sc-contextmenu-item title="复制"></sc-contextmenu-item>
    <sc-contextmenu-item title="粘贴"></sc-contextmenu-item>
    <sc-contextmenu-item title="多级菜单" divided>
        <sc-contextmenu-item title="子菜单1"></sc-contextmenu-item>
        <sc-contextmenu-item title="子菜单2"></sc-contextmenu-item>
    </sc-contextmenu-item>
    <sc-contextmenu-item title="关闭" divided></sc-contextmenu-item>
</sc-contextmenu>
&lt;script scoped>
    shuowMenu(e){
        this.$refs.contextmenu.openMenu(e);
    }
&lt;/script>`.replaceAll('&lt;', '<'),
        };
    },
    methods: {
        menuClick(action) {
            console.log(action);
        },
        cm(e) {
            this.$refs.contextmenu.openMenu(e);
        }
    }
}
</script>

<style scoped>
.el-card {
    max-width: 960px;
    margin: 0 auto;
}
</style>