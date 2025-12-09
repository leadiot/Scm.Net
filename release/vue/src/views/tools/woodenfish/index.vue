<template>
    <scWoodenfish />
</template>

<script>
import { defineAsyncComponent } from "vue";
export default {
    name: 'tools_woodenfish',
    components: {
        scWoodenfish: defineAsyncComponent(() => import("@/components/scWoodenfish")),
    },
    data() {
        return {
        }
    },
}
</script>
