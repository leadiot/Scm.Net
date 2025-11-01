<template>
    <el-container>
        <el-main>
            <div class="notepad">
                <el-card>
                    <div class="notepad-title">
                        <input v-model="formData.title" :placeholder="titlePlaceholder" />
                    </div>
                    <div class="notepad-content">
                        <el-input v-model="formData.content" type="textarea" rows="30" class="content"
                            :placeholder="contentPlaceholder"></el-input>
                    </div>
                </el-card>
            </div>
        </el-main>
        <el-footer>
            <div class="notepad-footbar">
                <el-space>
                    <label>文章分类</label>
                    <sc-select v-model="formData.cat_id" :data="cat_list"></sc-select>
                    <el-button type="primary" @click="newNote">
                        <sc-icon set="sc" name="sc-file-line"></sc-icon>
                        新建
                    </el-button>
                    <el-button type="danger" @click="save">
                        <sc-icon set="ms" name="save"></sc-icon>
                        保存
                    </el-button>
                </el-space>
            </div>
        </el-footer>
    </el-container>
</template>
<script>
export default {
    name: 'cms_doc_notepad',
    data() {
        return {
            formData: this.def_data(),
            cat_list: [],
            titlePlaceholder: '请输入标题……',
            contentPlaceholder: '请输入内容……',
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        def_data() {
            return {
                id: this.$SCM.DEF_ID,
                cat_id: '0',
                title: '',
                content: ''
            }
        },
        init() {
            this.listCat();
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
        newNote() {
            this.formData = this.def_data();
        },
        async save() {
            var res = await this.$API.scmsysnote.save.post(this.formData);
            if (!res || res.code != 200) {
                this.$message.error(res.message);
                return;
            }

            this.formData.id = res.data.data;
            this.$message.success("数据已保存！");
        }
    }
}
</script>
<style type="scss" scoped>
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

    .notepad-content {

        textarea {
            border: 0;
        }
    }
}

.notepad-footbar {
    margin-top: -4px;
    text-align: center;
    align-items: center;
}
</style>