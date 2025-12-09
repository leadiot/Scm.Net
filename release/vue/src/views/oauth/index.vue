<template>
    <el-container>
        <el-header>
            <div class="common-header-left">
                <div class="common-header-logo">
                    <img :alt="$CONFIG.APP_NAME" src="img/logo.png">
                    <label>{{ $CONFIG.APP_NAME }}</label>
                </div>
                <div class="common-header-title">{{ title }}</div>
            </div>
            <div class="common-header-right">
                <router-link to="/login">返回登录</router-link>
            </div>
        </el-header>
        <el-main>
            <div class="common-container">
                <h2 class="common-title">{{ title }}</h2>
                <div class="common-main el-card">
                    <bind :code="code" v-if="state == 'bind'"></bind>
                    <login :code="code" v-else-if="state == 'login'"></login>
                    <error :code="code" v-else></error>
                </div>
            </div>
        </el-main>
    </el-container>
</template>
<script>
import { useRoute } from "vue-router";
import login from './components/login';
import bind from './components/bind';
import error from './components/error';

export default {
    name: 'oauth',
    components: {
        login,
        bind,
        error
    },
    data() {
        return {
            title: '联合登录',
            code: '',
            state: ''
        }
    },
    mounted() {
        var route = useRoute();

        this.code = route.query.code;
        var reg = /^[0-9a-zA-Z]{32}$/;
        if (!reg.test(this.code)) {
            this.$router.replace({ path: '/login' });
            return;
        }

        this.state = route.query.state;
        console.log(this.state)
    }
}
</script>