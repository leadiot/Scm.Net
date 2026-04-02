<template>
    <div class="chat">
        <el-container>
            <el-aside width="80px">
                <div class="sidebar">
                    <avatar :user="user" @chatMe="chatMe" />
                    <el-scrollbar>
                        <ul>
                            <li class="active-line" :style="{ top: active * 70 + 'px' }" />
                            <li v-for="(icon, index) in icons" :key="index" @click="clickHandle(index)"
                                :class="active === index ? 'active' : ''">
                                <span class="unread" v-if="(index === 0 && unreadCount) || (index === 3 && applyCount)">
                                    {{ index === 0 ? unreadCount : applyCount }}
                                </span>
                                <sc-icon :name="icon" />
                            </li>
                            <li @click="exitHandle">
                                <span class="iconfont icon-exit" />
                            </li>
                        </ul>
                    </el-scrollbar>
                </div>
            </el-aside>
            <el-main class="nopadding">
                <!-- <transition name="el-fade-in-linear" mode="out-in">
                                <keep-alive> -->
                <component ref="myView" :is="activeView" :key="active" :user="user" @chatUser="chatUser" />
                <!-- </keep-alive>
                            </transition> -->
            </el-main>
        </el-container>
    </div>
</template>

<script>
import { defineAsyncComponent } from "vue";
import socket from "@/utils/socket";

export default {
    name: 'scm_msg_chat',
    components: {
        avatar: defineAsyncComponent(() => import("./components/avatar")),
        chatList: defineAsyncComponent(() => import("./components/chatList")),
        friendList: defineAsyncComponent(() => import("./components/friendList")),
    },
    data() {
        return {
            user: {
                id: this.$SCM.DEF_ID,
                account: "user@c-scm.net",
                namec: "我",
                avatar: "",
                sex: "男",
                summary: "",
                role_list: [],
                position_list: [],
            },
            icons: ['sc-wechat-line', 'sc-user-line', 'sc-bell-line'],
            active: 0,
            activePages: ['chatList', 'friendList', 'noticeList'],
            activeView: 'chatList',
            unreadCount: 0,
            applyCount: 0,
            keywords: '',
        }
    },
    mounted() {
        this.init();
        this.openWss();
    },
    methods: {
        async init() {
            const res = await this.$API.operator.userwork.get();
            if (!res || res.code != 200) {
                return;
            }
            this.user = res.data;
        },
        openWss() {
            socket.open_wss("ChatMessage", this.addMsg);
        },
        addMsg(msg) {
            if (!msg || msg.code != 200) {
                return;
            }

            var view = this.$refs.myView;
            if (view && view.addMsg) {
                view.addMsg(msg.data);
            }
        },
        clickHandle(index) {
            this.active = index;
            this.activeView = this.activePages[index];
        },
        /**
         * 头像使用，给自己发消息
         */
        async chatMe() {
            this.chatUser(this.user);
        },
        /**
         * 好友使用，给好友发消息
         * @param {user} user 
         */
        async chatUser(user) {
            var res = await this.$API.scmmsgchatmessage.add.post({ types: 1, users: [user.id], namec: user.namec });
            if (!res || res.code != 200) {
                return;
            }

            var chat = res.data;
            res = await this.$API.scmmsgchatmessage.list_detail.get({ id: chat.id });
            if (!res || res.code != 200) {
                return;
            }

            this.clickHandle(0);
            this.$refs.myView.change(this.user);
        },
        async chatMsg(chat) {
            this.$refs.message.setChat(chat);
        },
        addHandle() {
            this.$refs.myView.addHandle();
        },
        refresh() {

        },
    }
}
</script>

<style lang="scss" scoped>
@use '@/assets/sass/_variable.scss';
$height: 70px;

.chat {
    width: 80%;
    height: 100%;
    margin: 0 auto;
}

.sidebar {
    background-color: #303841;
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    overflow: hidden;

    ul {
        position: relative;

        li {
            position: relative;
            z-index: 1;
            height: $height;
            line-height: $height;
            width: 100%;
            color: #a5b5c1;
            cursor: pointer;
            text-align: center;
            font-size: 32px;

            .unread {
                position: absolute;
                top: 10px;
                left: 10px;
                padding: 0px 6px;
                font-size: 12px;
                line-height: 18px;
                color: white;
                border-radius: 10px;
                background-color: red;
            }

            span {
                font-size: 30px;
            }
        }

        .active-line {
            position: absolute;
            z-index: 0;
            width: 100%;
            border-left: 4px solid #5cadff;
            transition: .3s;
            background-color: #363e47;
        }

        .active {
            color: #ffffff;
        }
    }
}
</style>