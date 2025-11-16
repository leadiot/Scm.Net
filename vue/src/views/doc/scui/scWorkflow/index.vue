<template>
    <div class="doc-content-wrapper">
        <div class="doc-content-container">
            <div class="doc-content">
                <el-card>
                    <h1>Workflow 工作流组件</h1>
                    <p>Workflow 组件用于添加工作流。</p>
                    <sc-code lang="html" :code="example1.code" title="基础用法" desc="Workflow 组件默认使用示例。">
                        <sc-workflow ref="workflow" v-model="example1.flow"></sc-workflow>
                    </sc-code>
                </el-card>
            </div>
        </div>
    </div>
</template>
<script>
import scCode from '@/components/scCode';
import scWorkflow from '@/components/scWorkflow';

export default {
    name: 'scui_workflow',
    components: {
        scCode,
        scWorkflow,
    },
    data() {
        return {
            example1: {
                name: "请假审批",
                flow: {
                    nodeName: "发起人",
                    type: 1,
                    nodeRoleList: [],
                    childNode: {
                        nodeName: "条件路由",
                        type: 4,
                        conditionNodes: [
                            {
                                nodeName: "长期",
                                type: 5,
                                priorityLevel: 1,
                                conditionMode: 1,
                                conditionList: [
                                    {
                                        label: "请假天数",
                                        field: "day",
                                        operator: ">",
                                        value: "7",
                                    },
                                ],
                                childNode: {
                                    nodeName: "领导审批",
                                    type: 2,
                                    setType: 1,
                                    nodeUserList: [
                                        {
                                            id: "360000197302144442",
                                            name: "何敏",
                                        },
                                    ],
                                    nodeRoleList: [],
                                    examineLevel: 1,
                                    directorLevel: 1,
                                    selectMode: 1,
                                    termAuto: false,
                                    term: 0,
                                    termMode: 1,
                                    examineMode: 1,
                                    directorMode: 0,
                                },
                            },
                            {
                                nodeName: "短期",
                                type: 5,
                                priorityLevel: 2,
                                conditionMode: 1,
                                conditionList: [],
                                childNode: {
                                    nodeName: "直接主管审批",
                                    type: 2,
                                    setType: 2,
                                    nodeUserList: [],
                                    nodeRoleList: [],
                                    examineLevel: 1,
                                    directorLevel: 1,
                                    selectMode: 1,
                                    termAuto: false,
                                    term: 0,
                                    termMode: 1,
                                    examineMode: 1,
                                    directorMode: 0,
                                },
                            },
                        ],
                        childNode: {
                            nodeName: "抄送人",
                            type: 3,
                            userSelectFlag: true,
                            nodeUserList: [
                                {
                                    id: "220000200908305857",
                                    name: "何秀英",
                                },
                            ],
                        },
                    },
                },
                code: `<sc-workflow ref="workflow" v-model="flow"></sc-workflow>`,
            },
        };
    },
    methods: {
    }
}
</script>