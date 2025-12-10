<template>
    <el-container>
        <el-main class="nopadding">
            <div class="friend-details-box" style="padding-top: 20px;" v-show="user.id">
                <el-avatar :size="160" :src="user.avatar" />
                <p class="nickname">{{ user.namec }}</p>
                <el-divider content-position="right" />
            </div>
        </el-main>
        <el-footer>
            <div class="friend-opt">
                <el-button type="primary" @click="sendMsg">发送消息</el-button>
                <el-button type="danger" @click="removeHandle">删除好友</el-button>
            </div>
        </el-footer>
    </el-container>
</template>
<script>
export default {
    emits: ['chatUser'],
    data() {
        return {
            user: {},
        }
    },
    methods: {
        setUser(user) {
            this.user = user;
        },
        sendMsg() {
            this.$emit('chatUser', this.user);
        },
        removeHandle() {
            this.$message.warning('暂不支持删除！');
        }
    }
}
</script>
<style lang="scss" scoped>
@import '@/assets/sass/_variable.scss';

.friend-details-box {
    display: flex;
    flex-direction: column;
    align-items: center;

    .nickname {
        font-size: 20px;
        color: $darkColor-8;
        font-weight: bold;
    }
}

.friend-opt {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
}
</style>