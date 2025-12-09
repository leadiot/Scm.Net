<template>
    <div class="sc-list">
        <slot name="header">
            <div class="sc-list_header" v-if="header">{{ header }}</div>
        </slot>
        <el-empty :description="emptyText" :image-size="100" v-if="isEmpty()"></el-empty>
        <div class="sc-list_body" :style="{ padding: this.padding + 'px' }" v-else>
            <div v-for="(item, index) in data" :key="index" @click="itemClick(item, index)" class="sc-list-item"
                :class="{ actived: canSelected && currIndex == index }" :style="{ width: 100 / columns + '%' }">
                <span class="check" v-if="showCheckbox">
                    <el-checkbox v-model="item.checked" @change="itemChange(item, index)"></el-checkbox>
                </span>
                <slot name="item" :item="item" :index="index">
                    <div class="sc-list-item_label">
                        <sc-icon :name="icon" v-if="icon" :size="iconSize"></sc-icon>
                        <el-avatar :src="item.icon" v-if="item.icon" :size="iconSize" />
                        {{ item.label }}
                    </div>
                    <div class="sc-list-item_value">
                        <span v-if="item.value">{{ item.value }}</span>
                        <el-tag round v-if="item.count">{{ item.count }}</el-tag>
                    </div>
                </slot>
                <span class="opt" v-if="showOpt">
                    <sc-icon name="sc-edit-line" @click.stop="itemEdit(item, index)" v-if="showEdit" title="编辑" />
                    <sc-icon name="sc-delete-line" @click.stop="itemRemove(item, index)" v-if="showDrop" title="删除" />
                </span>
            </div>
        </div>
        <slot name="footer">
            <div class="sc-list_footer" v-if="footer">{{ footer }}</div>
        </slot>
    </div>
</template>
<script>
export default {
    name: "ScList",
    emits: ["change", "editItem", "dropItem"],
    data() {
        return {
            currIndex: -1,
            currItem: null,
        }
    },
    props: {
        header: { type: String, default: '' },
        footer: { type: String, default: '' },
        icon: { type: String, default: '' },
        iconSize: { type: Number, default: 24 },
        data: { type: Array, default: function () { return []; } },
        width: { type: String, default: "" },
        canSelected: { type: Boolean, default: true },
        selectedIndex: { type: Number, default: -1 },
        showOpt: { type: Boolean, default: false },
        showEdit: { type: Boolean, default: false },
        showDrop: { type: Boolean, default: false },
        showCheckbox: { type: Boolean, default: false },
        columns: { type: Number, default: 1 },
        padding: { type: Number, default: 0 },
        emptyText: { type: String, default: "" },
    },
    created() {
    },
    mounted() {
        this.currIndex = this.selectedIndex;
    },
    methods: {
        getWidth() {
            if (this.width) {
                return "width:" + this.width
            }
        },
        itemClick(item, index) {
            if (this.currIndex == index) {
                return;
            }

            this.currIndex = index;
            item.checked = !item.checked;
            this.$emit("change", item, index);
        },
        itemEdit(item, index) {
            this.$emit('editItem', item, index);
        },
        itemRemove(item, index) {
            this.$emit('dropItem', item, index);
        },
        itemChange(item, index) {
            this.$emit('change', item, index);
        },
        isEmpty() {
            return this.data == null || this.data.length < 1;
        }
    }
}
</script>

<style type="scss" scoped>
.sc-list {
    width: 100%;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    background-color: var(--el-fill-color-blank);
    border: 1px solid var(--el-border-color-lighter);
    border-radius: var(--el-border-radius-base);

    .sc-list_header {
        border-bottom: 0;
        font-size: 17px;
        font-weight: bold;
        padding: 15px 20px;
    }

    .sc-list_body {
        overflow-y: auto;
        flex-basis: 100%;

        .actived {
            background-color: var(--el-color-primary-light-9);
        }
    }

    .sc-list_footer {
        color: var(--el-text-color-primary);
        font-size: 16px;
        font-weight: 700;
    }

    .sc-list-item {
        display: flex;
        /* flex: 1; */
        align-items: center;
        justify-content: space-between;
        font-size: 14px;
        padding: 10px;
        background-color: var(--el-fill-color-blank);

        .sc-list-item_label {
            flex-basis: 100%;
            display: flex;
            align-items: center;
            gap: 5px;
        }

        .sc-list-item_value {
            color: lightgray;
        }

        .check {
            margin-right: 5px;
            width: 20px;
        }

        .opt {
            display: none;

            i {
                margin-left: 5px;
                color: #999;
            }

            i:hover {
                color: #333;
            }
        }
    }

    .sc-list-item:hover {
        background-color: var(--el-fill-color-light);
    }

    .sc-list-item:hover .opt {
        display: inline-block;
    }
}
</style>
