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
                    <sc-list :data="chatList" @change="change" :showOpt="true" :showDrop="true" @dropItem="dropItem">
                        <template #item="{ item }">
                            <sc-summary :title="item.namec" :summary="$TOOL.dateTimeFormat(item.update_time)"
                                :image="$SCM.get_user_avatar(item)"></sc-summary>
                        </template>
                    </sc-list>
                </el-main>
            </el-container>
        </el-aside>
        <el-main class="nopadding content">
            <transition name="el-fade-in-linear" mode="out-in">
                <chatPage ref="chatPage" :user="user" class="content-message" @chatUser="chatUser" />
            </transition>
        </el-main>
    </el-container>
    <select-user hide-input v-model:selectOpen="isOpenUser" @onSelect="selectUserRes"></select-user>
</template>
<script>
import { defineAsyncComponent } from "vue";
export default {
    emits: ['chatUser'],
    components: {
        selectUser: defineAsyncComponent(() => import("@/components/scUserSelect")),
        chatPage: defineAsyncComponent(() => import("./chatPage")),
    },
    data() {
        return {
            apiObj: '',
            key: '',
            chatList: [],
            isOpenUser: false
        }
    },
    props: {
        user: { type: Object, default: () => { } },
        keywords: { type: [Number, String, Boolean], required: false }
    },
    mounted() {
        this.listChats();
    },
    methods: {
        async listChats() {
            var res = await this.$API.scmmsgchatmessage.list_header.get();
            if (!res || res.code != 200) {
                return;
            }

            this.chatList = res.data;
        },
        addMsg(msg) {
            for (var i = 0; i < this.chatList.length; i += 1) {
                if (msg.chat_id == this.chatList[i].id) {
                    this.chatList[i].qty += 1;
                    break;
                }
            }

            this.$refs.chatPage.addMsg(msg);
        },
        change(chat) {
            this.$refs.chatPage.setChat(chat);
        },
        openSelectUser() {
            if (this.isOpenUser) {
                this.isOpenUser = false;
            }
            this.isOpenUser = true;
        },
        selectUserRes(users) {
            this.isOpenUser = false;
            if (!users || users.length < 1) {
                return;
            }

            var ids = [this.user.id];
            var name = this.user.namec;
            users.forEach(element => {
                ids.push(element.id);
                name += ',' + element.namec;
            });
            if (name.length > 64) {
                name = name.substring(0, 64);
            }
            var res = this.$API.scmmsgchatmessage.create.post({ types: 2, users: ids, namec: name });
            if (!res || res.code != 200) {
                return;
            }

            this.chatList.unshift(res.data);
            //this.erceipt = res;
        },
        addHandle() {
            if (this.isOpenUser) {
                this.isOpenUser = false;
            }
            this.isOpenUser = true;
        },
        chatUser(user) {
            this.$emit('chatUser', user);
        },
        refresh() {
            this.listChats();
        },
        dropItem(item) {
            alert(item.id);
        }
    }
}
</script>
<style lang="scss" scoped>
@use '@/assets/sass/_variable.scss' as *;

.content {
    display: flex;
    background-color: $lightColor-2;
    width: 100%;
    height: 100%;

    &-message {
        flex: 1
    }
}
</style>