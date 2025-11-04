<template>
    <el-container class="doc">
        <el-aside style="width: 320px;">
            <el-container>
                <el-header>
                    <el-input placeholder="输入关键字进行过滤" v-model="param.key" clearable>
                        <template #append>
                            <el-button icon="el-icon-search" @click="search()" />
                        </template>
                    </el-input>
                </el-header>
                <el-main class="guid">
                    <sc-list :data="article_list" style="height: 100%;" @dropItem="itemDelete" :canSelected="true">
                        <template #item="{ item }">
                            <sc-summary :title="item.title" :summary="this.$TOOL.dateTimeFormat(item.create_time)"
                                @click="itemClick(item)"></sc-summary>
                        </template>
                    </sc-list>
                </el-main>
            </el-container>
        </el-aside>
        <el-main style="padding: 0px;">
            <el-container>
                <el-header>
                    <slot name="header"></slot>
                </el-header>
                <el-main>
                    <slot></slot>
                </el-main>
                <el-footer>
                    <div class="footbar">
                        <el-space>
                            <label>文章分类</label>
                            <sc-select v-model="cat_id" :data="cat_list"></sc-select>
                            <el-button type="primary" @click="newNote">
                                <sc-icon set="sc" name="sc-file-line"></sc-icon>新建
                            </el-button>
                            <el-button type="danger" @click="saveNote">
                                <sc-icon set="ms" name="save"></sc-icon>保存
                            </el-button>
                        </el-space>
                    </div>
                </el-footer>
            </el-container>
        </el-main>
    </el-container>
</template>

<script>
export default {
    emits: ['readNote', 'saveNote', 'newNote'],
    props: {
        types: { type: Number, default: 0 }
    },
    data() {
        return {
            param: {
                types: 0,
                key: ''
            },
            cat_id: '0',
            cat_list: [],
            loading: false,
            article_list: []
        }
    },
    mounted() {
        this.listCat();
        this.search();
    },
    methods: {
        async search() {
            this.param.types = this.types;
            var res = await this.$API.scmsysnote.list.get(this.param);
            if (!res || res.code != 200) {
                return;
            }

            this.article_list = res.data;
        },
        async listCat() {
            var res = await this.$API.cmsrescat.list.get();
            if (!res || res.code != 200) {
                return;
            }

            this.cat_list.push({ id: this.$SCM.DEF_ID, value: '0', label: '默认' });
            res.data.forEach(element => {
                this.cat_list.push({ id: element.id, value: element.id, label: element.namec });
            });
        },
        itemClick(item) {
            if (!item) {
                return;
            }

            this.$emit('readNote', item);
        },
        itemDelete(item) {
            if (!item || !item.id) {
                return;
            }

            this.$confirm(
                `确定要删除文章 ${item.title} 吗？`,
                "提示",
                {
                    type: "warning",
                    confirmButtonText: "确定",
                    cancelButtonText: "取消",
                }
            ).then(async () => {
                var loading = this.$loading();
                var res = await this.$API.sysnotes.status.post({ 'ids': [item.id], 'status': 2 });
                if (!res || res.code != 200) {
                    this.$alert(res.message, "提示", { type: "error" });
                    return;
                }

                loading.close();
                this.search();
            }).catch(() => { });
        },
        newNote() {
            this.$emit('newNote');
        },
        saveNote() {
            this.$emit('saveNote', this.cat_id);
        }
    }
}
</script>
<style type="scss" scoped>
.doc {
    .guid {
        background-color: var(--el-bg-color);
        padding: 0px;
    }

    .footbar {
        text-align: center;
        align-items: center;
        margin-top: -4px;
    }
}
</style>
