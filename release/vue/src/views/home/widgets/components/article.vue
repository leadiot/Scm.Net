<template>
    <el-card shadow="hover" header="美文" class="item-background">
        <div class="time">
            <h2>{{ content }}</h2>
            <p>{{ author }}</p>
        </div>
    </el-card>
</template>

<script>
export default {
    title: "美文",
    icon: "el-icon-clock",
    description: "精美网文",
    data() {
        return {
            content: '',
            author: '',
            origin: '',
        }
    },
    mounted() {
        this.showData()
    },
    methods: {
        async showData() {
            var date = this.$SCM.getDate();
            var res = await this.$API.cmslogdailylitera.daily.get(date);
            if (res.code != 200) {
                return;
            }
            if (!res.data || !res.data.article) {
                return;
            }

            var article = res.data.article;
            this.author = article.author || '';
            this.origin = article.origin || '';

            var sections = article.sections;
            if (!sections) {
                return;
            }

            var content = '';
            sections.forEach(element => {
                content += element.content;
            });
            this.content = content;
        }
    }
}
</script>

<style scoped>
.item-background {
    background: linear-gradient(to right, #8E54E9, #4776E6);
    color: #fff;
}

.time h2 {
    font-size: 40px;
}

.time p {
    font-size: 14px;
    margin-top: 13px;
    opacity: 0.7;
}
</style>
