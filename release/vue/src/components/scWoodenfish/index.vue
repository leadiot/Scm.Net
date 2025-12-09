<template>
    <div class="sc-woodenfish">
        <div class="sc-woodenfish_container">
            <img class="sc-woodenfish_img" :src="getUrl(image)" alt="木鱼" @click="click"
                :class="animate ? 'sc-woodenfish_size' : ''" />
            <div class="sc-woodenfish_text">
                <p v-for="text in textList" :key="text">{{ text }}</p>
            </div>
        </div>
        <audio ref="audio" class="audio">
            <source :src="getUrl(audio)" />
        </audio>
    </div>
</template>

<script>
export default {
    props: {
        image: { type: String, default: '/data/woodenfish/wooden_fish.png' },
        audio: { type: String, default: '/data/woodenfish/wooden_fish.wav' },
        bgAudio: { type: String, default: '' },
    },
    data() {
        return {
            animate: false,
            textList: [],
            dd: false,
            last: 0,
        }
    },
    mounted() {
        this.init();
    },
    methods: {
        init() {
        },
        getUrl(url) {
            if (url.startsWith('/')) {
                return this.$CONFIG.SERVER_URL + url;
            }
            return url;
        },
        changeSize() {
            this.animate = true;
            setTimeout(() => { this.animate = false; }, 300)
        },
        appendText() {
            // if (this.textList.length > 10) {
            //     this.textList.shift();
            // }
            this.textList.push('功德 +1');
        },
        playAudio() {
            this.$refs.audio.play();
        },
        vibrate() {
            if (!this.dd) {
                return;
            }

            var navigator = window.navigator
            if (!("vibrate" in navigator)) {
                return;
            }

            navigator.vibrate = navigator.vibrate || navigator.webkitVibrate || navigator.mozVibrate || navigator.msVibrate;
            if (!navigator.vibrate) {
                return;
            }

            navigator.vibrate(300)
        },
        click() {
            // var now = new Date();
            // var time = now.getTime();
            // if ((time - this.last) < 300) {
            //     return;
            // }

            // this.last = time;

            this.changeSize();
            this.appendText();
            this.playAudio();
            this.vibrate();
        }
    }
}
</script>
<style scoped>
.sc-woodenfish {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 100%;
    min-height: 600px;
    background-color: #000;
}


.sc-woodenfish_container {
    width: 300px;
    height: 230px;
    position: relative;
}

.sc-woodenfish_img {
    width: 100%;
    height: 100%;
}

.sc-woodenfish_text p {
    width: 100%;
    font-size: 28px;
    color: #fff;
    text-align: center;
    position: absolute;
    animation: woodenfish-move 1s forwards;
}

.sc-woodenfish_size {
    animation: woodenfish-size 0.3s;
}

@keyframes woodenfish-move {
    0% {
        top: -60px;
        opacity: 1;
    }

    100% {
        top: -110px;
        opacity: 0;
    }
}

@keyframes woodenfish-size {
    0% {
        transform: scale(1);
    }

    50% {
        transform: scale(0.95);
    }

    100% {
        transform: scale(1);
    }
}

@media (min-width: 768px) {
    .sc-woodenfish_container {
        width: 500px;
        height: 380px;
    }
}
</style>