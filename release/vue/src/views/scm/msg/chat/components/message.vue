<template>
    <div class="message-container">
        <div :class="`message-wrap ${isMe(data) ? 'message-wrap_sender' : 'message-wrap_recipient'}`">
            <el-avatar class="avatar width-50" :size="40" :src="getAvatar(data)" />
            <div class="message-box">
                <div class="details">
                    <span class="nickname">
                        {{ getNamec(data) }}
                    </span>
                    <span class="time">{{ this.$TOOL.dateTimeFormat(data.create_time) }}</span>
                </div>
                <div class="content margin_t-10">
                    <span v-if="data.types === 1">{{ data.content }}</span>
                    <el-image v-if="data.types === 2" style="width: 100px; height: 100px" :src="data.url"
                        :preview-src-list="[data.url]" />
                    <div class="loading-icon-box" v-show="data.loading">
                        <el-icon class="loading-icon">
                        </el-icon>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    props: {
        data: { type: Object, default: () => { } },
        user: { type: Object, default: () => { } }
    },
    methods: {
        isMe(item) {
            if (!item || !this.user) {
                return false;
            }
            return item.user_id == this.user.id;
        },
        getNamec(item) {
            if (this.isMe(item)) {
                return this.user.namec;
            }
            var user = item.user;
            return user ? user.namec : 'unknown';
        },
        getAvatar(item) {
            var tmp = this.isMe(item) ? this.user : item.user;
            return this.$SCM.get_user_avatar(tmp);
        }
    }
}
</script>

<style lang="scss" scoped>
@import '@/assets/sass/_variable.scss';

.message-container {
    .message-wrap {
        display: flex;

        &_recipient {
            flex-direction: row;

            .message-box {
                margin-left: 10px;

                .details {
                    flex-direction: row;

                    .nickname {
                        margin-right: 10px;
                    }
                }

                .content {
                    flex-direction: row;
                    position: relative;

                    span::before {
                        content: '';
                        position: absolute;
                        left: -10px;
                        border-right: 5px solid $activeColor;
                        border-top: 5px solid transparent;
                        border-left: 5px solid transparent;
                        border-bottom: 5px solid transparent;
                    }
                }
            }
        }

        &_sender {
            flex-direction: row-reverse;

            .message-box {
                margin-right: 10px;

                .details {
                    flex-direction: row-reverse;

                    .nickname {
                        margin-left: 10px;
                    }
                }

                .content {
                    flex-direction: row-reverse;

                    span::before {
                        content: '';
                        position: absolute;
                        right: -10px;
                        border-left: 5px solid $activeColor;
                        border-top: 5px solid transparent;
                        border-right: 5px solid transparent;
                        border-bottom: 5px solid transparent;
                    }

                    .loading-icon-box {
                        position: relative;

                        .loading-icon {
                            position: absolute;
                            top: 50%;
                            right: 5px;
                            transform: translateY(-50%);
                        }
                    }
                }
            }

        }

        .message-box {
            flex: 1;
            width: 80%;

            .details {
                display: flex;
                font-size: 12px;

                .nickname {
                    color: $darkColor-9;
                }

                .time {
                    color: $darkColor-6;
                }
            }

            .content {
                display: flex;

                span {
                    position: relative;
                    padding: 10px;
                    font-size: 14px;
                    color: $lightColor-0;
                    text-align: left;
                    background-color: $activeColor;
                    border-radius: 4px;
                    max-width: 50%;
                    word-wrap: break-word;
                }
            }
        }
    }
}
</style>