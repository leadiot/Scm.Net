<template>
    <div @click="inputFocus" class="sc-comment-input">
        <div class="left" width="60px">
            <el-avatar class="header-img" :size="40" :src="myHeader"></el-avatar>
        </div>
        <div class="body">
            <div class="ref" v-if="replyName">
                @{{ replyName }}：{{ replyContent }}
            </div>
            <div class="reply">
                <div tabindex="0" contenteditable="true" spellcheck="false" :placeholder="placeholder"
                    class="reply-input" @focus="showReplyBtn" @input="onDivInput($event)" @blur="hideReplyBtn">
                </div>
            </div>
            <div v-if="btnShow" style="padding-top: 5px;">
                <el-row>
                    <el-col :span="16" style="text-align: left;">
                        <sc-icon name="sc-smile-line">表情</sc-icon>
                        <sc-icon name="sc-picture-line">图片</sc-icon>
                    </el-col>
                    <el-col :span="8" style="text-align: right;">
                        <el-button @click="sendComment" type="primary">发表评论</el-button>
                    </el-col>
                </el-row>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    emits: ['complete'],
    props: {
        placeholder: { type: String, default: "输入评论..." },
        content: { type: String, default: "" },
        replyName: { type: String, default: '' },
        replyContent: { type: String, default: '' }
    },
    data() {
        return {
            comment: '',
            btnShow: false,
            myHeader: 'https://ae01.alicdn.com/kf/Hd60a3f7c06fd47ae85624badd32ce54dv.jpg',
            replyInput: null,
        };
    },
    mounted() {
        this.comment = this.content;
    },
    methods: {
        inputFocus(obj) {
            if (!obj) {
                return;
            }

            // this.replyInput = obj.querySelector('.reply-input');
            // // this.replyInput.style.padding = "8px 8px"
            // // this.replyInput.style.border = "2px solid blue"
            // this.replyInput.focus()
        },
        showReplyBtn(obj) {
            this.replyInput = obj.target;
            this.btnShow = true
        },
        hideReplyBtn() {
            this.btnShow = !!this.comment;
            // this.replyInput.style.padding = "10px"
            // this.replyInput.style.border = "none"
        },
        onDivInput: function (e) {
            this.comment = e.target.innerHTML;
        },
        sendComment() {
            if (!this.comment) {
                this.$message({ showClose: true, type: 'warning', message: '评论不能为空' });
                return;
            }

            this.$emit('save', this.comment);
        },
        sendCommentReply(i) {
            if (!this.comment) {
                this.$message({ showClose: true, type: 'warning', message: '评论不能为空' });
                return;
            }

            this.$emit('save', this.comment, i);
        },
    }
}
</script>

<style>
.sc-comment-input {
    display: flex;
    flex-direction: row;
    flex: 1;
    flex-basis: auto;
    background-color: #fafbfc;
    text-align: center;
    padding: 10px;
}

.sc-comment-input .left {
    flex-shrink: 0;
    align-items: center;
}

.sc-comment-input .body {
    flex-basis: 100%;
    text-align: left;
}

.sc-comment-input .body .reply {
    display: inline-block;
    margin-left: 5px;
    width: 100%;
}

.sc-comment-input .body .reply .reply-input {
    min-height: 20px;
    line-height: 22px;
    padding: 10px 10px;
    color: #333;
    background-color: #fff;
    border-radius: 5px;
    border: 2px solid #efefef;
}

.sc-comment-input .body .reply .reply-input:empty:before {
    content: attr(placeholder);
}

.sc-comment-input .body .reply .reply-input:focus:before {
    content: none;
}

.sc-comment-input .body .reply .reply-input:focus {
    /* padding: 8px 8px;
    box-shadow: none;
    outline: none; */
    border: 2px solid #00f;
}
</style>