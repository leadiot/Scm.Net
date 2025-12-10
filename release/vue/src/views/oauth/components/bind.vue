<template>
    <div class="notice">
        <div class="warning">
            {{ error_msg }}
        </div>

        <a href="/#/home/oauth">返回联合登录页面</a>
    </div>
</template>
<script>
export default {
    props: {
        code: { type: String, default: '' }
    },
    data() {
        return {
            error_code: 0,
            error_msg: ''
        }
    },
    mounted() {
        this.init(this.code);
    },
    methods: {
        async init(code) {
            var res = await this.$API.scmuruseroidc.dobind.post({ 'key': code });
            if (res.code != 200) {
                this.$message.warning(res.message);
                return false;
            }

            var data = res.data;
            if (data.code != 0) {
                this.error_code = data.code;
                this.error_msg = data.message;
                return;
            }

            this.error_msg = '联合登录绑定成功，请关闭当前窗口！';
        }
    }
}
</script>
<style type="scss">
.notice {
    display: flex;
    flex-direction: column;
    justify-content: center;
    min-height: 320px;
    text-align: center;

    .warning {
        font-size: 26px;
        margin: 20px;
        color: red;
    }
}
</style>