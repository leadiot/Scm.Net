<template>
    <div class="sc-text-viewer" v-if="visible">
        <div class="sc-text-viewer_mask"></div>
        <span class="el-image-viewer__btn el-image-viewer__close" @click="close">
            <sc-icon name="sc-close-circle-line" />
        </span>
        <div class="sc-text-viewer_body">
            <div class="sc-text-viewer_title">
                <span>{{ title }}</span>
                <span>{{ create_time }}</span>
            </div>
            <div class="sc-text-viewer_content">
                <el-input v-model="content" type="textarea" style="min-height: 800px;" :readonly="true"></el-input>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    props: {
        Uri: { type: String, default: '' },
        Name: { type: String, default: '' },
        Data: { type: String, default: '' }
    },
    data() {
        return {
            uri: '',
            title: '',
            content: '',
            create_time: '',
            visible: false,
        }
    },
    mounted() {
    },
    methods: {
        showUri(uri) {
            this.uri = uri;
            this.visible = true;
            this.loadText(uri);
        },
        showText(title, content, createTime) {
            this.title = title;
            this.content = content;
            this.create_time = createTime;
            this.visible = true;
        },
        loadText(uri) {
            if (!uri) {
                return;
            }

            this.title = 'title';
            this.content = 'content';
        },
        close() {
            this.visible = false;
        }
    }
}
</script>
<style scoped>
.sc-text-viewer {
    background-color: #333;
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    left: 0;
    z-index: 1000;
}

.el-text-viewer__mask {
    position: absolute;
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
    opacity: .5;
    background: #000;
}

.el-text-viewer__btn {
    position: absolute;
    z-index: 1;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 50%;
    opacity: .8;
    cursor: pointer;
    box-sizing: border-box;
    -webkit-user-select: none;
    user-select: none;
}

.el-text-viewer__close {
    top: 40px;
    right: 40px;
    width: 40px;
    height: 40px;
    font-size: 40px;
}

.el-text-viewer__close {
    width: 44px;
    height: 44px;
    font-size: 24px;
    color: #fff;
    background-color: var(--el-text-color-regular);
    border-color: #fff;
}

.sc-text-viewer_body {
    top: 40px;
    right: 40px;
    max-width: 1024px;
    margin: 80px auto 40px auto;
    background-color: white;
    height: 100%;
}

.sc-text-viewer_title {
    font-size: 18px;
}

.sc-text-viewer_content {
    min-height: 800px;
}
</style>