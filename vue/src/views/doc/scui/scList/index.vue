<template>
    <div class="doc-content">
        <div class="doc-content__body">
            <el-card>
                <h1>List 列表组件</h1>
                <p>用于显示一个列表数据。</p>
                <sc-code lang="html" :code="example1.code" title="基础用法" desc="用于显示一个列表数据。">
                    <sc-list :data="example1.data" />
                </sc-code>
                <sc-code lang="html" :code="example2.code" title="带图标" desc="用于显示一个列表数据，可以指定图标。">
                    <sc-list :data="example2.data" icon="sc-folder-open-line" />
                </sc-code>
                <sc-code lang="html" :code="example3.code" title="显示头信息" desc="用于显示一个列表数据，可以指定图标。">
                    <sc-list :data="example3.data" icon="sc-folder-open-line">
                        <template #header>
                            <div style="font-size: 17px;padding: 15px 20px;">
                                <sc-icon name="sc-palette-line"></sc-icon>
                                颜色
                            </div>
                        </template>
                    </sc-list>
                </sc-code>
                <sc-code lang="html" :code="example4.code" title="显示尾信息" desc="用于显示一个列表数据，可以指定图标。">
                    <sc-list :data="example4.data" icon="sc-folder-open-line" header="颜色">

                        <template #footer>
                            <div style="text-align: right;padding: 5px; color: #999;">共3种可用颜色</div>
                        </template>
                    </sc-list>
                </sc-code>
                <h2>ScList属性</h2>
                <el-table :data="fieldTable" style="width: 100%">
                    <el-table-column prop="name" label="属性名" width="120" />
                    <el-table-column prop="desc" label="说明" />
                    <el-table-column prop="type" label="类型" width="80" />
                    <el-table-column prop="val" label="可选值" width="120" />
                    <el-table-column prop="def" label="默认值" width="120" />
                </el-table>
                <h2>ScList事件</h2>
                <el-table :data="eventTable" style="width: 100%">
                    <el-table-column prop="name" label="事件名" width="120" />
                    <el-table-column prop="desc" label="说明" />
                    <el-table-column prop="args" label="回调参数" width="120" />
                </el-table>
            </el-card>
        </div>
    </div>
</template>

<script>
import "@/assets/code.scss";
import scCode from "@/components/scCode";

export default {
    name: 'scui_list',
    components: {
        scCode: scCode
    },
    data() {
        return {
            // 基础用法
            example1: {
                code: `\x3csc-list :data="list" /\x3e
\x3cscript\x3e 
export default{
    data(){
        return {
            list:[{ id: 1, label: '红色', value: '1' },
                { id: 2, label: '黄色', value: '2' },
                { id: 3, label: '蓝色', value: '3' }
            ]
        }
    }
}
\x3c/script\x3e`,
                data: [{ id: 1, label: '红色', value: '1' },
                { id: 2, label: '黄色', value: '2' },
                { id: 3, label: '蓝色', value: '3' }
                ],
            },
            // 带图标
            example2: {
                code: `\x3csc-list :data="list" icon="sc-folder-open-line" /\x3e
\x3cscript\x3e 
export default{
    data(){
        return {
            list:[{ id: 1, label: '红色', value: '1' },
                { id: 2, label: '黄色', value: '2' },
                { id: 3, label: '蓝色', value: '3' }
            ]
        }
    }
}
\x3c/script\x3e`,
                data: [{ id: 1, label: '红色', value: '1' },
                { id: 2, label: '黄色', value: '2' },
                { id: 3, label: '蓝色', value: '3' }
                ]
            },
            // 带图标
            example3: {
                code: `\x3csc-list :data="list" icon="sc-folder-open-line" \x3e
    <template #header>
        <div style="font-size: 17px;padding: 15px 20px;">
            <sc-icon name="sc-palette-line"></sc-icon>
            颜色
        </div>
    </template>
</sc-list>
\x3cscript\x3e 
export default{
    data(){
        return {
            list:[{ id: 1, label: '红色', value: '1' },
                { id: 2, label: '黄色', value: '2' },
                { id: 3, label: '蓝色', value: '3' }
            ]
        }
    }
}
\x3c/script\x3e`,
                data: [{ id: 1, label: '红色', value: '1' },
                { id: 2, label: '黄色', value: '2' },
                { id: 3, label: '蓝色', value: '3' }
                ]
            },
            // 带图标
            example4: {
                code: `\x3csc-list :data="list" icon="sc-folder-open-line" header="颜色">
    <template #footer>
        <div style="text-align: right;padding: 5px; color: #999;">共3种可用颜色</div>
    </template>
</sc-list>
\x3cscript\x3e 
export default{
    data(){
        return {
            list:[{ id: 1, label: '红色', value: '1' },
                { id: 2, label: '黄色', value: '2' },
                { id: 3, label: '蓝色', value: '3' }
            ]
        }
    }
}
\x3c/script\x3e`,
                data: [{ id: 1, label: '红色', value: '1' },
                { id: 2, label: '黄色', value: '2' },
                { id: 3, label: '蓝色', value: '3' }
                ]
            },
            fieldTable: [
                { name: 'header', type: 'String', desc: '列表头', val: '', def: '' },
                { name: 'footer', type: 'String', desc: '列表尾', val: '', def: '' },
                { name: 'icon', type: 'String', desc: '列表项图标', val: '', def: '' },
                { name: 'data', type: 'Array', desc: '列表数据', val: '', def: '' },
                { name: 'width', type: 'Number', desc: '列表宽度', val: '', def: '' },
                { name: 'columns', type: 'Number', desc: '列表显示列数', val: '', def: '1' },
                { name: 'padding', type: 'Number', desc: '列表内衬大小', val: '', def: '0' },
                { name: 'emptyText', type: 'String', desc: '为空时显示文本', val: '', def: '内容为空' },
                { name: 'showOpt', type: 'Boolean', desc: '是否显示操作面板', val: '', def: 'false' },
                { name: 'showEdit', type: 'Boolean', desc: '是否显示编辑按钮', val: '', def: 'false' },
                { name: 'showDrop', type: 'Boolean', desc: '是否显示删除按钮', val: '', def: 'false' }
            ],
            eventTable: [
                { name: 'change', desc: '选择项变化事件', args: 'item,index' },
                { name: 'editItem', desc: '列表项编辑事件', args: 'item,index' },
                { name: 'dropItem', desc: '列表项删除事件', args: 'item,index' },
            ]
        }
    },
    mounted() {
    },
    methods: {
    },
}
</script>