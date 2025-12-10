<template>
    <el-container>
        <el-aside>
            <el-container>
                <el-header>
                    <el-input class="search-input margin_r-10" prefix-icon="el-icon-search" placeholder="请输入名称或ID搜索"
                        v-model="key" />
                    <el-button circle>
                        <sc-icon name="sc-plus" :size="16" @click="addHandle" />
                    </el-button>
                </el-header>
                <el-main class="nopadding">
                    <sc-list :data="friendList" @change="change">
                        <template #item="{ item }">
                            <sc-summary :title="item.namec" :summary="item.update_time"
                                :image="getAvatar(item)"></sc-summary>
                        </template>
                    </sc-list>
                </el-main>
            </el-container>
        </el-aside>
        <el-main class="nopadding content">
            <transition name="el-fade-in-linear" mode="out-in">
                <friend-info ref="friend" :user="user" class="content-details width-220" @chatUser="chatUser"
                    v-if="user.id" />
            </transition>
        </el-main>
    </el-container>
</template>
<script>
import { defineAsyncComponent } from "vue";
export default {
    components: {
        friendInfo: defineAsyncComponent(() => import("./friendPage")),
    },
    emits: ['chatUser'],
    data() {
        return {
            apiObj: '',
            key: '',
            friendList: []
        }
    },
    props: {
        user: { type: Object, default: null },
        keywords: { type: [Number, String, Boolean], required: false }
    },
    mounted() {
        this.listFriends();
    },
    methods: {
        async listFriends() {
            var res = await this.$API.scmmsgchatfriend.list.get();
            if (!res || res.code != 200) {
                return;
            }

            var userId = this.user ? this.user.id : '1';
            var list = [{ id: userId, user_id: userId, namec: '我', type: 1 },
            { id: '1000000000000001002', user_id: '1000000000000001002', namec: '小木同学', type: 1 }];
            res.data.forEach(element => {
                list.push(element);
            });
            this.friendList = list;
        },
        addMsg() {
        },
        getAvatar(item) {
            return this.$SCM.get_user_avatar(item);
        },
        change(item) {
            this.$refs.friend.setUser(item);
        },
        addHandle() {
        },
        refresh() {
        },
        chatUser(user) {
            this.$emit('chatUser', user);
        }
    }
}
</script>
<style lang="scss" scoped>
@import '@/assets/sass/_variable.scss';

.content {
    display: flex;
    background-color: $lightColor-2;
    width: 100%;
    height: 100%;
}
</style>