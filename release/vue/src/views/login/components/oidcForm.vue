<template>
    <div id="oidc"></div>
</template>
<script>
export default {
    mounted() {
        var option = {
            state: 'login', style: 'card',
            columns: 4, showCard: false,
            response_type: 'code',
            redirect_uri: this.$CONFIG.OIDC_REDIRECT_URI
        };
        window.oidc.init(this.$CONFIG.OIDC_KEY, 'oidc', option);
    }
}
</script>