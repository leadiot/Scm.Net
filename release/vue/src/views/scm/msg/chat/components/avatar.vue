<template>
    <div class="portrait-container">
        <el-popover placement="right" :width="200" trigger="click">
            <template #reference>
                <el-avatar class="portrait" :size="50" :src="getAvatar()" />
            </template>
            <div class="info-popover">
                <sc-icon name="sc-menu-show" class="edit iconfont" @click="editHandle" />
                <el-avatar :size="70" :src="getAvatar()" />
                <div class="nickname-box ellipse-1">
                    <el-tooltip effect="dark" placement="top" :content="user.namec">
                        <span>{{ user.namec }}</span>
                    </el-tooltip>
                </div>
                <sc-icon name="sc-menu-show" @click="sendMsg"></sc-icon>
            </div>
        </el-popover>
    </div>
    <!-- <dialog-edit-information ref="refDialog" v-if="visible" /> -->
</template>
<script>
export default {
    emits: ['chatMe'],
    props: {
        user: { type: Object, default: null }
    },
    data() {
        return {
        }
    },
    mounted() {
    },
    methods: {
        editHandle() {

        },
        sendMsg() {
            this.$emit('chatMe');
        },
        getAvatar() {
            return this.$SCM.get_user_avatar(this.user);
        }
    }
}
</script>

<style lang="scss" scoped>
.portrait-container {
    padding: 15px 0;
    text-align: center;

    .portrait {
        cursor: pointer;
    }
}

.info-popover {
    text-align: center;
    position: relative;

    .edit {
        position: absolute;
        right: 0;
        font-size: 12px;
        cursor: pointer;
        animation: rotateAgainst360 .3s ease-out 0s;

        &:hover {
            color: #5cadff;
            animation: rotateAlong360 .4s ease-out 0s;
        }
    }

    .nickname-box {
        font-size: 14px;
        color: #606266;
        max-width: 100px;
        margin: 0 auto;
    }
}
</style>