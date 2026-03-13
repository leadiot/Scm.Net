<template>
    <el-container>
        <el-main>
            <el-container>
                <el-header>
                    <el-row>
                        <el-col :span="8">
                            {{ header.codes }}
                        </el-col>
                        <el-col :span="8">
                            {{ flow_data.types }}/{{ flow_data.result }}
                        </el-col>
                    </el-row>
                </el-header>
                <el-main class="nopadding">
                    <scTable ref="table" :table-name="tableName" :api-obj="apiObj" :column="column" row-key="id">
                        <el-table-column label="#" type="index" width="50"></el-table-column>
                    </scTable>
                </el-main>
            </el-container>
        </el-main>
        <el-aside width="280px">
            <el-container>
                <el-main class="nopadding">
                    <el-timeline>
                        <el-timeline-item placement="top" v-for="item in result_list" :key="item.id"
                            :timestamp="item.date">
                            <el-card shadow="never">
                                <h4>版本：{{ item.ver }}（BUILD:{{ item.build }}）</h4>
                                <p v-html="toHtml(item.remark)"></p>
                            </el-card>
                        </el-timeline-item>
                    </el-timeline>
                </el-main>
                <el-footer>
                    <el-button type="primary" @click="accept">通过</el-button>
                    <el-button @click="reject">拒绝</el-button>
                </el-footer>
            </el-container>
        </el-aside>
    </el-container>
</template>
<script>
import { useRoute } from "vue-router";

export default {
    data() {
        return {
            title: '流程审批',
            tableName: 'samples_po_flow',
            apiObj: this.$API.samplespodetail.page,
            param: {
                id: this.$SCM.DEF_ID,
                row_status: this.$SCM.DEF_STATUS,
                create_time: '',
                key: ''
            },
            selection: [],
            column: [
                { label: "id", prop: "id", hide: true },
                { prop: 'book_codes', label: '书籍编码', width: 100 },
                { prop: 'book_names', label: '书籍名称', width: 140, align: 'left' },
                { prop: 'need_qty', label: '需求数量', width: 120, align: 'right' },
                { prop: 'real_qty', label: '实际数量', width: 100, align: 'right' },
                { prop: 'remark', label: '备注', minWidth: 140, align: 'left' },
                { prop: "update_names", label: "更新人员", width: 100, },
                { prop: "update_times", label: "更新时间", width: 160 },
                { prop: "create_names", label: "创建人员", width: 100, },
                { prop: "create_times", label: "创建时间", width: 160 },
            ],
            header: {},
            flow_data: {},
            result_list: []
        };
    },
    mounted() {
        this.init();
    },
    methods: {
        async init() {
            var route = useRoute();
            var id = route.query.id;
            console.log(id);
            if (!this.$SCM.is_valid_id(id)) {
                console.log('无效的id');
                return;
            }

            this.param.id = id;

            var res = await this.$API.scmsysflowdata.view.get(id);
            if (!res || res.code != 200) {
                return;
            }
            this.flow_data = res.data;

            res = await this.$API.samplespoheader.view.get(res.data.order_id);
            if (!res || res.code != 200) {
                return;
            }

            this.header = res.data;
            this.search();
        },
        complete() {
            this.$refs.table.refresh();
        },
        search() {
            this.$refs.table.upData(this.param);
        },
        accept() {
            alert('accept');
        },
        reject() {
            alert('reject');
        }
    },
};
</script>