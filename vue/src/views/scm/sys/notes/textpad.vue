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
                <el-main class="doc-guid">
                    <sc-list :data="note_list" style="height: 100%;" @dropItem="itemDelete" :canSelected="true">
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
                <el-main>
                    <div class="notepad">
                        <el-card>
                            <div class="notepad-title">
                                <input ref="title" v-model="formData.title" :placeholder="titlePlaceholder"
                                    @keydown.enter="handleEnter" />
                            </div>
                            <div class="notepad-content">
                                <el-input ref="content" v-model="formData.content" type="textarea" :rows="30"
                                    class="content" :placeholder="contentPlaceholder"></el-input>
                            </div>
                        </el-card>
                    </div>
                </el-main>
                <el-footer>
                    <div class="doc-tool">
                        <el-space>
                            <label>文章分类</label>
                            <sc-select v-model="formData.cat_id" :data="cat_list"></sc-select>
                            <el-button type="primary" @click="newNote">
                                <sc-icon set="sc" name="sc-file-line"></sc-icon>新建
                            </el-button>
                            <el-button type="danger" @click="saveNote">
                                <sc-icon set="ms" name="save"></sc-icon>保存
                            </el-button>
                            <el-button type="danger" @click="loadRemote">
                                使用服务端
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
    name: 'cms_doc_textpad',
    data() {
        return {
            param: {
                types: 1,
                key: ''
            },//查询条件
            cat_list: [],
            note_list: [],//笔记列表
            formData: this.def_data(),
            timer: null,// 定时器
            loading: false,//加载标识
            saving: false,// 保存标识
            titlePlaceholder: '请输入标题……',
            contentPlaceholder: '请输入内容……',
        }
    },
    mounted() {
        this.listCat();
        this.search();

        this.formData = this.loadCache('0');
        this.showNote();

        this.timer = setInterval(() => {
            this.saveCache();
        }, 1000);
    },
    unmounted() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    },
    methods: {
        def_data() {
            return {
                id: this.$SCM.DEF_ID,
                ver: 0,
                types: 1,
                title: '',
                content: '',
                cat_id: '0',
            }
        },
        async search() {
            var res = await this.$API.scmsysnote.list.get(this.param);
            if (!res || res.code != 200) {
                return;
            }

            this.note_list = res.data;
        },
        async listCat() {
            var res = await this.$API.scmrescat.list.get({ 'app_id': '1789632103915327488' });
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

            this.readNote(item.id, false);
        },
        loadRemote(){
            if(!this.formData){
                return;
            }
            
            this.readNote(this.formData.id, true);
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
        handleEnter() {
            this.$refs.content.focus();
        },
        showNote() {

        },
        async readNote(itemId, force) {
            if (!itemId) {
                return;
            }

            this.loading = true;

            // 读取远程数据
            var res = await this.$API.scmsysnote.model.get(itemId);
            if (!res || res.code != 200) {
                this.loading = false;
                return;
            }

            // 读取本地缓存
            if (force) {
                this.formData = res.data;
            }
            else {
                var cahced = this.loadCache(itemId);

                var nativeVer = cahced.ver || 0;
                var remoteVer = res.data.ver || 1;
                if (nativeVer >= remoteVer) {
                    this.formData = cahced;
                } else {
                    this.formData = res.data;
                }
            }
            this.showNote();

            this.loading = false;
        },
        newNote() {
            var tmp = this.loadCache(this.formData.id);
            var changed = false;
            if (tmp) {
                changed = tmp.id == '0' || this.formData.title != tmp.title || this.formData.content != tmp.content || this.formData.cat_id != tmp.cat_id;
            }

            if (!changed) {
                this.formData = this.def_data();
                this.showNote();
                return;
            }

            this.$confirm(`您有数据未保存，确认要新建文档吗？`, "提示", {
                type: "warning",
                confirmButtonText: "确定",
                cancelButtonText: "取消",
            })
                .then(() => {
                    this.formData = this.def_data();
                    this.showNote();
                })
                .catch(() => { });
        },
        async saveNote() {
            if (!this.formData.types) {
                this.$message.warning('请选择文章类型！');
                return;
            }

            if (!this.formData.content) {
                this.$message.warning('请输入文章内容！');
                return;
            }

            if (!this.formData.title) {
                this.formData.title = '未命名：' + this.$TOOL.dateTimeFormat(new Date());
            }

            this.saving = true;
            var res = await this.$API.scmsysnote.save.post(this.formData);
            if (!res || res.code != 200) {
                this.$message.error(res.message);
                this.saving = false;
                return;
            }

            this.formData.id = res.data.id;
            this.formData.ver = res.data.ver;
            this.$message.success("数据已保存！");

            // 暂不删除本地
            // this.$SCM.drop_cache(this.formData.id);

            this.saving = false;
            this.saveCache();

            this.search();
        },
        loadCache(id) {
            var cached = this.$SCM.read_json('textpad_' + id);
            if (!cached) {
                cached = this.def_data();
            }
            return cached;
        },
        saveCache() {
            if (this.saving) {
                return;
            }

            this.$SCM.save_cache('textpad_' + this.formData.id, this.formData);
        },
    }
}
</script>
<style type="scss" scoped>
.doc {
    .doc-guid {
        background-color: var(--el-bg-color);
        padding: 0px;
    }

    .doc-tool {
        text-align: center;
        align-items: center;
        margin-top: -4px;
    }

    .notepad {
        width: 860px;
        margin: 0 auto;

        .notepad-title {
            padding: 10px 0 20px 0;
            /* border-bottom: 1px solid #e8e8e8; */

            input {
                font-size: 30px;
                border: 0;
                outline: none;
                width: 100%;
                line-height: 1;
            }
        }

        .notepad-content {}
    }
}
</style>
