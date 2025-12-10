<template>
    <el-card>
        <div class="sc-comment">
            <div class="post">
                <div class="head">
                    <h3>发表评论</h3>
                </div>
                <div class="comment">
                    <myComment @save="saveComment"></myComment>
                </div>
            </div>
            <div class="list">
                <div class="head">
                    <el-row>
                        <el-col :span="12">
                            <h3>全部评论 {{ comment.qty }}</h3>
                        </el-col>
                        <el-col :span="12" style="text-align: right;">
                            <el-radio-group v-model="sort">
                                <el-radio-button value="0">默认</el-radio-button>
                                <el-radio-button value="1">最新</el-radio-button>
                                <el-radio-button value="2">最热</el-radio-button>
                            </el-radio-group>
                        </el-col>
                    </el-row>
                </div>
                <div class="sc-comment-list">
                    <div class="sc-comment-item" v-for="item in list" :key="item.id">
                        <div class="left" width="60px">
                            <el-avatar class="header-img" :size="40" :src="getAvatar(item.avatar)"></el-avatar>
                        </div>
                        <div class="body">
                            <el-row>
                                <el-col :span="20" style="text-align: left;">
                                    {{ item.namec }}
                                </el-col>
                                <el-col :span="4" style="text-align: right;">
                                    <sc-icon name="sc-star-line" />
                                    {{ item.likes }}
                                </el-col>
                            </el-row>
                            <el-row>
                                <el-col>
                                    <div class="ref" v-if="getReply(item.rid)">
                                        @用户名称:{{ getReply(item.rid).comment }}
                                    </div>
                                    <div class="comment">
                                        {{ item.comment }}
                                    </div>
                                </el-col>
                            </el-row>
                            <el-row>
                                <el-col>
                                    <el-button size="small" @click="showReplyInput(item)" v-if="showReply"
                                        style="margin-right: 10px;">
                                        {{ item.reply }} 回复 &gt;
                                    </el-button>
                                    <span class="time">
                                        <label style="line-height: 24px;">{{ this.$TOOL.dateTimeFormat(item.create_time)
                                        }}</label>
                                    </span>
                                </el-col>
                            </el-row>
                        </div>
                    </div>
                </div>
            </div>
            <div class="reply" v-if="toId != '0'">
                <myComment ref="reply" @save="saveComment"></myComment>
            </div>
        </div>
    </el-card>
</template>

<script>
import { defineAsyncComponent } from "vue";
import config from "@/config"

export default {
    components: {
        myComment: defineAsyncComponent(() => import("./comment.vue")),
    },
    namec: '评论',
    props: {
        code: { type: String, default: '0' },
        showReply: { type: Boolean, default: false },
    },
    data() {
        return {
            sort: '0',
            to: '',
            toId: '0',
            comment: {},
            list: [],
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        async init() {
            var res = await this.$API.scmmsgcomment.view.get({ 'codec': this.code });
            if (res == null || res.code != 200) {
                return;
            }

            this.comment = res.data || { qty: 0, details: [] };
            this.list = this.comment.details;
        },
        showReplyInput(item) {
            this.toId = item.id;

            var reply = this.$refs.reply;
            if (!reply) {
                return;
            }

            reply.replyName = item.namec;
            reply.replyContent = item.comment;
        },
        _inputShow(i) {
            return this.list[i].inputShow
        },
        async saveComment(comment) {
            var data = { 'codec': this.code, 'comment': comment, };
            var res = await this.$API.scmmsgcomment.save.post(data);
            if (res == null || res.code != 200) {
                return;
            }
            this.$message.success("保存成功");
        },
        getAvatar(avatar) {
            return config.SERVER_URL + avatar;
        },
        getReply(rid) {
            for (let index = 0; index < this.list.length; index++) {
                const element = this.list[index];
                if (element.id == rid) {
                    return element;
                }
            }
            return null;
        },
        dateStr(date) {
            //获取js 时间戳
            var time = new Date().getTime();
            //去掉 js 时间戳后三位，与php 时间戳保持一致
            time = parseInt((time - date) / 1000);
            //存储转换值 
            var s;
            if (time < 60 * 10) {//十分钟内
                return '刚刚';
            } else if ((time < 60 * 60) && (time >= 60 * 10)) {
                //超过十分钟少于1小时
                s = Math.floor(time / 60);
                return s + "分钟前";
            } else if ((time < 60 * 60 * 24) && (time >= 60 * 60)) {
                //超过1小时少于24小时
                s = Math.floor(time / 60 / 60);
                return s + "小时前";
            } else if ((time < 60 * 60 * 24 * 30) && (time >= 60 * 60 * 24)) {
                //超过1天少于30天内
                s = Math.floor(time / 60 / 60 / 24);
                return s + "天前";
            } else {
                //超过30天ddd
                var date0 = new Date(parseInt(date));
                return date0.getFullYear() + "/" + (date0.getMonth() + 1) + "/" + date0.getDate();
            }
        }
    },
}
</script>

<style scoped>
.sc-comment h3 {
    line-height: 31px;
}

.sc-comment .post .head {
    padding-bottom: 15px;
}

.sc-comment .reply {
    position: fixed;
    bottom: 0px;
    left: 0px;
    right: 0px;
    margin-left: 15px;
    margin-right: 15px;
    padding-left: 5px;
    padding-right: 5px;
    border: 1px solid #999;
    border-radius: 5px;
}

.sc-comment .list .head {
    padding-top: 15px;
}

.sc-comment-list {
    border-bottom: 1px dashed #efefef;
    padding-bottom: 15px;
}

.sc-comment-item {
    display: flex;
    flex-direction: row;
    flex: 1;
    flex-basis: auto;
    border-top: 1px dashed #efefef;
    margin-top: 15px;
}

.sc-comment-item .left {
    flex-shrink: 0;
    text-align: center;
    padding-top: 15px;
    padding-right: 15px;
}

.sc-comment-item .head {}

.sc-comment-item .body {
    flex-basis: 100%;
    margin-top: 10px;
}

.sc-comment-item .ref {
    background-color: #f8f8f8;
    border-radius: 5px;
    padding: 10px;
}

.sc-comment-item .comment {
    padding-top: 10px;
    padding-bottom: 10px;
}

.sc-comment-item .button {
    border-radius: 5px;
    border: 1px solid #999;
    background-color: #efefef;
    padding: 3px 8px;
}

.sc-comment-item .time {
    color: #666;
    padding-top: 4px;
}

.author-title:not(:last-child) {
    border-bottom: 1px solid rgba(178, 186, 194, 0.3);
}

.author-title {
    padding: 10px;
}

.author-title .header-img {
    display: inline-block;
    vertical-align: top;
}

.author-title .author-info {
    display: inline-block;
    margin-left: 5px;
    width: 60%;
    height: 40px;
    line-height: 20px;
}

.author-title .author-info>span {
    display: block;
    cursor: pointer;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
}

.author-title .author-info .author-namec {
    color: #000;
    font-size: 18px;
    font-weight: bold;
}

.author-title .author-info .author-time {
    font-size: 14px;
}

.author-title .icon-btn {
    width: 30%;
    padding: 0 !important;
    float: right;
    text-align: right;
}

@media screen and (max-width: 1200px) {
    .author-title .icon-btn {
        width: 20%;
        padding: 7px;
    }
}

.author-title .icon-btn>span {
    cursor: pointer;
}

.author-title .icon-btn .iconfont {
    margin: 0 5px;
}

.author-title .talk-box {
    margin: 0 50px;
}

.author-title .talk-box>p {
    margin: 0;
}

.author-title .talk-box .reply {
    font-size: 16px;
    color: #000;
}

.author-title .reply-box {
    margin: 10px 0 0 50px;
    background-color: #efefef;
}
</style>