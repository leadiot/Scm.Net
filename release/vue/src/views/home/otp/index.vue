<template>
    <el-container>
        <el-header>
            <div class="left-panel">
                {{ title }}
            </div>
            <div class="right-panel">
                <el-button type="danger" @click="disable" v-if="otp_info.status == 1">禁用</el-button>
                <el-button type="primary" @click="enable" v-else>启用</el-button>
            </div>
        </el-header>
        <el-main>
            <div class="center">
                <el-card style="width: 480px">
                    <div class="card-body">
                        <el-alert type="success" :closable="false" v-if="otp_info.status == 1">
                            您已于{{ $TOOL.dateTimeFormat(otp_info.time) }}启用了授信登录。
                        </el-alert>
                        <el-alert type="primary" :closable="false" v-else>
                            您尚末启用授信登录！
                        </el-alert>
                    </div>
                    <div class="card-footer" style="text-align: right; margin-top: 1rem;" v-if="otp_info.status == 1">
                        <el-button type="primary" @click="auth">授权</el-button>
                        <el-button type="warning" @click="change">重新生成密钥</el-button>
                    </div>
                </el-card>
            </div>
        </el-main>
    </el-container>
</template>

<script>
export default {
    name: 'user_register',
    data() {
        return {
            title: "授信登录",
            otp_info: {
                status: 0,
                time: '0'
            }
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        async init() {
            var res = await this.$API.scmuruserotp.model.get();
            if (res.code != 200) {
                this.$message.warning(res.message);
                return false;
            }
            this.otp_info = res.data;
        },
        async enable() {
            var res = await this.$API.scmuruserotp.status.post({ status: 1 });
            if (res.code != 200) {
                this.$message.warning(res.message);
                return false;
            }

            this.otp_info = res.data;
            this.$message.success('授信登录已启用');
        },
        auth() {
            this.$router.push({
                path: '/dev/test2'
            });
        },
        disable() {
            this.$confirm('确认禁用授信登录吗？', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(async () => {
                var res = await this.$API.scmuruserotp.status.post({ status: 2 });
                if (res.code != 200) {
                    this.$message.warning(res.message);
                    return false;
                }

                this.otp_info = res.data;
                this.$message.success('授信登录已禁用');
            }).catch(() => {
                this.$message.warning('授信登录操作失败！');
            });
        },
        change() {
            this.$confirm('确认要重新生成密钥吗，此操作将导致之前的绑定账户失效？', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(async () => {
                var res = await this.$API.scmuruserotp.update.put();
                if (res.code != 200) {
                    this.$message.warning(res.message);
                    return false;
                }

                this.$message.success('密钥已经重新生成，请重新操作绑定！');
            }).catch(() => {
                this.$message.warning('授信登录操作失败！');
            });
        }
    }
}
</script>
<style scoped>
.center {
    display: flex;
    justify-content: center;
    align-items: center;
}
</style>