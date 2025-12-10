<template>
    <div class="dv">
        <div class="d-flex">
            <div>adadf</div>
            <div class="title">
                <div class="title-text">
                    adfadf
                </div>
            </div>
            <div>adadf</div>
        </div>
        <sc-video
            src="https://sf1-cdn-tos.huoshanstatic.com/obj/media-fe/xgplayer_doc_video/mp4/xgplayer-demo-360p.mp4"></sc-video>
    </div>
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
    name: 'dev_version',
    components: {
        scVideo: defineAsyncComponent(() => import("@/components/scVideo")),
    },
    data() {
        return {
        };
    },
    mounted() {
    },
    methods: {
    },
};
</script>
<style scoped>
.dv {
    width: 100%;
    height: 100%;
    color: #d3d6dd;
    padding: 16px 16px 0 16px;
    background-color: #000000;
    background-image: url("@/assets/dv.png");
    background-size: cover;
    background-position: center center;
}

.d-flex {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
}

.title {
    text-align: center;

    .title-text {
        font-size: 24px;
    }
}
</style>