<template>
    <el-container>
        <el-main>
            <div style="width:800px; margin: 0 auto;">
                <sc-list :data="oauth_list" :hide-do="true">
                    <template #item="{ item }">
                        <div class="oauth-item">
                            <div class="thumb">
                                <el-avatar :size="50" :src="item.avatar" />
                            </div>
                            <div class="info">
                                <div class="label">{{ item.name }}</div>
                                <div class="value">{{ this.$TOOL.dateTimeFormat(item.create_time) }}</div>
                            </div>
                            <div class="opt">
                                <label class="provider">{{ item.provider }}</label>
                                <el-button type="danger" @click="del(item.id)">解绑</el-button>
                            </div>
                        </div>
                    </template>
                </sc-list>
                <div style="margin-top: 20px;">
                    <el-button type="primary" style="width: 100%;" :loading="isLoading" @click="add()">添加</el-button>
                </div>
            </div>
        </el-main>
    </el-container>
</template>
<script>
import config from "@/config";

export default {
    data() {
        return {
            oauth_list: [],// 已授权列表
            isLoading: false,// 绑定是否进行中
            oidc_option: {
                "mode": "spa",
                "view": "none",
                "style": "card",
                "state": "login",
                "columns": 5,
                "success": this.bind_success,
                "error": this.bind_error
            }
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        async init() {
            this.load();

            window.oidc.init(config.OIDC_KEY, "oidc", this.oidc_option);
        },
        async load() {
            var res = await this.$API.scmuruseroidc.list.get();
            if (!res || res.code != 200) {
                return;
            }

            this.oauth_list = res.data;
        },
        add() {
            this.isLoading = true;
            window.oidc.authroizeB();
        },
        del(id) {
            this.$confirm(
                `确定要操作解除绑定吗，解除后将无法正常使用三方登录？`,
                "提示",
                {
                    type: "warning",
                    confirmButtonText: "确定",
                    cancelButtonText: "取消",
                }
            )
                .then(() => {
                    this.unBind(id);
                })
                .catch(() => { });
        },
        bind_success(user) {
            this.doBind(user);
        },
        bind_error(message) {
            this.isLoading = false;
            this.$message.warning('联合登录绑定失败：' + message);
        },
        async doBind(user) {
            var res = await this.$API.scmuruseroidc.dobind.post(user);
            if (res.code != 200) {
                this.isLoading = false;
                this.$message.warning(res.message);
                return false;
            }

            this.isLoading = false;
            this.$message.success('联合登录解绑成功！');

            this.load();

            window.oidc.init(config.OIDC_KEY, "oidc", this.oidc_option);
        },
        async unBind(id) {
            var res = await this.$API.scmuruseroidc.unbind.post({ 'id': id });
            if (res.code != 200) {
                this.$message.warning(res.message);
                return false;
            }

            var data = res.data;
            if (data.code != 0) {
                this.$message.warning(data.message);
            }
            else {
                this.$message.success('联合登录解绑成功！');
            }

            this.load();
        },
    }
}
</script>

<style type="scss" scoped>
.oauth-item {
    display: flex;
    width: 100%;
    align-items: center;
    justify-content: space-between;
    font-size: 14px;
    padding: 10px;

    .thumb {
        width: 50px;
        height: 50px;
        padding: 1px;
    }

    .info {
        flex-basis: 100%;

        margin: 0 20px;

        .label {
            font-weight: bold;
        }

        .value {
            color: gray;
            margin-top: 5px;
        }

    }

    .opt {
        display: flex;
        align-items: center;

        .provider {
            color: #999;
            margin-right: 20px;
            width: 100px;
            text-align: right;
        }
    }
}
</style>
