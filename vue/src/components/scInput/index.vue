<template>
    <el-autocomplete v-bind="$attrs" :loading="loading" :fetch-suggestions="querySearch" @select="handleSelect">
        <template #prepend v-if="prepend">{{ prepend }}</template>
        <template #append v-if="append">{{ append }}</template>
    </el-autocomplete>
</template>

<script>
import config from "@/config/select";

export default {
    props: {
        apiObj: { type: Object, default: () => { } },
        data: { type: Object, default: () => { } },
        prepend: { type: String, default: '' },
        append: { type: String, default: '' },
        params: { type: Object, default: () => ({}) },
    },
    data() {
        return {
            loading: false,
            options: [],
            props: config.props,
            param: {}
        }
    },
    created() {
        //如果有默认值就去请求接口获取options
        this.options = this.data;
        if (this.apiObj && this.apiObj.url) {
            this.getRemoteData()
        }
    },
    methods: {
        //获取数据
        async getRemoteData() {
            this.loading = true;
            let res = await this.apiObj.get();
            this.options = res.data;
            this.loading = false;
        },
        async querySearch(queryString, cb) {
            if (!queryString) {
                cb(this.data);
                return;
            }
            if (!this.apiObj || !this.apiObj.url) {
                cb(this.data);
                return;
            }
            let res = await this.apiObj.get(queryString);
            if (!res || !res.data) {
                return;
            }
            cb(res.data);
        },
        handleSelect(item) {
            console.log(item);
        }
    }
}
</script>
